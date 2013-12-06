using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Security;
using WebMatrix.WebData;
using System.Data.Entity.Infrastructure;
using ISP.GestaoMatriculas.Model.Indicadores;
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

            //    AdicionaRoles(db);
            //    List<Entidade> listaEntidades = AdicionaEntidades(db);

            //    //List<Categoria> listaCategorias = AdicionaCategorias(db);
            //    List<Concelho> listaConcelhos = AdicionaConcelhos(db);

            //    //AdicionaApolicesEx1(db, listaCategorias, listaConcelhos);

            //    AddIndicadores(db);
            
            //if(db.UserProfiles.ToList().Count == 0){
                
            //    AddNotificacoes(db);
            //    db.SaveChanges();
            
            //    AddUsers(db);
            //}
            
            //db.SaveChanges();

        }


        public static void AdicionaApolicesEx1(DomainModels db, List<Categoria> categorias, List<Concelho> listaConcelhos)
        {
            if (db.Apolices.ToList().Count == 0)
            {
                List<Apolice> listaApolices = new List<Apolice>();
                List<ApoliceHistorico> listaApolicesHist = new List<ApoliceHistorico>();

                Entidade ent = db.Entidades.Single(e => e.Nome == "Acoreana");
                Categoria cat = categorias.Single(c => c.nome == "Categoria 1");
                Apolice ap = new Apolice
                {
                    NumeroApolice = "SEG1/AP1",
                    DataInicio = new DateTime(2013, 11, 11),
                    DataFim = new DateTime(2014, 11, 11),
                    DataFimPlaneada = new DateTime(2014, 11, 11),
                    DataReporte = new DateTime(2013, 11, 11),
                    Veiculo = new Veiculo { numeroMatricula = "11-11-AA", anoConstrucao = "2013", marcaVeiculo = "OPEL", modeloVeiculo = "CORSA", categoria = cat },
                    Tomador = new Pessoa { nome = "Joao Mota", numeroIdentificacao = "123456789", nif = "987654321" },
                    Concelho = listaConcelhos.Single(c => c.Nome == "Lisboa"),
                    Entidade = ent,
                    EventoHistorico = new EventoHistorico { codigoOperacao = "Criacao", seguradoraId = "11111", idOcorrencia = "11" }
                };
                ent.apolices.Add(ap);
                listaApolices.Add(ap);

                ApoliceHistorico apHist = new ApoliceHistorico
                {
                    NumeroApolice = "SEG1/AP1",
                    DataInicio = new DateTime(2013, 11, 11),
                    DataFim = new DateTime(2014, 11, 11),
                    DataFimPlaneada = new DateTime(2014, 11, 11),
                    DataReporte = new DateTime(2013, 11, 11),
                    Veiculo = new Veiculo { numeroMatricula = "11-11-AA", anoConstrucao = "2013", marcaVeiculo = "OPEL", modeloVeiculo = "CORSA", categoria = categorias.Single(c => c.nome == "Categoria 1") },
                    Tomador = new Pessoa { nome = "Joao Mota", numeroIdentificacao = "123456789", nif = "987654321" },
                    Concelho = listaConcelhos.Single(c => c.Nome == "Lisboa"),
                    Entidade = ent,
                    EventoHistorico = new EventoHistorico { codigoOperacao = "Criacao", seguradoraId = "11111", idOcorrencia = "11" },
                    Apolice = ap
                };
                ent.apolicesHistorico.Add(apHist);
                listaApolicesHist.Add(apHist);

                foreach (ApoliceHistorico a in listaApolicesHist)
                {
                    if (db.ApolicesHistorico.ToList().Count == 0 || db.ApolicesHistorico.Single(apol => apol.NumeroApolice == a.NumeroApolice) == null)
                        db.ApolicesHistorico.Add(a);
                }

                ap = new Apolice
                {
                    NumeroApolice = "SEG1/AP2",
                    DataInicio = new DateTime(2013, 11, 11),
                    DataFim = new DateTime(2014, 11, 11),
                    DataFimPlaneada = new DateTime(2014, 11, 11),
                    DataReporte = new DateTime(2013, 11, 11),
                    Veiculo = new Veiculo { numeroMatricula = "22-22-BB", anoConstrucao = "2013", marcaVeiculo = "OPEL", modeloVeiculo = "SAFIRA", categoria = categorias.Single(c => c.nome == "Categoria 2") },
                    Tomador = new Pessoa { nome = "Pedro Crespo", numeroIdentificacao = "98989898998", nif = "4554545454" },
                    Concelho = listaConcelhos.Single(c => c.Nome == "Loures"),
                    Entidade = ent,
                    EventoHistorico = new EventoHistorico { codigoOperacao = "Criacao", seguradoraId = "11111", idOcorrencia = "11" }
                };
                ent.apolices.Add(ap);
                listaApolices.Add(ap);

                ent = db.Entidades.Single(e => e.Nome == "Victoria");
                ap = new Apolice
                {
                    NumeroApolice = "SEG2/AP1",
                    DataInicio = new DateTime(2013, 11, 11),
                    DataFim = new DateTime(2014, 11, 11),
                    DataFimPlaneada = new DateTime(2014, 11, 11),
                    DataReporte = new DateTime(2013, 11, 11),
                    Veiculo = new Veiculo { numeroMatricula = "11-11-AA", anoConstrucao = "2013", marcaVeiculo = "OPEL", modeloVeiculo = "CORSA", categoria = categorias.Single(c => c.nome == "Categoria 1") },
                    Tomador = new Pessoa { nome = "Joao Mota", numeroIdentificacao = "123456789", nif = "987654321" },
                    Concelho = listaConcelhos.Single(c => c.Nome == "Odivelas"),
                    Entidade = ent,
                    EventoHistorico = new EventoHistorico { codigoOperacao = "Criacao", seguradoraId = "11111", idOcorrencia = "11" }
                };
                ent.apolices.Add(ap);
                listaApolices.Add(ap);


                foreach (Apolice a in listaApolices)
                {
                    if (db.Apolices.ToList().Count == 0 || db.Apolices.Single(apol => apol.NumeroApolice == a.NumeroApolice) == null)
                        db.Apolices.Add(a);
                }


                

                db.SaveChanges();

            }
        }

        //public static List<Categoria> AdicionaCategorias(DomainModels db)
        //{
        //    List<Categoria> listaCategorias = new List<Categoria>();
        //    if (db.Categorias.ToList().Count == 0)
        //    {

        //        listaCategorias.Add(new Categoria { nome = "Categoria 1" });
        //        listaCategorias.Add(new Categoria { nome = "Categoria 2" });
        //        listaCategorias.Add(new Categoria { nome = "Categoria 3" });
        //        listaCategorias.Add(new Categoria { nome = "Categoria 4" });

        //        foreach (Categoria c in listaCategorias)
        //        {
        //            if (db.Categorias.ToList().Count == 0 || db.Categorias.Single(cat => cat.nome == c.nome) == null)
        //                db.Categorias.Add(c);
        //        }

        //        db.SaveChanges();
        //    }
        //    else
        //    {
        //        listaCategorias = db.Categorias.ToList();
        //    }

        //    return listaCategorias;
        //}

        //public static List<Concelho> AdicionaConcelhos(DomainModels db)
        //{
        //    List<Concelho> listaConcelhos = new List<Concelho>();

        //    listaConcelhos.Add(new Concelho { Nome = "Lisboa" });
        //    listaConcelhos.Add(new Concelho { Nome = "Odivelas" });
        //    listaConcelhos.Add(new Concelho { Nome = "Loures" });

        //    foreach (Concelho c in listaConcelhos)
        //    {
        //        if (db.Concelhos.ToList().Count == 0 || db.Concelhos.Single(conc => conc.Nome == c.Nome) == null)
        //            db.Concelhos.Add(c);
        //    }

        //    db.SaveChanges();

        //    return listaConcelhos;
        //}

        //public static void AdicionaRoles(DomainModels db)
        //{

        //    List<Role> listaRoles = new List<Role>();

        //    listaRoles.Add(new Role { RoleName = "Admin" });
        //    listaRoles.Add(new Role { RoleName = "ISP" });
        //    listaRoles.Add(new Role { RoleName = "Seguradora" });

        //    foreach (Role r in listaRoles)
        //        if (db.Roles.ToList().Count == 0 || db.Roles.Single(role => role.RoleName == r.RoleName) == null)
        //        {
        //            db.Roles.Add(r);
        //        }

        //    db.SaveChanges();
        //}

        //public static List<Entidade> AdicionaEntidades(DomainModels db)
        //{
        //    List<Entidade> listaEntidades = new List<Entidade>();

        //    listaEntidades.Add(new Entidade { Nome = "ISP", scope = Entidade.ScopeLevel.Global, role = db.Roles.Single(r => r.RoleName == "ISP"), ativo = true });
        //    listaEntidades.Add(new Entidade { Nome = "Acoreana", scope = Entidade.ScopeLevel.Local, role = db.Roles.Single(r => r.RoleName == "Seguradora"), ativo = true});
        //    listaEntidades.Add(new Entidade { Nome = "Victoria", scope = Entidade.ScopeLevel.Local, role = db.Roles.Single(r => r.RoleName == "Seguradora"), ativo = true});

        //    foreach (Entidade e in listaEntidades)
        //    {
        //        if (db.Entidades.ToList().Count == 0 || db.Entidades.Single(ent => ent.Nome == e.Nome) == null)
        //        {
        //            db.Entidades.Add(e);

        //        }
        //    }

        //    db.SaveChanges();

        //    return listaEntidades;
        //}

        //public static void AddUsers(DomainModels db)
        //{
            
        //    if (!WebSecurity.Initialized)
        //    {
        //        WebSecurity.InitializeDatabaseConnection("DomainModels", "UserProfile", "UserId", "UserName", autoCreateTables: true);
        //    }
            
        //    /*
        //    if (db.UserProfiles.ToList().Count == 0 || db.UserProfiles.Single(u => u.nome == "admin1") == null)
        //    {
        //        UserProfile user = new UserProfile
        //        {
        //            UserName = "admin1",
        //            email = "admin@app.net",
        //            entidadeId = db.Entidades.Single(e => e.nome == "ISP").entidadeId,
        //            ativo = true
        //        };
        //        db.UserProfiles.Add(user);
        //        db.SaveChanges();

        //        Model.Membership membership = new Model.Membership
        //        {
        //            UserId = (db.UserProfiles.Single(us => us.UserName == "admin1").UserId),
        //            CreateDate = DateTime.Now,
        //            Password = Crypto.HashPassword("administrador"),
        //            PasswordSalt = String.Empty,
        //            PasswordFailuresSinceLastSuccess = 0,
        //            IsConfirmed = true,
        //            PasswordChangedDate = DateTime.Now
        //        };
        //        db.Membership.Add(membership);

        //            db.SaveChanges();
         
        //        Role role = db.Roles.Single(r => r.RoleName == "Admin");
        //        membership.Roles.Add(role);
        //        db.SaveChanges();
        //    }

        //        */
        //    if (!WebMatrix.WebData.WebSecurity.UserExists("admin1"))
        //    {
        //        string userName = WebMatrix.WebData.WebSecurity.CreateUserAndAccount("admin1", "administrador", new { email = "admin1@app.net", ativo = true, entidadeId = db.Entidades.Single(e => e.Nome == "ISP").Id }, false);
        //        System.Web.Security.Roles.AddUserToRole("admin1", "Admin");
        //    }
            
        //}

        //public static void AddNotificacoes(DomainModels db)
        //{

        //    foreach (Entidade e in db.Entidades)
        //    {
        //        for (int i = 0; i < 2; i++)
        //        {
        //            Notificacao n = new Notificacao { tipo = Notificacao.TipoNotificacao.ErroFicheiro, mensagem = "Notificacao para " + e.Nome, dataCriacao = DateTime.Now, lida = false };
        //            e.notificacoes.Add(n);
        //            n = new Notificacao { tipo = Notificacao.TipoNotificacao.SucessoFicheiro, mensagem = "Notificacao para " + e.Nome, dataCriacao = DateTime.Now, lida = false };
        //            e.notificacoes.Add(n);
        //            n = new Notificacao { tipo = Notificacao.TipoNotificacao.WarningFicheiro, mensagem = "Notificacao para " + e.Nome, dataCriacao = DateTime.Now, lida = false };
        //            e.notificacoes.Add(n);
        //        }
        //        Notificacao n1 = new Notificacao { tipo = Notificacao.TipoNotificacao.ErroFicheiro, mensagem = "Notificacao para " + e.Nome, dataCriacao = DateTime.Now, lida = true };
        //        e.notificacoes.Add(n1);
        //    }
        //    db.SaveChanges();
        //}


        //public static void AddIndicadores(DomainModels db)
        //{
        //    foreach (Entidade entidade in db.Entidades)
        //    {
        //        if (entidade.indicadores.Count == 0)
        //        {
        //            Indicador i = new NumRegistosInd { descricao = "Número de registos", entidade = entidade };
        //            entidade.indicadores.Add(i);
        //            i = new NumApolicesInd { descricao = "Número de apólices", entidade = entidade };
        //            entidade.indicadores.Add(i);
        //        }
        //    }
        //}
    }
}