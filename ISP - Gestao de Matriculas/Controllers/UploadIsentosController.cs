using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using ISP.GestaoMatriculas.Model;
using ISP.GestaoMatriculas.Contracts;
using ISP.GestaoMatriculas.ViewModels;
using System.Data;
using System.Data.Entity;
using WebMatrix.WebData;
using ISP.GestaoMatriculas.Filters;
using Everis.Web.Mvc;
using ISP.GestaoMatriculas.Utils;
using PagedList;

namespace ISP.GestaoMatriculas.Controllers
{
    [InitializeSimpleMembership(Order = 1)]
    [Authorize(Order = 2)]
    public class UploadIsentosController : SKController
    {
        private IFicheiroIsentosRepository ficheirosIsentosRepository;
        private IUserProfileRepository usersRepository;
        private IEntidadeRepository entidadesRepository;
        private INotificacaoRepository notificacoesRepository;
        private IValorSistemaRepository valoresSistemaRepository;

        public UploadIsentosController(IFicheiroIsentosRepository ficheirosIsentosRepository, IUserProfileRepository usersRepository, IEntidadeRepository entidadesRepository,
            INotificacaoRepository notificacoesRepository, IValorSistemaRepository valoresSistemaRepository)
        {
            this.ficheirosIsentosRepository = ficheirosIsentosRepository;
            this.usersRepository = usersRepository;
            this.entidadesRepository = entidadesRepository;
            this.notificacoesRepository = notificacoesRepository;
            this.valoresSistemaRepository = valoresSistemaRepository;
        }

        //
        // GET: /Index/
        //
        [Authorize(Roles = "Admin, ISP, ISP-Leitura")]
        public ActionResult Index(UploadIsentosViewModel viewModel, string sort, string direction, int? page, string tabNr)
        {
            if (tabNr == null || tabNr == "")
                tabNr = "1";

            viewModel.SetParameters(int.Parse(tabNr), page == null ? 1 : page.Value, sort, direction, Request.Params);

            UploadIsentosViewModel result = getFicheiros(viewModel);
            return View(result);
        }

        public ActionResult exportFicheirosToCsv(UploadIsentosViewModel viewModel, string sort, string direction, string tabNr)
        {
            viewModel.PageSize = 0;
            if (tabNr == null || tabNr == "")
                tabNr = "1";

            viewModel.SetParameters(int.Parse(tabNr), 1, sort, direction, Request.Params);

            UploadIsentosViewModel result = getFicheiros(viewModel);

            List<FicheiroToCsv> ficheirosCsv = new List<FicheiroToCsv>();
            foreach (FicheiroIsentos file in result.ficheiros)
                ficheirosCsv.Add(new FicheiroToCsv
                {
                    entidade = file.entidade.nome,
                    nome = file.nomeFicheiro,
                    estado = file.estado.descricao.ToString(),
                    dataUpload = file.dataUpload.ToString(),
                    dataAlteracao = file.dataAlteracao.ToString(),
                    username = file.userName
                });

            CsvExport<FicheiroToCsv> csv = new CsvExport<FicheiroToCsv>(ficheirosCsv);

            byte[] fileBytes = csv.ExportToBytes();
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "FicheirosIsentos_" + DateTime.Now.ToShortDateString() + ".csv");
        }

        protected UploadIsentosViewModel getFicheiros(UploadIsentosViewModel viewModel)
        {
            IPagedList<FicheiroIsentos> ficheirosToView = null;
            IQueryable<FicheiroIsentos> query = null;

            UserProfile user = usersRepository.GetUserByIDIncludeEntidade(WebSecurity.CurrentUserId);

            if (viewModel.entidadeId == default(int))
                viewModel.entidadeId = user.entidadeId;

            int entId = viewModel.entidadeId;
            if (user.entidade.nome == "ISP" && viewModel.entidadeId == user.entidadeId)
                entId = -1;
            
            if (user.entidade.scope.valor == "GLOBAL")
            {
                if(entId == -1)
                    query = ficheirosIsentosRepository.All.OrderByDescending(f => f.dataUpload);
                else
                    query = ficheirosIsentosRepository.All.Where(f => f.entidadeId == viewModel.entidadeId).OrderByDescending(f => f.dataUpload);
                this.ViewBag.entidadeId = new SelectList(entidadesRepository.All, "entidadeId", "nome", viewModel.entidadeId);
            }
            else
            {
                query = ficheirosIsentosRepository.All.Where(f => f.entidadeId == user.entidadeId).OrderByDescending(f => f.dataUpload);
                this.ViewBag.entidadeId = new SelectList(entidadesRepository.All, "entidadeId", "nome", user.entidadeId);
            }


            viewModel.totalNumberOfFicheiros = query.Count();

            if (viewModel.PageSize == 0)
                viewModel.PageSize = viewModel.totalNumberOfFicheiros;

            switch (viewModel.SortColumn)
            {
                case "Entidade":
                    if (viewModel.SortDirection == System.ComponentModel.ListSortDirection.Descending)
                        ficheirosToView = query.OrderByDescending(s => s.entidade.nome).ToPagedList(viewModel.CurrentPageNumber, viewModel.PageSize);
                    else
                        ficheirosToView = query.OrderBy(s => s.entidade.nome).ToPagedList(viewModel.CurrentPageNumber, viewModel.PageSize);
                    break;
                case "NomeFicheiro":
                    if (viewModel.SortDirection == System.ComponentModel.ListSortDirection.Descending)
                        ficheirosToView = query.OrderByDescending(s => s.nomeFicheiro).ToPagedList(viewModel.CurrentPageNumber, viewModel.PageSize);
                    else
                        ficheirosToView = query.OrderBy(s => s.nomeFicheiro).ToPagedList(viewModel.CurrentPageNumber, viewModel.PageSize);
                    break;
                case "Estado":
                    if (viewModel.SortDirection == System.ComponentModel.ListSortDirection.Descending)
                        ficheirosToView = query.OrderByDescending(s => s.estado).ToPagedList(viewModel.CurrentPageNumber, viewModel.PageSize);
                    else
                        ficheirosToView = query.OrderBy(s => s.estado).ToPagedList(viewModel.CurrentPageNumber, viewModel.PageSize);
                    break;
                case "DataUpload":
                    if (viewModel.SortDirection == System.ComponentModel.ListSortDirection.Descending)
                        ficheirosToView = query.OrderByDescending(s => s.dataUpload).ToPagedList(viewModel.CurrentPageNumber, viewModel.PageSize);
                    else
                        ficheirosToView = query.OrderBy(s => s.dataUpload).ToPagedList(viewModel.CurrentPageNumber, viewModel.PageSize);
                    break;
                case "DataAlteracao":
                    if (viewModel.SortDirection == System.ComponentModel.ListSortDirection.Descending)
                        ficheirosToView = query.OrderByDescending(s => s.dataAlteracao).ToPagedList(viewModel.CurrentPageNumber, viewModel.PageSize);
                    else
                        ficheirosToView = query.OrderBy(s => s.dataAlteracao).ToPagedList(viewModel.CurrentPageNumber, viewModel.PageSize);
                    break;
                case "UserName":
                    if (viewModel.SortDirection == System.ComponentModel.ListSortDirection.Descending)
                        ficheirosToView = query.OrderByDescending(s => s.userName).ToPagedList(viewModel.CurrentPageNumber, viewModel.PageSize);
                    else
                        ficheirosToView = query.OrderBy(s => s.userName).ToPagedList(viewModel.CurrentPageNumber, viewModel.PageSize);
                    break;
                default:
                    ficheirosToView = query.OrderByDescending(s => s.dataAlteracao).ToPagedList(viewModel.CurrentPageNumber, viewModel.PageSize);
                    break;
            }

            viewModel.ficheiros = ficheirosToView.ToList();

            return viewModel;
        }

        
        //
        // POST: /Upload
        //
        [Authorize(Roles = "Admin, ISP")]
        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            //List<ValorSistema> paramSistema = valoresSistemaRepository.All.Where(v => v.tipologia == "ESTADO_FICHEIRO"
            //                                                                    || v.tipologia == "PARAM_PASTA_FICHEIROS_ISENTOS").ToList();
            //List<ValorSistema> estadosFicheiro = paramSistema.Where(v => v.tipologia == "ESTADO_FICHEIRO").ToList();
            //ValorSistema pastaUploadIsentos = paramSistema.Where(v => v.tipologia == "PARAM_PASTA_FICHEIROS_ISENTOS").Single();

            List<ValorSistema> estadosFicheiro = valoresSistemaRepository.GetPorTipologia("ESTADO_FICHEIRO");
            ValorSistema pastaUploadIsentos = valoresSistemaRepository.GetPorTipologia("PARAM_PASTA_FICHEIROS_ISENTOS").Single();
            
                
            if (file != null && file.ContentLength > 0)
            {
                DateTime dataSubmissao = DateTime.Now;
                var filename = Path.GetFileNameWithoutExtension(file.FileName);
                var fileExtension = Path.GetExtension(file.FileName);
                var newFilename = filename + "_" + dataSubmissao.ToString("yyyyMMddHHmmssfff") + fileExtension;
                var destination = pastaUploadIsentos.valor;
                //var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), filename);
                var path = Path.Combine(destination, newFilename);
                file.SaveAs(path);

                UserProfile user = usersRepository.GetUserByIDIncludeEntidade(WebSecurity.CurrentUserId);
                Entidade entidadeAssociada = user.entidade;

                FicheiroIsentos newFicheiro = new FicheiroIsentos
                { 
                    nomeFicheiro = filename,
                    localizacao = path,
                    dataUpload = dataSubmissao,
                    dataAlteracao = dataSubmissao,
                    //entidade = entidadeAssociada,
                    entidadeId = entidadeAssociada.entidadeId,
                    estadoId = estadosFicheiro.Where(f => f.valor == "SUBMETIDO").Single().valorSistemaId,
                    userName = user.UserName
                };

                ficheirosIsentosRepository.Insert(newFicheiro);
                ficheirosIsentosRepository.Save();

            }

            return RedirectToAction("Index");            
        }

        //
        // GET: /Processar/2
        //
        [Authorize(Roles = "Admin, ISP")]
        public ActionResult Processar(int id)
        {
            List<ValorSistema> estadosFicheiro = valoresSistemaRepository.GetPorTipologia("ESTADO_FICHEIRO");
            List<ValorSistema> tiposNotificacao = valoresSistemaRepository.GetPorTipologia("TIPO_NOTIFICACAO");
            UserProfile user = usersRepository.Find(WebSecurity.CurrentUserId);

            FicheiroIsentos ficheiro = null;
            try
            {
                ficheiro = ficheirosIsentosRepository.All.Single(f => f.ficheiroIsentosId == id);
                //Altera estado do ficheiro
                if (ficheiro.estado.valor == "SUBMETIDO")
                {
                    ficheiro.estadoId = estadosFicheiro.Where(f => f.valor == "PENDENTE").Single().valorSistemaId;
                    ficheiro.dataAlteracao = DateTime.Now;
                    ficheiro.userName = user.UserName;

                    ficheirosIsentosRepository.InsertOrUpdate(ficheiro);
                    ficheirosIsentosRepository.Save();

                    Notificacao notificacao = new Notificacao
                    {
                        dataCriacao = DateTime.Now,
                        entidadeId = ficheiro.entidadeId,
                        tipologiaId = tiposNotificacao.Where(f => f.valor == "SUCESSO_RECECAO_FICHEIRO").Single().valorSistemaId,
                        mensagem = "Ficheiro de Isentos '" + ficheiro.nomeFicheiro + "' recebido com sucesso",
                    };

                    Entidade entidade = entidadesRepository.Find((int)notificacao.entidadeId);

                    entidade.notificacoes.Add(notificacao);
                    entidadesRepository.Save();
                    notificacoesRepository.Save();

                }
                else
                {
                    return this.HttpNotFound();
                }
            }
            catch (System.ArgumentNullException)
            {
                return this.HttpNotFound();
            }
            catch (System.InvalidOperationException)
            {
                return this.HttpNotFound();
            }

            this.Alerts.Success("Ficheiro de Isentos marcado para processamento.");
            return this.RedirectToAction("Index", new { tabNr = 2 }); 
        }

        //
        // GET: /Cancelar/2
        //
        [Authorize(Roles = "Admin, ISP")]
        public ActionResult Cancelar(int id)
        {
            List<ValorSistema> estadosFicheiro = valoresSistemaRepository.GetPorTipologia("ESTADO_FICHEIRO");
            UserProfile user = usersRepository.Find(WebSecurity.CurrentUserId);

            FicheiroIsentos ficheiro = null;
            try
            {
                ficheiro = ficheirosIsentosRepository.All.Single(f => f.ficheiroIsentosId == id);
                //Altera estado do ficheiro
                if (ficheiro.estado.valor == "SUBMETIDO")
                {
                    ficheiro.estadoId = estadosFicheiro.Where(f => f.valor == "CANCELADO").Single().valorSistemaId;
                    ficheiro.dataAlteracao = DateTime.Now;
                    ficheiro.userName = user.UserName;

                    ficheirosIsentosRepository.InsertOrUpdate(ficheiro);
                    ficheirosIsentosRepository.Save();
                }
                else
                {
                    return this.HttpNotFound();
                }
            }
            catch (System.ArgumentNullException)
            {
                return this.HttpNotFound();
            }
            catch (System.InvalidOperationException)
            {
                return this.HttpNotFound();
            }

            this.Alerts.Success("Ficheiro de Isentos cancelado.");
            return this.RedirectToAction("Index", new { tabNr = 2 }); 
        }
    }
}
