namespace ISP.GestaoMatriculas.Repositories.Migrations
{
    using ISP.GestaoMatriculas.Model;
    using ISP.GestaoMatriculas.Model.Indicadores;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using WebMatrix.WebData;

    internal sealed class Configuration : DbMigrationsConfiguration<ISP.GestaoMatriculas.Model.DomainModels>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ISP.GestaoMatriculas.Model.DomainModels context)
        {
            var roles = SeedRoles(context);
            var categorias = SeedCategorias(context);
            var concelhos = SeedConcelhos(context);
            var entidades = SeedEntidades(context, roles);
            SeedUsers(context, entidades);
            SeedNotificacoes(context, entidades);
            SeedIndicadores(context, entidades);
            SeedApolices(context, categorias, concelhos, entidades);
        }

        private void SeedApolices(DomainModels context, IEnumerable<Categoria> categorias, IEnumerable<Concelho> concelhos, IEnumerable<Entidade> entidades)
        {
            
            Entidade ent = entidades.Single(e => e.Nome == "Acoreana");
            Categoria cat = categorias.Single(c => c.nome == "Categoria 1");
            var lst = new[]{
                new Apolice
                {
                    NumeroApolice = "SEG1/AP1",
                    DataInicio = new DateTime(2013, 11, 11),
                    DataFim = new DateTime(2014, 11, 11),
                    DataFimPlaneada = new DateTime(2014, 11, 11),
                    DataReporte = new DateTime(2013, 11, 11),
                    Veiculo = new Veiculo { numeroMatricula = "11-11-AA", anoConstrucao = "2013", marcaVeiculo = "OPEL", modeloVeiculo = "CORSA", categoria = cat },
                    Tomador = new Pessoa { nome = "Joao Mota", numeroIdentificacao = "123456789", nif = "987654321" },
                    Concelho = concelhos.Single(c => c.Nome == "Lisboa"),
                    Entidade = ent,
                    EventoHistorico = new EventoHistorico { codigoOperacao = "Criacao", seguradoraId = "11111", idOcorrencia = "11" },
                },
                new Apolice
                {
                    NumeroApolice = "SEG1/AP2",
                    DataInicio = new DateTime(2013, 11, 11),
                    DataFim = new DateTime(2014, 11, 11),
                    DataFimPlaneada = new DateTime(2014, 11, 11),
                    DataReporte = new DateTime(2013, 11, 11),
                    Veiculo = new Veiculo { numeroMatricula = "22-22-BB", anoConstrucao = "2013", marcaVeiculo = "OPEL", modeloVeiculo = "SAFIRA", categoria = categorias.Single(c => c.nome == "Categoria 2") },
                    Tomador = new Pessoa { nome = "Pedro Crespo", numeroIdentificacao = "98989898998", nif = "4554545454" },
                    Concelho = concelhos.Single(c => c.Nome == "Loures"),
                    Entidade = ent,
                    EventoHistorico = new EventoHistorico { codigoOperacao = "Criacao", seguradoraId = "11111", idOcorrencia = "11" }
                },
                new Apolice
                {
                    NumeroApolice = "SEG2/AP1",
                    DataInicio = new DateTime(2013, 11, 11),
                    DataFim = new DateTime(2014, 11, 11),
                    DataFimPlaneada = new DateTime(2014, 11, 11),
                    DataReporte = new DateTime(2013, 11, 11),
                    Veiculo = new Veiculo { numeroMatricula = "11-11-AA", anoConstrucao = "2013", marcaVeiculo = "OPEL", modeloVeiculo = "CORSA", categoria = categorias.Single(c => c.nome == "Categoria 1") },
                    Tomador = new Pessoa { nome = "Joao Mota", numeroIdentificacao = "123456789", nif = "987654321" },
                    Concelho = concelhos.Single(c => c.Nome == "Odivelas"),
                    Entidade = ent,
                    EventoHistorico = new EventoHistorico { codigoOperacao = "Criacao", seguradoraId = "11111", idOcorrencia = "11" }
                }


            };
            context.Apolices.AddOrUpdate(p => p.NumeroApolice, lst);
        }

        private void SeedIndicadores(DomainModels context, IEnumerable<Entidade> entidades)
        {

            // ONDE É QUE OS INDICADORES SÃO GUARDADOS????

            //var lst = new List<Indicador>();
            //foreach (Entidade entidade in entidades)
            //{
            //    if (entidade.indicadores.Count == 0)
            //    {
            //        Indicador i = new NumRegistosInd { descricao = "Número de registos", entidade = entidade };
            //        entidade.indicadores.Add(i);
            //        i = new NumApolicesInd { descricao = "Número de apólices", entidade = entidade };
            //        entidade.indicadores.Add(i);
            //    }
            //}
            //context.I
        }

        private void SeedNotificacoes(DomainModels context, IEnumerable<Entidade> entidades)
        {

            //var lst = new List<Notificacao>();
            //foreach (Entidade e in entidades)
            //{

            //    lst.AddRange(new[]{
            //        new Notificacao { tipo = Notificacao.TipoNotificacao.ErroFicheiro, mensagem = "Notificacao para " + e.Nome + " " + Notificacao.TipoNotificacao.ErroFicheiro, dataCriacao = DateTime.Now, lida = false, entidadeId = e.Id },
            //        new Notificacao { tipo = Notificacao.TipoNotificacao.SucessoFicheiro, mensagem = "Notificacao para " + e.Nome + " " + Notificacao.TipoNotificacao.ErroFicheiro, dataCriacao = DateTime.Now, lida = false, entidadeId = e.Id },
            //        new Notificacao { tipo = Notificacao.TipoNotificacao.WarningFicheiro, mensagem = "Notificacao para " + e.Nome + " " + Notificacao.TipoNotificacao.ErroFicheiro, dataCriacao = DateTime.Now, lida = false, entidadeId = e.Id }
            //    });
            //}
            //context.Notificacoes.AddOrUpdate(p => p.mensagem, lst.ToArray());
        }

        private void SeedUsers(DomainModels context, IEnumerable<Entidade> entidades)
        {
            //if (!WebSecurity.Initialized)
            //{
            //    WebSecurity.InitializeDatabaseConnection("DomainModels", "UserProfile", "UserId", "UserName", autoCreateTables: true);
            //}
            //if (!WebMatrix.WebData.WebSecurity.UserExists("admin1"))
            //{
            //    string userName = WebMatrix.WebData.WebSecurity.CreateUserAndAccount("admin1", "administrador", new { email = "admin1@app.net", ativo = true, entidadeId = entidades.Single(e => e.Nome == "ISP").Id }, false);
            //    System.Web.Security.Roles.AddUserToRole("admin1", "Admin");
            //}
        }

        private IEnumerable<Entidade> SeedEntidades(DomainModels context, IEnumerable<Role> roles)
        {
            var lst = new[]{
                new Entidade { Nome = "ISP", scope = Entidade.ScopeLevel.Global, role = roles.Single(r => r.RoleName == "ISP"), ativo = true },
                new Entidade { Nome = "Acoreana", scope = Entidade.ScopeLevel.Local, role = roles.Single(r => r.RoleName == "Seguradora"), ativo = true },
                new Entidade { Nome = "Victoria", scope = Entidade.ScopeLevel.Local, role = roles.Single(r => r.RoleName == "Seguradora"), ativo = true }
            };

            foreach (var item in lst)
            {
                item.notificacoes = new[]{
                    new Notificacao { tipo = Notificacao.TipoNotificacao.ErroFicheiro, mensagem = "Notificacao para " + item.Nome + " " + Notificacao.TipoNotificacao.ErroFicheiro, dataCriacao = DateTime.Now, lida = false},
                    new Notificacao { tipo = Notificacao.TipoNotificacao.SucessoFicheiro, mensagem = "Notificacao para " + item.Nome + " " + Notificacao.TipoNotificacao.ErroFicheiro, dataCriacao = DateTime.Now, lida = false},
                    new Notificacao { tipo = Notificacao.TipoNotificacao.WarningFicheiro, mensagem = "Notificacao para " + item.Nome + " " + Notificacao.TipoNotificacao.ErroFicheiro, dataCriacao = DateTime.Now, lida = false}
                };
            }

            context.Entidades.AddOrUpdate(p => p.Nome, lst);
            return lst;
        }

        private IEnumerable<Role> SeedRoles(DomainModels context)
        {
            var lst = new[]{
                new Role { RoleName = "Admin" },
                new Role { RoleName = "ISP" },
                new Role { RoleName = "Seguradora" }
            };
            context.Roles.AddOrUpdate(p => p.RoleName, lst);
            return lst;
        }

        private IEnumerable<Concelho> SeedConcelhos(DomainModels context)
        {
            var lst = new[]{
                new Concelho { Nome = "Lisboa" },
                new Concelho{ Nome = "Odivelas" },
                new Concelho { Nome = "Loures" }
            };
            context.Concelhos.AddOrUpdate(p => p.Nome, lst);
            return lst;
        }

        private static IEnumerable<Categoria> SeedCategorias(ISP.GestaoMatriculas.Model.DomainModels context)
        {
            var lst = new[]{
                new Categoria { nome = "Categoria 1" },
                new Categoria { nome = "Categoria 2" },
                new Categoria { nome = "Categoria 3" },
                new Categoria { nome = "Categoria 4" }
            };
            context.Categorias.AddOrUpdate(p => p.nome, lst);
            return lst;
        }
    }
}
