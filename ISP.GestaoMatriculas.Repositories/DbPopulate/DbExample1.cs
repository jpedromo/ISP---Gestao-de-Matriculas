using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Security;
using WebMatrix.WebData;
using System.Data.Entity.Infrastructure;
using ISP.GestaoMatriculas.Model;
using System.Web.Helpers;
using System.Data.Entity.Validation;
using System.Diagnostics;

namespace ISP.GestaoMatriculas.Repositories.DbPopulate
{
    public class DbExample1
    {

        public static void PopulateDB(DomainModels db)
        {

                AdicionaRoles(db);
                AdicionaValoresSistema(db);

                List<Entidade> listaEntidades = AdicionaEntidades(db);

                List<Categoria> listaCategorias = AdicionaCategorias(db);
                List<Concelho> listaConcelhos = AdicionaConcelhos(db);

                //AdicionaApolicesEx1(db);
            
                //AddNotificacoes(db);
                db.SaveChanges();
            
                AddUsers(db);         
                db.SaveChanges();

        }

        public static List<ValorSistema> AdicionaValoresSistema(DomainModels db)
        {
            List<ValorSistema> listaValores = new List<ValorSistema>();

            listaValores.Add(new ValorSistema { tipologia = "TIPO_AVISO", valor = "GENERICO", descricao = "Genérico", descricaoLonga = "Tipo de Aviso Genérico", editavel = false });

            listaValores.Add(new ValorSistema { tipologia = "TIPO_ERRO", valor = "GENERICO", descricao = "Genérico", descricaoLonga = "Tipo de Erro Genérico", editavel = false });

            listaValores.Add(new ValorSistema { tipologia = "TIPO_SCOPE", valor = "GLOBAL", descricao = "Global", descricaoLonga = "Tipo de scope de consulta global", editavel = false });
            listaValores.Add(new ValorSistema { tipologia = "TIPO_SCOPE", valor = "LOCAL", descricao = "Local", descricaoLonga = "Tipo de scope de consulta local", editavel = false });

            listaValores.Add(new ValorSistema { tipologia = "ESTADO_EVENTO_STAGGING", valor = "PENDENTE", descricao = "Pendente", descricaoLonga = "Evento de stagging pendente de processamento.", editavel = false });
            listaValores.Add(new ValorSistema { tipologia = "ESTADO_EVENTO_STAGGING", valor = "EM_PROCESSAMENTO", descricao = "Em Processamento", descricaoLonga = "Evento de stagging em processamento.", editavel = false });
            listaValores.Add(new ValorSistema { tipologia = "ESTADO_EVENTO_STAGGING", valor = "PROCESSADO", descricao = "Processado", descricaoLonga = "Evento de stagging processado.", editavel = false });
            listaValores.Add(new ValorSistema { tipologia = "ESTADO_EVENTO_STAGGING", valor = "ERRO", descricao = "Erro", descricaoLonga = "Evento de stagging com erro.", editavel = false });

            listaValores.Add(new ValorSistema { tipologia = "OPERACAO_EVENTO", valor = "C", descricao = "Criação", descricaoLonga = "Operação de criação de novo período seguro.", editavel = false });
            listaValores.Add(new ValorSistema { tipologia = "OPERACAO_EVENTO", valor = "M", descricao = "Modificação", descricaoLonga = "Operação de modificação de período seguro.", editavel = false });
            listaValores.Add(new ValorSistema { tipologia = "OPERACAO_EVENTO", valor = "A", descricao = "Anulação", descricaoLonga = "Operação de anulação de período seguro.", editavel = false });

            listaValores.Add(new ValorSistema { tipologia = "ESTADO_FICHEIRO", valor = "SUBMETIDO", descricao = "Submetido", descricaoLonga = "Ficheiro submetido e sem ordem de processamento.", editavel = false });
            listaValores.Add(new ValorSistema { tipologia = "ESTADO_FICHEIRO", valor = "PENDENTE", descricao = "Pendente", descricaoLonga = "Ficheiro submetido e pendente de processamento.", editavel = false });
            listaValores.Add(new ValorSistema { tipologia = "ESTADO_FICHEIRO", valor = "EM_PROCESSAMENTO", descricao = "Em Processamento", descricaoLonga = "Ficheiro em processamento.", editavel = false });
            listaValores.Add(new ValorSistema { tipologia = "ESTADO_FICHEIRO", valor = "PROCESSADO", descricao = "Processado", descricaoLonga = "Ficheiro processado.", editavel = false });
            listaValores.Add(new ValorSistema { tipologia = "ESTADO_FICHEIRO", valor = "NOTIFICADO", descricao = "Notificado", descricaoLonga = "Ficheiro notificado.", editavel = false });
            listaValores.Add(new ValorSistema { tipologia = "ESTADO_FICHEIRO", valor = "CANCELADO", descricao = "Cancelado", descricaoLonga = "Ficheiro cancelado.", editavel = false });
            listaValores.Add(new ValorSistema { tipologia = "ESTADO_FICHEIRO", valor = "ERRO", descricao = "Erro", descricaoLonga = "Ficheiro com Erro.", editavel = false });

            listaValores.Add(new ValorSistema { tipologia = "TIPO_INDICADOR", valor = "NR_EVENTOS", descricao = "Nr de eventos", descricaoLonga = "Número eventos reportados.", editavel = false });
            listaValores.Add(new ValorSistema { tipologia = "TIPO_INDICADOR", valor = "NR_EVENTOS_PROCESSADOS", descricao = "Nr de eventos processados", descricaoLonga = "Número eventos reportados e processados.", editavel = false });
            listaValores.Add(new ValorSistema { tipologia = "TIPO_INDICADOR", valor = "NR_ERROS_EVENTOS", descricao = "Nr de eventos com erro", descricaoLonga = "Número de eventos reportados com erro.", editavel = false });
            listaValores.Add(new ValorSistema { tipologia = "TIPO_INDICADOR", valor = "NR_ERROS_EVENTOS_TIPO", descricao = "Nr de erros por tipologia", descricaoLonga = "Número de erros por tipologia.", editavel = false });
            listaValores.Add(new ValorSistema { tipologia = "TIPO_INDICADOR", valor = "NR_AVISOS_PERIODO_SEGURO", descricao = "Nr de períodos seguros com aviso", descricaoLonga = "Número de períodos seguros reportados com avisos.", editavel = false });
            listaValores.Add(new ValorSistema { tipologia = "TIPO_INDICADOR", valor = "NR_AVISOS_PERIODO_SEGURO_TIPO", descricao = "Nr de avisos por tipologia", descricaoLonga = "Número de avisos de períodos seguros por tipologia.", editavel = false });
            listaValores.Add(new ValorSistema { tipologia = "TIPO_INDICADOR", valor = "NR_EVENTOS_OPERACAO", descricao = "Nr de eventos por operação", descricaoLonga = "Número de eventos por tipologia de operação.", editavel = false });
            listaValores.Add(new ValorSistema { tipologia = "TIPO_INDICADOR", valor = "NR_OPERACOES_FORA_SLA", descricao = "Nr de operaçõs fora do SLA", descricaoLonga = "Número de eventos processados fora do SLA.", editavel = false });
            listaValores.Add(new ValorSistema { tipologia = "TIPO_INDICADOR", valor = "NR_OPERACOES_DENTRO_SLA", descricao = "Nr de operações dentro do SLA", descricaoLonga = "Número de eventos processados dentro do SLA.", editavel = false });
            listaValores.Add(new ValorSistema { tipologia = "TIPO_INDICADOR", valor = "TEMPO_MEDIO_DESVIOS_SLA", descricao = "Tempo médio de desvio do SLA", descricaoLonga = "Tempo médio de desvio de processamento fora do SLA.", editavel = false });
            listaValores.Add(new ValorSistema { tipologia = "TIPO_INDICADOR", valor = "NR_ERROS_PENDENTES", descricao = "Nr de erros pendentes", descricaoLonga = "Número de eventos reportados com erro e pendentes de correção.", editavel = false });

            listaValores.Add(new ValorSistema { tipologia = "INDICADOR", valor = "TOTAIS_EVENTOS", descricao = "Nr de eventos reportados", descricaoLonga = "Número eventos reportados.", editavel = false });
            listaValores.Add(new ValorSistema { tipologia = "INDICADOR", valor = "ERROS_TIPOLOGIA", descricao = "Nr de erros com erro", descricaoLonga = "Número eventos reportados com erro.", editavel = false });
            listaValores.Add(new ValorSistema { tipologia = "INDICADOR", valor = "AVISOS_TIPOLOGIA", descricao = "Nr de eventos com aviso", descricaoLonga = "Número de eventos reportados com aviso.", editavel = false });
            listaValores.Add(new ValorSistema { tipologia = "INDICADOR", valor = "OPERACOES_TIPOLOGIA", descricao = "Nr de eventos reportados por operação", descricaoLonga = "Nr de eventos reportados por operação.", editavel = false });
            listaValores.Add(new ValorSistema { tipologia = "INDICADOR", valor = "SLA", descricao = "Indicador de SLA", descricaoLonga = "Indicadores de SLA's.", editavel = false });

            listaValores.Add(new ValorSistema { tipologia = "TIPO_NOTIFICACAO", valor = "NOTIFICACAO_ISP", descricao = "Notificação ISP", descricaoLonga = "Notificação criada pelo ISP.", editavel = false });
            listaValores.Add(new ValorSistema { tipologia = "TIPO_NOTIFICACAO", valor = "NOTIFICACAO_PUBLICA", descricao = "Notificação enviada para o ISP", descricaoLonga = "Notificação enviada para o ISP via site público.", editavel = false });
            listaValores.Add(new ValorSistema { tipologia = "TIPO_NOTIFICACAO", valor = "SUCESSO_RECECAO_FICHEIRO", descricao = "Notificação de sucesso na receção de ficheiro", descricaoLonga = "Notificação enviada às entidades seguradoras para reporte do sucesso na receção de um ficheiro.", editavel = false });
            listaValores.Add(new ValorSistema { tipologia = "TIPO_NOTIFICACAO", valor = "INSUCESSO_RECECAO_FICHEIRO", descricao = "Notificação de insucesso na receção de ficheiro", descricaoLonga = "Notificação enviada às entidades seguradoras para reporte do insucesso na receção de um ficheiro.", editavel = false });
            listaValores.Add(new ValorSistema { tipologia = "TIPO_NOTIFICACAO", valor = "SUCESSO_PROCESSAMENTO_FICHEIRO", descricao = "Notificação de sucesso no processamento de ficheiro", descricaoLonga = "Notificação enviada às entidades seguradoras para reporte do sucesso no processamento de um ficheiro.", editavel = false });
            listaValores.Add(new ValorSistema { tipologia = "TIPO_NOTIFICACAO", valor = "ERRO_PROCESSAMENTO_FICHEIRO", descricao = "Notificação de erro no processamento de ficheiro", descricaoLonga = "Notificação enviada às entidades seguradoras para reporte de erro no processamento de um ficheiro.", editavel = false });
            listaValores.Add(new ValorSistema { tipologia = "TIPO_NOTIFICACAO", valor = "AVISO_PROCESSAMENTO_FICHEIRO", descricao = "Notificação de aviso no processamento de ficheiro", descricaoLonga = "Notificação enviada às entidades seguradoras para reporte de aviso no processamento de um ficheiro.", editavel = false });


            listaValores.Add(new ValorSistema { tipologia = "PARAM_PASTA_FICHEIROS", valor = "C:\\UploadFolder", descricao = "Pasta de upload de ficheiros", descricaoLonga = "Pasta onde são guardados os ficheiros de upload.", editavel = true });
            listaValores.Add(new ValorSistema { tipologia = "PARAM_PASTA_FICHEIROS_ISENTOS", valor = "C:\\UploadIsentosFolder", descricao = "Pasta de upload de ficheiros isentos", descricaoLonga = "Pasta onde são guardados os ficheiros de upload de matrículas isentas.", editavel = true });
            listaValores.Add(new ValorSistema { tipologia = "PARAM_HORA_LIMITE_SLA", valor = "16", descricao = "Notificação de aviso no processamento de ficheiro", descricaoLonga = "Notificação enviada às entidades seguradoras para reporte de aviso no processamento de um ficheiro.", editavel = true });
            listaValores.Add(new ValorSistema { tipologia = "PARAM_HORA_EXTENSAO_SLA", valor = "24", descricao = "Notificação de aviso no processamento de ficheiro", descricaoLonga = "Notificação enviada às entidades seguradoras para reporte de aviso no processamento de um ficheiro.", editavel = true });
            listaValores.Add(new ValorSistema { tipologia = "SMTP_SERVER", valor = "email.rede.isp.pt", descricao = "SMTP para envio de emails", descricaoLonga = "Servidor SMTP para envio de emails.", editavel = true });
            listaValores.Add(new ValorSistema { tipologia = "SMTP_PASSWORD", valor = "", descricao = "User SMTP para envio de emails", descricaoLonga = "Password do utilizador do servidor SMTP para envio de emails.", editavel = true });
            listaValores.Add(new ValorSistema { tipologia = "SMTP_USER", valor = "", descricao = "User SMTP para envio de emails", descricaoLonga = "Utilizador do servidor SMTP para envio de emails.", editavel = true });
            listaValores.Add(new ValorSistema { tipologia = "SMTP_PORT", valor = "25", descricao = "Port de SMTP para envio de emails", descricaoLonga = "Port do servidor SMTP para envio de emails.", editavel = true });

            if (db.ValoresSistema.ToList().Count == 0)
            {
                foreach (ValorSistema e in listaValores)
                {
                    db.ValoresSistema.Add(e);
                }
            }

            db.SaveChanges();

            return listaValores;
        }

        public static void AdicionaApolicesEx1(DomainModels db)
        {
            List<ValorSistema> operacoesEvento = db.ValoresSistema.Where(v => v.tipologia == "OPERACAO_EVENTO").ToList();
            List<ValorSistema> estadosEvento = db.ValoresSistema.Where(v => v.tipologia == "ESTADO_EVENTO_STAGGING").ToList();

            if (db.Apolices.ToList().Count == 0)
            {
                List<Apolice> listaApolices = new List<Apolice>();
                List<ApoliceHistorico> listaApolicesHist = new List<ApoliceHistorico>();

                Entidade ent = db.Entidades.Single(e => e.nome == "Acoreana");
                Apolice ap = new Apolice
                {
                    numeroApolice = "SEG1/AP1",
                    dataInicio = new DateTime(2013, 11, 11),
                    dataFim = new DateTime(2014, 11, 11),
                    dataFimPlaneada = new DateTime(2014, 11, 11),
                    dataReporte = new DateTime(2013, 11, 11),
                    veiculo = new Veiculo {  numeroMatricula = "11-11-AA", anoConstrucao = "2013", marcaVeiculo = "OPEL", modeloVeiculo = "CORSA", categoria = db.Categorias.Single(c => c.nome == "Categoria 1") },
                    tomador = new Pessoa { nome = "Joao Mota", numeroIdentificacao = "123456789", nif = "987654321" },
                    concelho = db.Concelhos.Single(c => c.nomeConcelho == "Lisboa"),
                    entidade = ent,
                    eventoHistorico = new EventoHistorico { entidadeId = ent.entidadeId, codigoOperacaoId = operacoesEvento.Where(o=> o.valor == "C").Single().valorSistemaId, idOcorrencia = "11", dataReporte = new DateTime(2013, 11, 11) },
                    dataRegisto = new DateTime(2013, 11, 11)
                };
                ent.apolices.Add(ap);
                listaApolices.Add(ap);

                ap = new Apolice
                {
                    numeroApolice = "SEG1/AP2",
                    dataInicio = new DateTime(2013, 11, 11),
                    dataFim = new DateTime(2014, 11, 11),
                    dataFimPlaneada = new DateTime(2014, 11, 11),
                    dataReporte = new DateTime(2013, 11, 11),
                    veiculo = new Veiculo { numeroMatricula = "22-22-BB", anoConstrucao = "2013", marcaVeiculo = "OPEL", modeloVeiculo = "SAFIRA", categoria = db.Categorias.Single(c => c.nome == "Categoria 2") },
                    tomador = new Pessoa { nome = "Pedro Crespo", numeroIdentificacao = "98989898998", nif = "4554545454" },
                    concelho = db.Concelhos.Single(c => c.nomeConcelho == "Loures"),
                    entidade = ent,
                    eventoHistorico = new EventoHistorico { entidadeId = ent.entidadeId, codigoOperacaoId = operacoesEvento.Where(o => o.valor == "C").Single().valorSistemaId, idOcorrencia = "11", dataReporte = new DateTime(2013, 11, 11) },
                    dataRegisto = new DateTime(2013, 11, 11)
                };
                ent.apolices.Add(ap);
                listaApolices.Add(ap);

                ent = db.Entidades.Single(e => e.nome == "Victoria");
                ap = new Apolice
                {
                    numeroApolice = "SEG2/AP1",
                    dataInicio = new DateTime(2013, 11, 11),
                    dataFim = new DateTime(2014, 11, 11),
                    dataFimPlaneada = new DateTime(2014, 11, 11),
                    dataReporte = new DateTime(2013, 11, 11),
                    veiculo = new Veiculo { numeroMatricula = "11-11-AA", anoConstrucao = "2013", marcaVeiculo = "OPEL", modeloVeiculo = "CORSA", categoria = db.Categorias.Single(c => c.nome == "Categoria 1") },
                    tomador = new Pessoa { nome = "Joao Mota", numeroIdentificacao = "123456789", nif = "987654321" },
                    concelho = db.Concelhos.Single(c => c.nomeConcelho == "Odivelas"),
                    entidade = ent,
                    eventoHistorico = new EventoHistorico { entidadeId = ent.entidadeId, codigoOperacaoId = operacoesEvento.Where(o => o.valor == "C").Single().valorSistemaId, idOcorrencia = "11", dataReporte = new DateTime(2013, 11, 11) },
                    dataRegisto = new DateTime(2013, 11, 11)
                };
                ent.apolices.Add(ap);
                listaApolices.Add(ap);


                foreach (Apolice a in listaApolices)
                {
                    if (db.Apolices.ToList().Count == 0 || db.Apolices.Single(apol => apol.numeroApolice == a.numeroApolice) == null)
                        db.Apolices.Add(a);
                }


                ApoliceHistorico apHist = new ApoliceHistorico
                {
                    numeroApolice = "SEG1/AP1",
                    dataInicio = new DateTime(2013, 11, 11),
                    dataFim = new DateTime(2014, 11, 11),
                    dataFimPlaneada = new DateTime(2014, 11, 11),
                    dataReporte = new DateTime(2013, 11, 11),
                    veiculo = new Veiculo { numeroMatricula = "11-11-AA", anoConstrucao = "2013", marcaVeiculo = "OPEL", modeloVeiculo = "CORSA", categoria = db.Categorias.Single(c => c.nome == "Categoria 1") },
                    tomador = new Pessoa { nome = "Joao Mota", numeroIdentificacao = "123456789", nif = "987654321" },
                    concelho = db.Concelhos.Single(c => c.nomeConcelho == "Lisboa"),
                    entidade = ent,
                    eventoHistorico = new EventoHistorico { entidadeId = ent.entidadeId, codigoOperacaoId = operacoesEvento.Where(o => o.valor == "C").Single().valorSistemaId, idOcorrencia = "11", dataReporte = new DateTime(2013, 11, 11) },
                    dataArquivo = new DateTime(2013, 11, 11)
                };
                ent.apolicesHistorico.Add(apHist);
                listaApolicesHist.Add(apHist);

                foreach (ApoliceHistorico a in listaApolicesHist)
                {
                    if (db.ApolicesHistorico.ToList().Count == 0 || db.ApolicesHistorico.Single(apol => apol.numeroApolice == a.numeroApolice) == null)
                        db.ApolicesHistorico.Add(a);
                }


                EventoStagging stagging = new EventoStagging
                {
                    codigoOperacao = "C",
                    estadoEventoId = estadosEvento.Where(o=> o.valor == "ERRO").Single().valorSistemaId,
                    nrApolice = "SEG1/AP1",
                    dataInicioCobertura = "12-12-2013",
                    horaInicioCobertura = "08:00",
                    dataFimCobertura = "13-12-2013",
                    horaFimCobertura = "08:00",
                    matricula = "11-11-AA",
                    nomeTomadorSeguro = "Joao Mota",
                    entidade = ent,
                    dataReporte = new DateTime(2013, 12,12),
                    dataUltimaAlteracaoErro = new DateTime(2013, 12, 12),
                    dataRegisto = new DateTime(2013, 12, 12)
                };
                db.EventosStagging.Add(stagging);



                db.SaveChanges();

            }
        }

        public static List<Categoria> AdicionaCategorias(DomainModels db)
        {
            List<Categoria> listaCategorias = new List<Categoria>();
            if (db.Categorias.ToList().Count == 0)
            {

                listaCategorias.Add(new Categoria { nome = "Categoria 1" , codigoCategoriaVeiculo = "1"});
                listaCategorias.Add(new Categoria { nome = "Categoria 2" , codigoCategoriaVeiculo = "2"});
                listaCategorias.Add(new Categoria { nome = "Categoria 3" , codigoCategoriaVeiculo = "3"});
                listaCategorias.Add(new Categoria { nome = "Categoria 4" , codigoCategoriaVeiculo = "4"});
                listaCategorias.Add(new Categoria { nome = "Desconhecida", codigoCategoriaVeiculo = "99" });

                foreach (Categoria c in listaCategorias)
                {
                    if (db.Categorias.ToList().Count == 0 || db.Categorias.Single(cat => cat.nome == c.nome) == null)
                        db.Categorias.Add(c);
                }

                db.SaveChanges();
            }

            return listaCategorias;
        }

        public static List<Concelho> AdicionaConcelhos(DomainModels db)
        {
            List<Concelho> listaConcelhos = new List<Concelho>();

            listaConcelhos.Add(new Concelho { nomeConcelho = "Lisboa" , codigoConcelho = "1"});
            listaConcelhos.Add(new Concelho { nomeConcelho = "Odivelas" , codigoConcelho = "2"});
            listaConcelhos.Add(new Concelho { nomeConcelho = "Loures" , codigoConcelho = "3"});
            listaConcelhos.Add(new Concelho { nomeConcelho = "Desconhecido", codigoConcelho = "99" });

            foreach (Concelho c in listaConcelhos)
            {
                if (db.Concelhos.ToList().Count == 0 || db.Concelhos.Single(conc => conc.nomeConcelho == c.nomeConcelho) == null)
                    db.Concelhos.Add(c);
            }

            db.SaveChanges();

            return listaConcelhos;
        }

        public static void AdicionaRoles(DomainModels db)
        {

            List<Role> listaRoles = new List<Role>();

            listaRoles.Add(new Role { RoleName = "Admin" });
            listaRoles.Add(new Role { RoleName = "ISP" });
            listaRoles.Add(new Role { RoleName = "ISP-Leitura" });
            listaRoles.Add(new Role { RoleName = "Seguradora" });

            foreach (Role r in listaRoles)
                
                try{
                    if (db.Roles.Single(role => role.RoleName == r.RoleName) == null)
                    {
                        db.Roles.Add(r);
                    }
                }
                catch(InvalidOperationException){
                    db.Roles.Add(r);
                }

            db.SaveChanges();
        }

        public static List<Entidade> AdicionaEntidades(DomainModels db)
        {
            ValorSistemaRepository valoresSistemaRepository = new ValorSistemaRepository();
            List<ValorSistema> scopes = valoresSistemaRepository.All.Where(v => v.tipologia == "TIPO_SCOPE").ToList();

            List<Entidade> listaEntidades = new List<Entidade>();
            
            listaEntidades.Add(new Entidade { nome = "ISP", scopeId = scopes.Where(s => s.valor == "GLOBAL").Single().valorSistemaId  , role = db.Roles.Single(r => r.RoleName == "ISP"), ativo = true });
            listaEntidades.Add(new Entidade { nome = "Acoreana", scopeId = scopes.Where(s => s.valor == "LOCAL").Single().valorSistemaId, role = db.Roles.Single(r => r.RoleName == "Seguradora"), ativo = true });
            listaEntidades.Add(new Entidade { nome = "Victoria", scopeId = scopes.Where(s => s.valor == "LOCAL").Single().valorSistemaId, role = db.Roles.Single(r => r.RoleName == "Seguradora"), ativo = true });

            foreach (Entidade e in listaEntidades)
            {
                if (db.Entidades.ToList().Count == 0 || db.Entidades.Single(ent => ent.nome == e.nome) == null)
                {
                    db.Entidades.Add(e);

                }
            }

            db.SaveChanges();

            return listaEntidades;
        }

        public static void AddUsers(DomainModels db)
        {
            
            if (!WebSecurity.Initialized)
            {
                WebSecurity.InitializeDatabaseConnection("DomainModels", "MAT_USER_PROFILE", "UserId_PK", "UserName", autoCreateTables: true);
            }
            
            /*
            if (db.UserProfiles.ToList().Count == 0 || db.UserProfiles.Single(u => u.nome == "admin1") == null)
            {
                UserProfile user = new UserProfile
                {
                    UserName = "admin1",
                    email = "admin@app.net",
                    entidadeId = db.Entidades.Single(e => e.nome == "ISP").entidadeId,
                    ativo = true
                };
                db.UserProfiles.Add(user);
                db.SaveChanges();

                Model.Membership membership = new Model.Membership
                {
                    UserId = (db.UserProfiles.Single(us => us.UserName == "admin1").UserId),
                    CreateDate = DateTime.Now,
                    Password = Crypto.HashPassword("administrador"),
                    PasswordSalt = String.Empty,
                    PasswordFailuresSinceLastSuccess = 0,
                    IsConfirmed = true,
                    PasswordChangedDate = DateTime.Now
                };
                db.Membership.Add(membership);

                    db.SaveChanges();
         
                Role role = db.Roles.Single(r => r.RoleName == "Admin");
                membership.Roles.Add(role);
                db.SaveChanges();
            }

                */
            if (!WebMatrix.WebData.WebSecurity.UserExists("admin1"))
            {
                
                string userName = WebMatrix.WebData.WebSecurity.CreateUserAndAccount("admin1", "administrador", new { ValEmail = "admin1@app.net", FlgAtivo = true, entidadeId_FK = db.Entidades.Single(e => e.nome == "ISP").entidadeId }, false);
                System.Web.Security.Roles.AddUserToRole("admin1", "Admin");
            }

            if (!WebMatrix.WebData.WebSecurity.UserExists("isp1"))
            {

                string userName = WebMatrix.WebData.WebSecurity.CreateUserAndAccount("isp1", "utilizador", new { ValEmail = "isp1@app.net", FlgAtivo = true, entidadeId_FK = db.Entidades.Single(e => e.nome == "ISP").entidadeId }, false);
                System.Web.Security.Roles.AddUserToRole("isp1", "ISP");
            }

            if (!WebMatrix.WebData.WebSecurity.UserExists("acoreana1"))
            {

                string userName = WebMatrix.WebData.WebSecurity.CreateUserAndAccount("acoreana1", "utilizador", new { ValEmail = "isp1@app.net", FlgAtivo = true, entidadeId_FK = db.Entidades.Single(e => e.nome == "Acoreana").entidadeId }, false);
                System.Web.Security.Roles.AddUserToRole("acoreana1", "Seguradora");
            }

            if (!WebMatrix.WebData.WebSecurity.UserExists("isp1leitura"))
            {
                string userName = WebMatrix.WebData.WebSecurity.CreateUserAndAccount("isp1leitura", "utilizador", new { ValEmail = "isp1leitura@app.net", FlgAtivo = true, entidadeId_FK = db.Entidades.Single(e => e.nome == "ISP").entidadeId }, false);
                System.Web.Security.Roles.AddUserToRole("isp1leitura", "ISP-Leitura");
            }



            if (!WebMatrix.WebData.WebSecurity.UserExists("pmtcrespoAD"))
            {
                string userName = WebMatrix.WebData.WebSecurity.CreateUserAndAccount("pmtcrespoAD", "utilizadorAD", new { ValEmail = "pmtcrespoAD@app.net", FlgAtivo = true, entidadeId_FK = db.Entidades.Single(e => e.nome == "ISP").entidadeId, ValUtilizadorAD = "REDEISP\\pmtcrespo" }, false);
                System.Web.Security.Roles.AddUserToRole("pmtcrespoAD", "Admin");
            }

            if (!WebMatrix.WebData.WebSecurity.UserExists("jpamotaAD"))
            {
                string userName = WebMatrix.WebData.WebSecurity.CreateUserAndAccount("jpamotaAD", "utilizadorAD", new { ValEmail = "jpamotaAD@app.net", FlgAtivo = true, entidadeId_FK = db.Entidades.Single(e => e.nome == "ISP").entidadeId, ValUtilizadorAD = "REDEISP\\jpamota" }, false);
                System.Web.Security.Roles.AddUserToRole("jpamotaAD", "Admin");
            }

            if (!WebMatrix.WebData.WebSecurity.UserExists("pmbladeiraAD"))
            {
                string userName = WebMatrix.WebData.WebSecurity.CreateUserAndAccount("pmbladeiraAD", "utilizadorAD", new { ValEmail = "pmbladeiraAD@app.net", FlgAtivo = true, entidadeId_FK = db.Entidades.Single(e => e.nome == "ISP").entidadeId, ValUtilizadorAD = "REDEISP\\pmbladeira" }, false);
                System.Web.Security.Roles.AddUserToRole("pmbladeiraAD", "Admin");
            }

            if (!WebMatrix.WebData.WebSecurity.UserExists("jcdmoreiraAD"))
            {
                string userName = WebMatrix.WebData.WebSecurity.CreateUserAndAccount("jcdmoreiraAD", "utilizadorAD", new { ValEmail = "jcdmoreira@isp.pt", FlgAtivo = true, entidadeId_FK = db.Entidades.Single(e => e.nome == "ISP").entidadeId, ValUtilizadorAD = "REDEISP\\jcdmoreira" }, false);
                System.Web.Security.Roles.AddUserToRole("jcdmoreiraAD", "Admin");
            }

            if (!WebMatrix.WebData.WebSecurity.UserExists("gmscostaAD"))
            {
                string userName = WebMatrix.WebData.WebSecurity.CreateUserAndAccount("gmscostaAD", "utilizadorAD", new { ValEmail = "gmscosta@isp.pt", FlgAtivo = true, entidadeId_FK = db.Entidades.Single(e => e.nome == "ISP").entidadeId, ValUtilizadorAD = "REDEISP\\gmscosta" }, false);
                System.Web.Security.Roles.AddUserToRole("gmscostaAD", "Admin");
            }

            if (!WebMatrix.WebData.WebSecurity.UserExists("fjcsilvaAD"))
            {
                string userName = WebMatrix.WebData.WebSecurity.CreateUserAndAccount("fjcsilvaAD", "utilizadorAD", new { ValEmail = "fjcsilva@isp.pt", FlgAtivo = true, entidadeId_FK = db.Entidades.Single(e => e.nome == "ISP").entidadeId, ValUtilizadorAD = "REDEISP\\fjcsilva" }, false);
                System.Web.Security.Roles.AddUserToRole("fjcsilvaAD", "Admin");
            }

            if (!WebMatrix.WebData.WebSecurity.UserExists("jacruzAD"))
            {
                string userName = WebMatrix.WebData.WebSecurity.CreateUserAndAccount("jacruzAD", "utilizadorAD", new { ValEmail = "jacruz@isp.pt", FlgAtivo = true, entidadeId_FK = db.Entidades.Single(e => e.nome == "ISP").entidadeId, ValUtilizadorAD = "REDEISP\\jacruz" }, false);
                System.Web.Security.Roles.AddUserToRole("jacruzAD", "Admin");
            }
        }

        public static void AddNotificacoes(DomainModels db)
        {

            //foreach (Entidade e in db.Entidades)
            //{
            //    for (int i = 0; i < 2; i++)
            //    {
            //        Notificacao n = new Notificacao { tipo = Notificacao.TipoNotificacao.ErroFicheiro, mensagem = "Notificacao para " + e.nome, dataCriacao = DateTime.Now, lida = false };
            //        e.notificacoes.Add(n);
            //        n = new Notificacao { tipo = Notificacao.TipoNotificacao.SucessoFicheiro, mensagem = "Notificacao para " + e.nome, dataCriacao = DateTime.Now, lida = false };
            //        e.notificacoes.Add(n);
            //        n = new Notificacao { tipo = Notificacao.TipoNotificacao.WarningFicheiro, mensagem = "Notificacao para " + e.nome, dataCriacao = DateTime.Now, lida = false };
            //        e.notificacoes.Add(n);
            //    }
            //    Notificacao n1 = new Notificacao { tipo = Notificacao.TipoNotificacao.ErroFicheiro, mensagem = "Notificacao para " + e.nome, dataCriacao = DateTime.Now, lida = true };
            //    e.notificacoes.Add(n1);
            //}
            //db.SaveChanges();
        }


        
    }
}
