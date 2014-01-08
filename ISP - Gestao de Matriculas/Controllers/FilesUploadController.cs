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
<<<<<<< HEAD
using ISP.GestaoMatriculas.Filters;
using Everis.Web.Mvc;
using PagedList;
using ISP.GestaoMatriculas.Utils;

namespace ISP.GestaoMatriculas.Controllers
{
    [InitializeSimpleMembership(Order = 1)]
    [Authorize(Order = 2)]
    public class FilesUploadController : SKController
    {
        private IFicheiroRepository ficheirosRepository;
        private IUserProfileRepository usersRepository;
        private IEntidadeRepository entidadesRepository;
        private INotificacaoRepository notificacoesRepository;
        private IValorSistemaRepository valoresSistemaRepository;

        public FilesUploadController(IFicheiroRepository ficheirosRepository, IUserProfileRepository usersRepository, IEntidadeRepository entidadesRepository,
            INotificacaoRepository notificacoesRepository, IValorSistemaRepository valoresSistemaRepository)
        {
            this.ficheirosRepository = ficheirosRepository;
            this.usersRepository = usersRepository;
            this.entidadesRepository = entidadesRepository;
            this.notificacoesRepository = notificacoesRepository;
            this.valoresSistemaRepository = valoresSistemaRepository;
        }

        //
        // Get: /Index/
        //
        public ActionResult Index(FilesUploadListViewModel viewModel, string sort, string direction, int? page, string tabNr)
        {
            if (tabNr == null || tabNr == "")
                tabNr = "1";

            viewModel.SetParameters(int.Parse(tabNr), page == null ? 1 : page.Value, sort, direction, Request.Params);

            FilesUploadListViewModel result = getFicheiros(viewModel);
            return View(result);
        }

        public ActionResult exportFicheirosToCsv(FilesUploadListViewModel viewModel, string sort, string direction, string tabNr)
        {
            if (tabNr == null || tabNr == "")
                tabNr = "1";

            viewModel.SetParameters(int.Parse(tabNr), 1, sort, direction, Request.Params);

            viewModel.PageSize = 0;
            FilesUploadListViewModel result = getFicheiros(viewModel);

            List<FicheiroToCsv> ficheirosCsv = new List<FicheiroToCsv>();
            foreach (Ficheiro file in result.ficheiros)
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
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "Ficheiros_" + DateTime.Now.ToShortDateString() + ".csv");
        }

        protected FilesUploadListViewModel getFicheiros(FilesUploadListViewModel viewModel)
        {
            IPagedList<Ficheiro> ficheirosToView = null;
            IQueryable<Ficheiro> query = null;

            UserProfile user = usersRepository.GetUserByIDIncludeEntidade(WebSecurity.CurrentUserId);

            if (viewModel.entidadeId == default(int))
                viewModel.entidadeId = user.entidadeId;

            int entId = viewModel.entidadeId;
            if (user.entidade.nome == "ISP" && viewModel.entidadeId == user.entidadeId)
                entId = -1;
            
            if (user.entidade.scope.valor == "GLOBAL")
            {
                if(entId == -1)
                    query = ficheirosRepository.All.OrderByDescending(f => f.dataUpload);
                else
                    query = ficheirosRepository.All.Where(f => f.entidadeId == viewModel.entidadeId).OrderByDescending(f => f.dataUpload);
                this.ViewBag.entidadeId = new SelectList(entidadesRepository.All.Where(e => e.nome != "ISP") , "entidadeId", "nome", viewModel.entidadeId);
            }
            else
            {
                query = ficheirosRepository.All.Where(f => f.entidadeId == user.entidadeId).OrderByDescending(f => f.dataUpload);
                this.ViewBag.entidadeId = new SelectList(entidadesRepository.All.Where(e => e.nome != "ISP"), "entidadeId", "nome", user.entidadeId);
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
                        ficheirosToView = query.OrderByDescending(s => s.estado.descricao).ToPagedList(viewModel.CurrentPageNumber, viewModel.PageSize);
                    else
                        ficheirosToView = query.OrderBy(s => s.estado.descricao).ToPagedList(viewModel.CurrentPageNumber, viewModel.PageSize);
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
=======

namespace ISP.GestaoMatriculas.Models.DbPopulate
{
    public class FilesUploadController : Controller
    {
        private IFicheiroRepository ficheirosRepository;
        private IUserProfileRepository usersRepository;

        public FilesUploadController(IFicheiroRepository ficheirosRepository, IUserProfileRepository usersRepository)
        {
            this.ficheirosRepository = ficheirosRepository;
            this.usersRepository = usersRepository;
        }

        //
        // GET: /Index/
        //
        public ActionResult Index(bool? upload)
        {
            FilesUploadViewModel viewModel = new FilesUploadViewModel();

            UserProfile user = usersRepository.All.Include("entidade").Single(u => u.UserId  == WebSecurity.CurrentUserId);
            IQueryable<Ficheiro> query = null;
            query = ficheirosRepository.All.Where(f => f.entidadeId == user.entidadeId).OrderByDescending(f => f.dataUpload);
            viewModel.ficheiros = query.ToList();

            if (upload != null)
            { 
                viewModel.upload = (bool)upload;
            }

            return View(viewModel);
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
        }

        
        //
        // POST: /Upload
        //
<<<<<<< HEAD
        [Authorize(Roles = "Admin, ISP, Seguradora")]
        [HttpPost]
        public ActionResult Upload(FilesUploadListViewModel model, HttpPostedFileBase file)
        {
            //List<ValorSistema> paramSistema = valoresSistemaRepository.All.Where(v => v.tipologia == "ESTADO_FICHEIRO"
            //                                                                    || v.tipologia == "PARAM_PASTA_FICHEIROS"
            //                                                                    || v.tipologia == "PARAM_PASTA_FICHEIROS_ISENTOS").ToList();
            //List<ValorSistema> estadosFicheiro = paramSistema.Where(v => v.tipologia == "ESTADO_FICHEIRO").ToList();
            //ValorSistema pastaUpload = paramSistema.Where(v => v.tipologia == "PARAM_PASTA_FICHEIROS").Single();
            //ValorSistema pastaUploadIsentos = paramSistema.Where(v => v.tipologia == "PARAM_PASTA_FICHEIROS_ISENTOS").Single();

            List<ValorSistema> estadosFicheiro = valoresSistemaRepository.GetPorTipologia("ESTADO_FICHEIRO");
            ValorSistema pastaUpload = valoresSistemaRepository.GetPorTipologia("PARAM_PASTA_FICHEIROS").Single();
            ValorSistema pastaUploadIsentos = valoresSistemaRepository.GetPorTipologia("PARAM_PASTA_FICHEIROS_ISENTOS").Single();
            

=======
        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
            if (file != null && file.ContentLength > 0)
            {
                DateTime dataSubmissao = DateTime.Now;
                var filename = Path.GetFileNameWithoutExtension(file.FileName);
                var fileExtension = Path.GetExtension(file.FileName);
                var newFilename = filename + "_" + dataSubmissao.ToString("yyyyMMddHHmmssfff") + fileExtension;
<<<<<<< HEAD
                var destination = pastaUpload.valor;
                //var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), filename);
                var path = Path.Combine(destination, newFilename);
                file.SaveAs(path);

                UserProfile user = usersRepository.GetUserByIDIncludeEntidade(WebSecurity.CurrentUserId);
                int entidadeID;
                if (user.entidade.scope.valor == "GLOBAL")
                    entidadeID = model.entidadeId;
                else
                    entidadeID = user.entidadeId;


                Ficheiro newFicheiro = new Ficheiro { 
                    nomeFicheiro = filename,
                    localizacao = path,
                    dataUpload = dataSubmissao,
                    dataAlteracao = dataSubmissao,
                    entidadeId = entidadeID,
                    estadoId = estadosFicheiro.Where(f => f.valor == "SUBMETIDO").Single().valorSistemaId,
                    userName = user.UserName
                };

                ficheirosRepository.Insert(newFicheiro);
                ficheirosRepository.Save();

=======
                var destination = System.Configuration.ConfigurationManager.AppSettings["FilesUploadDestination"];
                //var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), filename);
                var path = Path.Combine(destination, newFilename);
                file.SaveAs(path);
         
                UserProfile user = usersRepository.All.Include("entidade").Single(u => u.UserId  == WebSecurity.CurrentUserId);
                Entidade entidadeAssociada = user.entidade;

                Ficheiro newFicheiro = new Ficheiro();
                newFicheiro.nomeFicheiro = filename;
                newFicheiro.localizacao = path;
                newFicheiro.dataUpload = dataSubmissao;
                newFicheiro.dataAlteracao = dataSubmissao;
                //newFicheiro.entidade = entidadeAssociada;
                newFicheiro.entidadeId = entidadeAssociada.Id;
                newFicheiro.estado = Ficheiro.EstadoFicheiro.submetido;
                newFicheiro.erro = false;
                newFicheiro.userName = user.UserName;

                ficheirosRepository.Insert(newFicheiro);
                ficheirosRepository.Save();
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
            }

            return RedirectToAction("Index");            
        }

        //
        // GET: /Processar/2
        //
<<<<<<< HEAD
        [Authorize(Roles = "Admin, ISP, Seguradora")]
        public ActionResult Processar(int id)
        {
            List<ValorSistema> estadosFicheiro = valoresSistemaRepository.GetPorTipologia("ESTADO_FICHEIRO").ToList();
            List<ValorSistema> tiposNotificao = valoresSistemaRepository.GetPorTipologia("TIPO_NOTIFICACAO").ToList();
=======
        public ActionResult Processar(int id)
        {
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
            UserProfile user = usersRepository.Find(WebSecurity.CurrentUserId);

            Ficheiro ficheiro = null;
            try
            {
                ficheiro = ficheirosRepository.All.Single(f => f.ficheiroId  == id);
                //Altera estado do ficheiro
<<<<<<< HEAD
                if (ficheiro.estado.valor == "SUBMETIDO")
                {
                    ficheiro.estadoId = estadosFicheiro.Where(f => f.valor == "PENDENTE").Single().valorSistemaId;
=======
                if (ficheiro.estado == Ficheiro.EstadoFicheiro.submetido)
                {
                    ficheiro.estado = Ficheiro.EstadoFicheiro.pendente;
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
                    ficheiro.dataAlteracao = DateTime.Now;
                    ficheiro.userName = user.UserName;

                    ficheirosRepository.InsertOrUpdate(ficheiro);
                    ficheirosRepository.Save();
<<<<<<< HEAD

                    Notificacao notificacao = new Notificacao
                    {
                        dataCriacao = DateTime.Now,
                        entidadeId = ficheiro.entidadeId,
                        email = true,
                        tipologiaId = tiposNotificao.Where(f => f.valor == "SUCESSO_RECECAO_FICHEIRO").Single().valorSistemaId,
                        mensagem = "O 'Ficheiro Nacional de Matrículas do Parque Automóvel Seguro' com nome '" + ficheiro.nomeFicheiro + "' e data de reporte '" + ficheiro.dataUpload.ToShortDateString() + "' foi recebido com sucesso. "+
                        "O ficheiro encontra-se no estado 'Pendente'."
                    };

                    Entidade entidade = entidadesRepository.Find((int)notificacao.entidadeId);

                    entidade.notificacoes.Add(notificacao);
                    entidadesRepository.Save();
                    notificacoesRepository.Save();

=======
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
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

<<<<<<< HEAD
            this.Alerts.Success("Ficheiro marcado para processamento.");
            return this.RedirectToAction("Index", new { tabNr = 2 }); 
=======
            
            return this.RedirectToAction("Index"); 
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
        }

        //
        // GET: /Cancelar/2
        //
<<<<<<< HEAD
        [Authorize(Roles = "Admin, ISP, Seguradora")]
        public ActionResult Cancelar(int id)
        {
            List<ValorSistema> estadosFicheiro = valoresSistemaRepository.GetPorTipologia("ESTADO_FICHEIRO");
=======
        public ActionResult Cancelar(int id)
        {
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
            UserProfile user = usersRepository.Find(WebSecurity.CurrentUserId);

            Ficheiro ficheiro = null;
            try
            {
                ficheiro = ficheirosRepository.All.Single(f => f.ficheiroId == id);
                //Altera estado do ficheiro
<<<<<<< HEAD
                if (ficheiro.estado.valor == "SUBMETIDO")
                {
                    ficheiro.estadoId = estadosFicheiro.Where(f => f.valor == "CANCELADO").Single().valorSistemaId; ;
=======
                if (ficheiro.estado == Ficheiro.EstadoFicheiro.submetido)
                {
                    ficheiro.estado = Ficheiro.EstadoFicheiro.cancelado;
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
                    ficheiro.dataAlteracao = DateTime.Now;
                    ficheiro.userName = user.UserName;

                    ficheirosRepository.InsertOrUpdate(ficheiro);
                    ficheirosRepository.Save();
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

<<<<<<< HEAD
            this.Alerts.Success("Ficheiro cancelado.");
            return this.RedirectToAction("Index", new { tabNr = 2 }); 
=======
            return this.RedirectToAction("Index"); 
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
        }
    }
}
