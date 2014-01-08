using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ISP.GestaoMatriculas.Model;
using System.Text.RegularExpressions;
using ISP.GestaoMatriculas.Repositories;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Threading;

namespace ISP.GestaoMatriculas.Utils
{
    public static class ValidacaoEventos
    {
        public static bool validarCampos(EventoStagging eventoStagging, Mutex mutex = null)
        {
            ValorSistemaRepository valoresSistemaRepository = new ValorSistemaRepository();
            CategoriaRepository categoriasRepository = new CategoriaRepository();
            ConcelhoRepository concelhosRepository = new ConcelhoRepository();

            List<ValorSistema> tiposErro = valoresSistemaRepository.GetPorTipologia("TIPO_ERRO", mutex);
            List<ValorSistema> tiposAviso = valoresSistemaRepository.GetPorTipologia("TIPO_AVISO", mutex);

            int tipoErroGenerico = tiposErro.Where(e => e.valor == "GENERICO").Single().valorSistemaId;
            int tipoAvisoGenerico = tiposAviso.Where(e => e.valor == "GENERICO").Single().valorSistemaId;

            DateTime aux;
            int intAux;

            //ERROS
            //tem Operação válida
            if (eventoStagging.codigoOperacao == null)
            {
                eventoStagging.errosEventoStagging.Add(new ErroEventoStagging { campo = "CodigoOperacao",
                    descricao = "Código de operação '" + eventoStagging.codigoOperacao + "' inválido.",
                                                                                tipologiaId = tipoErroGenerico,
                                                                                eventoStagging = eventoStagging
                });
            }
            //tem matrícula válida - check
            if (eventoStagging.matricula == null || !Regex.Match(eventoStagging.matricula, "[-\\/?#() A-Za-z0-9]+").Success)
            {
                eventoStagging.errosEventoStagging.Add(new ErroEventoStagging { campo = "NumeroMatricula",
                    descricao = "Número de matrícula '" + eventoStagging.matricula + "' inválido.",
                                                                                tipologiaId = tipoErroGenerico,
                                                                                eventoStagging = eventoStagging
                });
            }
            //tem data inicio válida - check
           
            if (eventoStagging.dataInicioCobertura == null || eventoStagging.dataInicioCobertura == string.Empty || !DateTime.TryParseExact(eventoStagging.dataInicioCobertura, "yyyyMMdd", new CultureInfo("pt-PT"), DateTimeStyles.None, out aux))
            {
                eventoStagging.errosEventoStagging.Add(new ErroEventoStagging { campo = "DataInicio",
                    descricao = "Data de início do período de cobertura '" + eventoStagging.dataInicioCobertura + "' inválida.",
                                                                                tipologiaId = tipoErroGenerico,
                                                                                eventoStagging = eventoStagging
                });
            }
            //tem data fim válida - check
            if (eventoStagging.dataFimCobertura == null || eventoStagging.dataFimCobertura == string.Empty || !DateTime.TryParseExact(eventoStagging.dataFimCobertura, "yyyyMMdd", new CultureInfo("pt-PT"), DateTimeStyles.None, out aux))
            {
                eventoStagging.errosEventoStagging.Add(new ErroEventoStagging { campo = "DataFim",
                    descricao = "Data de fim do período de cobertura '" + eventoStagging.dataInicioCobertura + "' inválida.",
                                                                                tipologiaId = tipoErroGenerico,
                                                                                eventoStagging = eventoStagging
                });
            }
            //sem numero de apolice e sem certificado provisório - check
            if (eventoStagging.nrApolice == null || eventoStagging.nrApolice == string.Empty)
            {
                if (eventoStagging.nrCertificadoProvisorio == null || eventoStagging.nrCertificadoProvisorio == string.Empty)
                {
                    eventoStagging.errosEventoStagging.Add(new ErroEventoStagging { campo = "NumeroApolice",
                        descricao = "Número de apólice ou Número de certificado provisório em falta.",
                                                                                    tipologiaId = tipoErroGenerico,
                                                                                    eventoStagging = eventoStagging
                    });
                }
            }

            //AVISOS
            //tem marca - check
            if (eventoStagging.marca == null || eventoStagging.marca == string.Empty)
            {
                eventoStagging.avisosEventoStagging.Add(new Aviso { campo = "MarcaVeiculo",
                    descricao = "Marca de veiculo '"+eventoStagging.marca+"' inválida.",
                                                                    tipologiaId = tipoAvisoGenerico,
                                                                    eventoStagging = eventoStagging
                });
            }
            //tem modelo - check
            if (eventoStagging.modelo == null || eventoStagging.modelo == string.Empty)
            {
                eventoStagging.avisosEventoStagging.Add(new Aviso { campo = "ModeloVeiculo",
                    descricao = "Modelo de veiculo '"+eventoStagging.modelo+"' inválido.",
                                                                    tipologiaId = tipoAvisoGenerico,
                                                                    eventoStagging = eventoStagging
                });
            }
            //tem ano construção válido - check
            if (eventoStagging.anoConstrucao == null || eventoStagging.anoConstrucao == string.Empty || !int.TryParse(eventoStagging.anoConstrucao, out intAux) /*|| intAux < 0 || intAux > (DateTime.Now.Year+2)*/)
            {
                eventoStagging.avisosEventoStagging.Add(new Aviso { campo = "AnoConstrucaoVeiculo",
                    descricao = "Ano de construção do veiculo '"+eventoStagging.anoConstrucao+"' inválido.",
                                                                    tipologiaId = tipoAvisoGenerico,
                                                                    eventoStagging = eventoStagging
                });
            }
            //tem categoria válida - check
            if (eventoStagging.codigoCategoriaVeiculo == null || eventoStagging.codigoCategoriaVeiculo == string.Empty)
            {
                //TODO: verificar se a categoria existe. Forçar para categoria desconhecida caso não exista.
                eventoStagging.avisosEventoStagging.Add(new Aviso
                {
                    campo = "CódigoCategoriaVeiculo",
                    descricao = "Código de categoria do veiculo '" + eventoStagging.codigoCategoriaVeiculo + "' inválido.",
                    tipologiaId = tipoAvisoGenerico,
                    eventoStagging = eventoStagging
                });
            }
            else
            {
                List<Categoria> queryCategoria = categoriasRepository.All.Where(c => c.codigoCategoriaVeiculo == eventoStagging.codigoCategoriaVeiculo).ToList();
                if (queryCategoria.Count == 0)
                {
                    //TODO: verificar se a categoria existe. Forçar para categoria desconhecida caso não exista.
                    eventoStagging.avisosEventoStagging.Add(new Aviso
                    {
                        campo = "CódigoCategoriaVeiculo",
                        descricao = "Código de categoria do veiculo '" + eventoStagging.codigoCategoriaVeiculo + "' inválido.",
                        tipologiaId = tipoAvisoGenerico,
                        eventoStagging = eventoStagging
                    });
                }
            }
            //tem concelho de circulação habitual válido - check
            if (eventoStagging.codigoConcelhoCirculacao == null || eventoStagging.codigoConcelhoCirculacao == string.Empty)
            {
                //TODO: verificar se o concelho existe. Forçar para concelho desconhecido caso não exista.
                eventoStagging.avisosEventoStagging.Add(new Aviso { campo = "ConcelhoCirculacao",
                    descricao = "Concelho de circulação '"+eventoStagging.codigoConcelhoCirculacao+"' inválido.",
                                                                    tipologiaId = tipoAvisoGenerico,
                                                                    eventoStagging = eventoStagging
                });
            }
            else
            {
                List<Concelho> queryCategoria = concelhosRepository.All.Where(c => c.codigoConcelho == eventoStagging.codigoConcelhoCirculacao).ToList();
                if (queryCategoria.Count == 0)
                {
                    //TODO: verificar se a categoria existe. Forçar para categoria desconhecida caso não exista.
                    eventoStagging.avisosEventoStagging.Add(new Aviso
                    {
                        campo = "ConcelhoCirculacao",
                        descricao = "Concelho de circulação '" + eventoStagging.codigoConcelhoCirculacao + "' inválido.",
                        tipologiaId = tipoAvisoGenerico,
                        eventoStagging = eventoStagging
                    });
                }
            }
            //tem nome de tomador de seguro - check
            if (eventoStagging.nomeTomadorSeguro == null || eventoStagging.nomeTomadorSeguro == string.Empty)
            {
                eventoStagging.avisosEventoStagging.Add(new Aviso { campo = "NomeTomador",
                    descricao = "Nome de tomador de seguro '"+eventoStagging.nomeTomadorSeguro+"' inválido.",
                                                                    tipologiaId = tipoAvisoGenerico,
                                                                    eventoStagging = eventoStagging
                });
            }
            //tem morada de tomador de seguro - check
            if (eventoStagging.moradaTomadorSeguro == null || eventoStagging.moradaTomadorSeguro == string.Empty)
            {
                eventoStagging.avisosEventoStagging.Add(new Aviso { campo = "MoradaTomador",
                    descricao = "Morada do tomador de seguro '"+eventoStagging.moradaTomadorSeguro+"' inválida.",
                                                                    tipologiaId = tipoAvisoGenerico,
                                                                    eventoStagging = eventoStagging
                });
            }
            //TODO: talvez remover visto não estar na lista de validações
            //tem codigo postal válido do tomador de seguro
            if (eventoStagging.codigoPostalTomador == null || eventoStagging.codigoPostalTomador == string.Empty)
            {
                //TODO: verificar se o código postal existe.
                eventoStagging.avisosEventoStagging.Add(new Aviso { campo = "CodigoPostalTomador",
                    descricao = "Código postal do tomador de seguro '" + eventoStagging.codigoPostalTomador + "' inválido.",
                                                                    tipologiaId = tipoAvisoGenerico,
                                                                    eventoStagging = eventoStagging
                });
            }
            //Tem NIF válido do tomador do seguro - check
            if (eventoStagging.nifTomadorSeguro == null || eventoStagging.nifTomadorSeguro == string.Empty || !ValidacaoEventos.NifValido(eventoStagging.nifTomadorSeguro))
            {
                eventoStagging.avisosEventoStagging.Add(new Aviso { campo = "NifTomador",
                    descricao = "Nif do tomador de seguro '"+eventoStagging.nifTomadorSeguro+"' inválido.",
                                                                    tipologiaId = tipoAvisoGenerico,
                                                                    eventoStagging = eventoStagging
                });
            }
            //Tem número de identificação do tomador de seguro - check
            if (eventoStagging.nrIdentificacaoTomadorSeguro == null || eventoStagging.nrIdentificacaoTomadorSeguro == string.Empty)
            {
                eventoStagging.avisosEventoStagging.Add(new Aviso { campo = "NumeroIdentificacaoTomador",
                    descricao = "Número de identificação do tomador de seguro '"+eventoStagging.nrIdentificacaoTomadorSeguro+"' inválido.",
                                                                    tipologiaId = tipoAvisoGenerico,
                                                                    eventoStagging = eventoStagging
                });
            }

            if (eventoStagging.errosEventoStagging.Count > 0)
            {
                return false;
            }

            return true;
        }
        
        public static bool NifValido(string nif)
        {
            if (!Regex.IsMatch(nif, @"\d{9}")) //tem de ter 9 digitos
            {
                return false;
            }

            char firstChar = nif[0]; //O primeiro digito tem de ser um de: 1,2,5,6,8,9
            if (firstChar.Equals('1')
                || firstChar.Equals('2')
                || firstChar.Equals('5')
                || firstChar.Equals('6')
                || firstChar.Equals('8')
                || firstChar.Equals('9'))
            {
                int checkDigit = (Convert.ToInt32(firstChar.ToString()) * 9);
                for (int i = 2; i <= 8; ++i)
                {
                    checkDigit += Convert.ToInt32(nif[i - 1].ToString()) * (10 - i);
                }
                // a soma 9*<1 Digito> + 8*<2 Digito> + 7*<3 Digito> + 6*<4 Digito> + 5*<5 Digito> + 4*<6 Digito> + 3*<7 Digito> + 2*<8 Digito> + 1*<9 Digito>
                // tem de ser multipla de 11
                checkDigit = 11 - (checkDigit % 11);
                if (checkDigit >= 10)
                {
                    checkDigit = 0;
                }
                // em certos casos, se o último número for 0, este representa o valor de 10 de forma a validar o checksum
                if (checkDigit.ToString() == nif[8].ToString())
                {
                    return true;
                }
            }

            return false;
        }

        public static bool validarNegocio(EventoStagging eventoStagging, Mutex mutex = null)
        {
            ApoliceRepository apolicesRepository = new ApoliceRepository();
            ValorSistemaRepository valoresSistemaRepository = new ValorSistemaRepository();

            List<ValorSistema> tiposErro = valoresSistemaRepository.GetPorTipologia("TIPO_ERRO", mutex);
            List<ValorSistema> tiposAviso = valoresSistemaRepository.GetPorTipologia("TIPO_AVISO", mutex);

            int tipoErroGenerico = tiposErro.Where(e => e.valor == "GENERICO").Single().valorSistemaId;
            int tipoAvisoGenerico = tiposAviso.Where(e => e.valor == "GENERICO").Single().valorSistemaId;

            List<Apolice> listaApolices;

            DateTime dataInicio;
            DateTime dataFim;

            TimeSpan horaInicio;
            TimeSpan horaFim;

            DateTime.TryParseExact(eventoStagging.dataInicioCobertura, "yyyyMMdd", new CultureInfo("pt-PT"), DateTimeStyles.None, out dataInicio);
            DateTime.TryParseExact(eventoStagging.dataFimCobertura, "yyyyMMdd", new CultureInfo("pt-PT"), DateTimeStyles.None, out dataFim);
            TimeSpan.TryParseExact(eventoStagging.horaInicioCobertura, "hhmmss", new CultureInfo("pt-PT"), TimeSpanStyles.None, out horaInicio);
            TimeSpan.TryParseExact(eventoStagging.horaFimCobertura, "hhmmss", new CultureInfo("pt-PT"), TimeSpanStyles.None, out horaFim);

            dataInicio = dataInicio.Add(horaInicio);
            dataFim = dataFim.Add(horaFim);

            listaApolices = apolicesRepository.All.Include("veiculo").Where(a => a.dataInicio == dataInicio &&
                                 a.entidadeId == eventoStagging.entidadeId && a.veiculo.numeroMatricula == eventoStagging.matricula).ToList();

            //ERROS
            //Criação para uma chave de registo já existente. - check
            if (listaApolices != null && listaApolices.Count > 0 && eventoStagging.codigoOperacao == "C")
            {
                eventoStagging.errosEventoStagging.Add(new ErroEventoStagging
                {
                    campo = "Chave",
                    descricao = "O registo de cobertura já existe. Não é possível efectuar nova criação.", 
                    tipologiaId = tipoErroGenerico , eventoStagging = eventoStagging });
            }

            //Data Fim (data + hora) menor ou igual que data de início. - check
            if(dataFim <= dataInicio){
                eventoStagging.errosEventoStagging.Add(new ErroEventoStagging
                {
                    campo = "DataFim",
                    descricao = "A data/hora de fim da cobertura tem de ser posterior à data/hora de início de cobertura.",
                    tipologiaId = tipoErroGenerico,
                    eventoStagging = eventoStagging
                });
            }

            //Datas de início inferiores a 01-01-1900. - check
            if (dataInicio < new DateTime(1900, 01, 01))
            {
                eventoStagging.errosEventoStagging.Add(new ErroEventoStagging
                {
                    campo = "DataInício",
                    descricao = "A data de início da cobertura tem de ser posterior à data de 01-01-1900.",
                    tipologiaId = tipoErroGenerico,
                    eventoStagging = eventoStagging
                });
            }

            //Modificação e Anulação para uma chave de registo não existente - check
            if ((eventoStagging.codigoOperacao == "A" || eventoStagging.codigoOperacao == "M")
                && listaApolices.Count == 0)
            {
                eventoStagging.errosEventoStagging.Add(new ErroEventoStagging
                {
                    campo = "Chave",
                    descricao = "O registo de cobertura não existe. Não é possível efectuar modificações ou anulações.",
                    tipologiaId = tipoErroGenerico,
                    eventoStagging = eventoStagging
                });
            }

            //Modificações com alteração de data/hora Fim. - check
            if (listaApolices.Count > 0)
            {
                if (eventoStagging.codigoOperacao == "M" &&
                    dataFim != listaApolices.First().dataFim)
                {
                    eventoStagging.errosEventoStagging.Add(new ErroEventoStagging
                    {
                        campo = "DataFim",
                        descricao = "Operações de modificação não podem alterar a data/hora de fim de cobertura.",
                        tipologiaId = tipoErroGenerico,
                        eventoStagging = eventoStagging
                    });
                }
            }

            //Anulação para uma data posterior à data fim planeada. - check
            if (listaApolices.Count > 0)
            {
                if (eventoStagging.codigoOperacao == "A" &&
                    dataFim > listaApolices.First().dataFimPlaneada)
                {
                    eventoStagging.errosEventoStagging.Add(new ErroEventoStagging
                    {
                        campo = "DataFim",
                        descricao = "Anulação não pode alterar a data/hora de fim de cobertura para data posterior à planeada na criação.",
                        tipologiaId = tipoErroGenerico,
                        eventoStagging = eventoStagging
                    });
                }
            }


            //AVISOS
            //Caso a matricula já se encontre associada a outra seguradora que se intercepte o periodo a reportar. - check
            //registos que originem mais que um seguro válido (inter ou intra seguradoras)
            if(eventoStagging.codigoOperacao == "C"){
                listaApolices = apolicesRepository.All.Include("veiculo").Where(a => a.veiculo.numeroMatricula == eventoStagging.matricula).ToList();
                List<Apolice> apolicesCruzadas = new List<Apolice>();

                foreach (Apolice ap in listaApolices)
                {
                    if ((dataInicio >= ap.dataInicio && dataInicio <= ap.dataFim) ||
                        (dataFim >= ap.dataInicio && dataFim <= ap.dataFim) ||
                        (dataInicio <= ap.dataInicio && dataFim >= ap.dataFim))
                    {
                        if (!(dataInicio == ap.dataInicio && eventoStagging.entidadeId == ap.entidadeId && eventoStagging.matricula == ap.veiculo.numeroMatricula))
                        {
                            apolicesCruzadas.Add(ap);
                        }
                    }
                }

                if (apolicesCruzadas.Count > 0)
                {
                    eventoStagging.avisosEventoStagging.Add(new Aviso
                    {
                        campo = "Matricula",
                        descricao = "O periodo de cobertura apresenta registos cruzados.",
                        tipologiaId = tipoAvisoGenerico,
                        eventoStagging = eventoStagging
                    });
                }
            }

            if (eventoStagging.errosEventoStagging.Count > 0)
            {
                return false;
            }

            return true;
        }

        public static bool validarEvento(EventoStagging eventoStagging, Mutex mutex = null)
        {
            if (!ValidacaoEventos.validarCampos(eventoStagging, mutex))
            {
                return false;
            }

            if (!ValidacaoEventos.validarNegocio(eventoStagging, mutex))
            {
                return false;
            }

            //throw new ErroEventoStaggingException();
            //TODO
            return true;
        }



    }
}
