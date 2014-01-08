using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISP.GestaoMatriculas.Model;
using ISP.GestaoMatriculas.Repositories;
using ISP.GestaoMatriculas.Model.Exceptions;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Threading;

namespace ISP.GestaoMatriculas.Utils
{
    public static class EventoStaggingFactory
    {

        public static EventoStagging duplicarEventoStagging(string matricula, int seguradoraId, string dtInicio, string hInicio, string operacao, Mutex mutex = null)
        {
            ValorSistemaRepository valoresSistemaRepository = new ValorSistemaRepository();

            List<ValorSistema> tiposErro = valoresSistemaRepository.GetPorTipologia("TIPO_ERRO", mutex);

            List<ErroEventoStagging> errosEventoStagging = new List<ErroEventoStagging>();

            if (matricula == null || matricula == string.Empty)
            {
                errosEventoStagging.Add(new ErroEventoStagging { campo = "matricula", descricao = "Matrícula Inválida", tipologiaId = tiposErro.Where(e => e.valor == "GENERICO").Single().valorSistemaId });
            }
            if (dtInicio == null || dtInicio == string.Empty)
            {
                errosEventoStagging.Add(new ErroEventoStagging { campo = "dataInicio", descricao = "Data de início de período Inválida", tipologiaId = tiposErro.Where(e => e.valor == "GENERICO").Single().valorSistemaId });
            }
            if (hInicio == null || hInicio == string.Empty)
            {
                errosEventoStagging.Add(new ErroEventoStagging { campo = "horaInicio", descricao = "Hora de início de período Inválida", tipologiaId = tiposErro.Where(e => e.valor == "GENERICO").Single().valorSistemaId });
            }

            if (errosEventoStagging.Count > 0)
                throw new ErroEventoStaggingException { errosEventoStagging = errosEventoStagging};

            EventoStaggingRepository eventosStaggingRepository = new EventoStaggingRepository();

            List<EventoStagging> eventosStagging =  eventosStaggingRepository.All.Where(e => 
                                                    e.matricula == matricula 
                                                    && e.entidadeId == seguradoraId 
                                                    && e.dataInicioCobertura == dtInicio 
                                                    && e.horaInicioCobertura == hInicio 
                                                    && e.estadoEvento.valor == "ERRO"
                                                    && e.arquivado == false
                                                    && e.codigoOperacao == operacao).ToList();

            if (eventosStagging == null || eventosStagging.Count == 0)
            {
                return null;
                //return new EventoStagging() - Construtor vazio nao tem base
            }

            if (eventosStagging.Count > 1)
            {
                //Erro interno; Não deveria existir esta situação
            }

            return new EventoStagging(eventosStagging.First());
            //new EventoStagging(eventosStagging.First())
            //Adicionar Erros
        }

        public static EventoStagging duplicarEventoProducao(string matricula, int seguradoraId, string dtInicio, string hInicio, Mutex mutex = null)
        {
            ValorSistemaRepository valoresSistemaRepository = new ValorSistemaRepository();
            List<ValorSistema> tiposErro = valoresSistemaRepository.GetPorTipologia("TIPO_ERRO", mutex);
            List<ValorSistema> estadosEvento = valoresSistemaRepository.GetPorTipologia("ESTADO_EVENTO_STAGGING", mutex);

            DateTime dataInicio;
            TimeSpan horaInicio;

            DateTime dataHoraInicio;

            List<ErroEventoStagging> errosEventoStagging = new List<ErroEventoStagging>();

            if (matricula == null || matricula == string.Empty)
            {
                errosEventoStagging.Add(new ErroEventoStagging { campo = "matricula", descricao = "Matrícula Inválida", tipologiaId = tiposErro.Where(e => e.valor == "GENERICO").Single().valorSistemaId });
            }
            if (!DateTime.TryParseExact(dtInicio, "yyyyMMdd", new CultureInfo("pt-PT"), DateTimeStyles.None, out dataInicio))
            {
                errosEventoStagging.Add(new ErroEventoStagging { campo = "dataInicio", descricao = "Data de início de período Inválida", tipologiaId = tiposErro.Where(e => e.valor == "GENERICO").Single().valorSistemaId });
            }
            if (!TimeSpan.TryParseExact(hInicio, "hhmmss", new CultureInfo("pt-PT"), TimeSpanStyles.None, out horaInicio))
            {
                errosEventoStagging.Add(new ErroEventoStagging { campo = "horaInicio", descricao = "Hora de início de período Inválida", tipologiaId = tiposErro.Where(e => e.valor == "GENERICO").Single().valorSistemaId });
            }

            if (errosEventoStagging.Count > 0)
                throw new ErroEventoStaggingException { errosEventoStagging = errosEventoStagging };

            dataHoraInicio = dataInicio.Add(horaInicio);

            ApoliceRepository apolicesRepository = new ApoliceRepository();

            List<Apolice> apolicesProducao = apolicesRepository.All.Include("veiculo").Include("veiculo.categoria").Include("tomador")
                                                    .Include("concelho").Include("eventoHistorico").Where(e =>
                                                        e.veiculo.numeroMatricula == matricula
                                                        && e.entidadeId == seguradoraId
                                                        && e.dataInicio == dataHoraInicio).ToList();

            if (apolicesProducao == null || apolicesProducao.Count == 0)
            {
                return new EventoStagging { entidadeId = seguradoraId};
                //return new EventoStagging() - Construtor vazio nao tem base
            }

            if (apolicesProducao.Count > 1)
            {
                //Erro interno; Não deveria existir esta situação
            }

            return new EventoStagging(apolicesProducao.First(), estadosEvento.Where(e=>e.valor == "PENDENTE").Single().valorSistemaId);
            //return new EventoStagging(eventosProducao.First())
        }

        public static EventoStagging criarEventoStagging(EventoStagging eventoPendente, Mutex mutex = null)
        {
            ValorSistemaRepository valoresSistemaRepository = new ValorSistemaRepository();
            List<ValorSistema> tiposErro = valoresSistemaRepository.GetPorTipologia("TIPO_ERRO", mutex);

            EventoStagging novoEvento = null;

            if (eventoPendente.codigoOperacao == null)
            {
                throw new ErroEventoStaggingException { errosEventoStagging = new List<ErroEventoStagging> { new ErroEventoStagging { campo = "CodigoOperacao", descricao = "Código de Operação Inválido", tipologiaId = tiposErro.Where(e => e.valor == "GENERICO").Single().valorSistemaId } } };
            }

            try
            {              
                novoEvento = EventoStaggingFactory.duplicarEventoStagging(eventoPendente.matricula, (int)eventoPendente.entidadeId, eventoPendente.dataInicioCobertura, eventoPendente.horaInicioCobertura, eventoPendente.codigoOperacao, mutex);

                //Caso não exista registo em stagging duplica-se o Registo em Producao               
                //novoEvento = EventoStaggingFactory.duplicarEventoProducao(eventoPendente.matricula, (int)eventoPendente.entidadeId, eventoPendente.dataInicioCobertura, eventoPendente.horaInicioCobertura);
                
                //Caso este também não exista, trata-se de uma nova ocorrência.
                //Poderiamos concluir que se não é uma criação é um erro, mas validemos isto juntamente com as validações de negócio.
                if (novoEvento == null)
                {
                    novoEvento = new EventoStagging();

                    if (eventoPendente.ficheiroID != null)
                    {
                        novoEvento.dataReporte = eventoPendente.ficheiro.dataAlteracao;
                        novoEvento.dataUltimaAlteracaoErro = eventoPendente.ficheiro.dataAlteracao;
                    }
                }
            }
            catch (ErroEventoStaggingException erros)
            {
                novoEvento = new EventoStagging();
                if (eventoPendente.ficheiroID != null)
                {
                    novoEvento.dataReporte = eventoPendente.ficheiro.dataAlteracao;
                    novoEvento.dataUltimaAlteracaoErro = eventoPendente.ficheiro.dataAlteracao;
                }
                
                foreach (ErroEventoStagging erro in erros.errosEventoStagging)
                {
                    erro.eventoStagging = novoEvento;
                    novoEvento.errosEventoStagging.Add(erro);
                }
            }

            novoEvento.esmagaDados(eventoPendente);

            return novoEvento;
        }

    }
}
