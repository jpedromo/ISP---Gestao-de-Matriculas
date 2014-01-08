using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using ISP.GestaoMatriculas.Model;
using ISP.GestaoMatriculas.Repositories;
using LinqToExcel;
using System.Globalization;
using ISP.GestaoMatriculas.Model.Exceptions;

namespace ISP.GestaoMatriculas.Utils
{
    public static class ProcessadorFicheiros
    {
        public static void processaFicheiro(Ficheiro ficheiro){

            ValorSistemaRepository valoresSistemaRepository = new ValorSistemaRepository();

            //List<ValorSistema> valSistema = valoresSistemaRepository.All.Where(v => v.tipologia == "ESTADO_EVENTO_STAGGING" || v.tipologia == "OPERACAO_EVENTO").ToList();

            //List<ValorSistema> estadosEvento = valSistema.Where(v => v.tipologia == "ESTADO_EVENTO_STAGGING").ToList();
            //List<ValorSistema> operacoesEvento = valSistema.Where(v => v.tipologia == "OPERACAO_EVENTO").ToList();

            List<ValorSistema> estadosEvento = valoresSistemaRepository.GetPorTipologia("ESTADO_EVENTO_STAGGING");
            List<ValorSistema> operacoesEvento = valoresSistemaRepository.GetPorTipologia("OPERACAO_EVENTO");
            List<ValorSistema> tiposErro = valoresSistemaRepository.GetPorTipologia("TIPO_ERRO");
            List<ValorSistema> estadosFicheiro = valoresSistemaRepository.GetPorTipologia("ESTADO_FICHEIRO");

            ReporteOcorrenciasMatricula reporte;

            XmlSerializer serializer = new XmlSerializer(typeof(ReporteOcorrenciasMatricula));
            using (FileStream fileStream = new FileStream(ficheiro.localizacao, FileMode.Open))
            {
                reporte = (ReporteOcorrenciasMatricula)serializer.Deserialize(fileStream);
            }

            if (reporte.Cabecalho.CodigoEstatistico != ficheiro.entidade.codigoEntidade)
            {
                ErroFicheiro erroFicheiro = new ErroFicheiro(){ dataValidacao = DateTime.Now, 
                                                                tipologiaId = tiposErro.Single(e => e.valor == "GENERICO").valorSistemaId,
                                                                descricao = "Ficheiro reportado pela entidade com código '" + ficheiro.entidade.codigoEntidade + "' e com registos relativos à entidade '" + reporte.Cabecalho.CodigoEstatistico + "'.",
                                                                ficheiroId = ficheiro.ficheiroId };

                ErroFicheiroException ex = new ErroFicheiroException();
                ex.errosFicheiro.Add(erroFicheiro);

                throw ex;
            }

            ReporteOcorrenciasFNMPAS reporteOcorrenciasFNMPAS = new ReporteOcorrenciasFNMPAS(reporte);
            reporteOcorrenciasFNMPAS.ordenaOcorrencias();
           

            for (int i = 0; i < reporteOcorrenciasFNMPAS.OcorrenciaOrdenada.Count(); i++)
            {
                ReporteOcorrenciasMatriculaOcorrenciaOrdenada ocorrencia = reporteOcorrenciasFNMPAS.OcorrenciaOrdenada[i];

                //evento Duplicado e esmagado com os novos dados. (C M - stagging/producao) (A - producao/stagging) 
                //Não foi ainda inserido na Base de dados nem houve delete logico do anterior. 
                //Fazer deletes logicos, e inserções após verificações.
                //Anulações podem necessitar logica adicional caso a validação falhe. (duplicar registo em stagging)
                EventoStagging evento = new EventoStagging();
                
                int codigoOperacaoId = 0;
                switch (ocorrencia.CodigoOperacao)
                {
                    case ReporteOcorrenciasMatriculaOcorrenciaCodigoOperacao.C: codigoOperacaoId = operacoesEvento.Where(o=> o.valor =="C").Single().valorSistemaId; break;
                    case ReporteOcorrenciasMatriculaOcorrenciaCodigoOperacao.M: codigoOperacaoId = operacoesEvento.Where(o=> o.valor =="M").Single().valorSistemaId; break;
                    case ReporteOcorrenciasMatriculaOcorrenciaCodigoOperacao.A: codigoOperacaoId = operacoesEvento.Where(o=> o.valor =="A").Single().valorSistemaId; break;
                    default: codigoOperacaoId = operacoesEvento.Where(o => o.valor == "C").Single().valorSistemaId; break;
                }
                evento.esmagaDados(ocorrencia, ficheiro, codigoOperacaoId);

                evento.estadoEventoId = estadosEvento.Where(e => e.valor == "PENDENTE").Single().valorSistemaId;

                ficheiro.eventosStagging.Add(evento);
            }

            //EventoStaggingRepository eventosStaggingRepository = new EventoStaggingRepository();
            //eventosStaggingRepository.InsertOrUpdate(evento);
            //eventosStaggingRepository.Save();
        }


        public static void processaFicheiroIsentos(FicheiroIsentos ficheiro)
        {
            //FicheiroIsentosRepository ficheirosIsentosRepository = new FicheiroIsentosRepository();
            ValorSistemaRepository valoresSistemaRepository = new ValorSistemaRepository();
            ApoliceIsentoRepository apolicesIsentoRepository = new ApoliceIsentoRepository();
            NotificacaoRepository notificacoesRepository = new NotificacaoRepository();
            
            //List<ValorSistema> valoresSistema = valoresSistemaRepository.All.Where(v => v.tipologia == "TIPO_AVISO" || v.tipologia == "ESTADO_FICHEIRO").ToList();
            //ValorSistema tipoNotificacao = valoresSistema.Where(v => v.tipologia == "TIPO_AVISO" && v.valor == "GENERICO").Single();
            //ValorSistema valErroFicheiro = valoresSistema.Where(v => v.tipologia == "ESTADO_FICHEIRO" && v.valor == "ERRO").Single();
            //ValorSistema valProcessadoFicheiro = valoresSistema.Where(v => v.tipologia == "ESTADO_FICHEIRO" && v.valor == "PROCESSADO").Single();

            ValorSistema tipoNotificacao = valoresSistemaRepository.GetPorTipologia("TIPO_AVISO").Where(v => v.valor == "GENERICO").Single();
            ValorSistema valErroFicheiro = valoresSistemaRepository.GetPorTipologia("ESTADO_FICHEIRO").Where(v => v.valor == "ERRO").Single();
            ValorSistema valProcessadoFicheiro = valoresSistemaRepository.GetPorTipologia("ESTADO_FICHEIRO").Where(v => v.valor == "PROCESSADO").Single();

            //Insere os novos registos de períodos isentos que são reportados no ficheiro
            bool erroFicheiro = false;
            DateTime dataRegisto = DateTime.Now;
            DateTime dataInicioTemp;
            DateTime? dataFimTemp;
            DateTime dataAux;
            var excel = new ExcelQueryFactory(ficheiro.localizacao);
            excel.AddMapping<RegistoExcelMatriculaIsenta>(x => x.matricula, "MATRICULA"); // ou excel.AddMapping("matricula", "MATRICULA");
            excel.AddMapping<RegistoExcelMatriculaIsenta>(x => x.mensagem, "MENSAGEM");
            excel.AddMapping<RegistoExcelMatriculaIsenta>(x => x.entidadeResponsavel, "ENTIDADE");
            excel.AddMapping<RegistoExcelMatriculaIsenta>(x => x.dataInicio, "DATA_INICIO");
            excel.AddMapping<RegistoExcelMatriculaIsenta>(x => x.dataFim, "DATA_FIM");
            excel.AddMapping<RegistoExcelMatriculaIsenta>(x => x.confidencial, "CONFIDENCIAL"); 
            
            var matriculasIsentas = from m in excel.Worksheet<RegistoExcelMatriculaIsenta>("Resultados")
                                   select m;
            
            foreach (RegistoExcelMatriculaIsenta mat in matriculasIsentas)
            {
                if (mat.matricula == null || mat.matricula == string.Empty || ! DateTime.TryParseExact(mat.dataInicio, "yyyyMMdd", new CultureInfo("pt-PT"), DateTimeStyles.None, out dataInicioTemp))
                {
                    erroFicheiro = true;
                    break;
                }
                
                if (DateTime.TryParseExact(mat.dataFim, "yyyyMMdd", new CultureInfo("pt-PT"), DateTimeStyles.None, out dataAux))
                    dataFimTemp = dataAux;
                else
                    dataFimTemp = null;

                ApoliceIsento isento = new ApoliceIsento()
                {
                    entidadeId = ficheiro.entidadeId.Value,
                    matricula = mat.matricula,
                    matriculaCorrigida = mat.matricula.Replace("-",""),
                    entidadeResponsavel = mat.entidadeResponsavel,
                    mensagem = mat.mensagem,
                    dataInicio = dataInicioTemp,
                    dataFim = dataFimTemp,
                    confidencial = mat.confidencial,
                    origemFicheiro = true,
                    arquivo = false,
                    dataReporte = ficheiro.dataUpload,
                    dataCriacao = dataRegisto,
                    dataModificacao = dataRegisto,
                };

                ficheiro.apolicesIsentos.Add(isento);
            }

            Notificacao notif = new Notificacao();
            if (!erroFicheiro)
            {

                //Coloca para arquivo todos os registos de períodos isentos que foram inseridos via ficheiro.
                List<ApoliceIsento> periodosAtivos = apolicesIsentoRepository.All.Where(m => m.arquivo == false && m.origemFicheiro == true && m.dataCriacao < dataRegisto).ToList();
                if (periodosAtivos != null && periodosAtivos.Count > 0)
                {
                    foreach(ApoliceIsento isento in periodosAtivos)
                    {
                        isento.arquivo = true;
                        apolicesIsentoRepository.InsertOrUpdate(isento);
                    }
                }

                apolicesIsentoRepository.Save();

                string mensagemNotificacao = string.Format("O ficheiro de reporte e matriculas isentas de seguro com o nome {0} e submetido na data {1}, " +
                                                            "foi processado com sucesso sendo registados {2} matriculas.",
                                                            ficheiro.nomeFicheiro, 
                                                            ficheiro.dataUpload.ToString(),
                                                            ficheiro.apolicesIsentos.Count);

                notif.dataCriacao = DateTime.Now;
                notif.email = false;
                notif.entidadeId = ficheiro.entidadeId;
                notif.mensagem = mensagemNotificacao;
                notif.tipologiaId = tipoNotificacao.valorSistemaId;
                ficheiro.estadoId = valProcessadoFicheiro.valorSistemaId;
            }
            else
            {
                string mensagemNotificacao = string.Format("O ficheiro de reporte e matriculas isentas de seguro com o nome {0} e submetido na data {1}, " +
                                                            "não foi processado devido a um erro da informação relativa ao número de matrícula ou à data de início do período.",
                                                            ficheiro.nomeFicheiro, ficheiro.dataUpload.ToString());
                
                notif.dataCriacao = DateTime.Now;
                notif.email = false;
                notif.entidadeId = ficheiro.entidadeId;
                notif.mensagem = mensagemNotificacao;
                notif.tipologiaId = tipoNotificacao.valorSistemaId;
                               
                ficheiro.estadoId = valErroFicheiro.valorSistemaId;
            }

            notificacoesRepository.InsertOrUpdate(notif);
            notificacoesRepository.Save();
            //ficheirosIsentosRepository.InsertOrUpdate(ficheiro);
            //ficheirosIsentosRepository.Save();
        }
    }




    public class RegistoExcelMatriculaIsenta
    {
        //Key - Identificador Interno
        public string matricula { get; set; }
        public string entidadeResponsavel { get; set; }
        public string mensagem { get; set; }
        public string dataInicio { get; set; }
        public string dataFim { get; set; }
        public bool confidencial { get; set; }
    }
}
