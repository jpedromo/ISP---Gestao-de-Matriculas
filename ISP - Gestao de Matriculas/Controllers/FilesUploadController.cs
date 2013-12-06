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
        }

        
        //
        // POST: /Upload
        //
        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                DateTime dataSubmissao = DateTime.Now;
                var filename = Path.GetFileNameWithoutExtension(file.FileName);
                var fileExtension = Path.GetExtension(file.FileName);
                var newFilename = filename + "_" + dataSubmissao.ToString("yyyyMMddHHmmssfff") + fileExtension;
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
            }

            return RedirectToAction("Index");            
        }

        //
        // GET: /Processar/2
        //
        public ActionResult Processar(int id)
        {
            UserProfile user = usersRepository.Find(WebSecurity.CurrentUserId);

            Ficheiro ficheiro = null;
            try
            {
                ficheiro = ficheirosRepository.All.Single(f => f.ficheiroId  == id);
                //Altera estado do ficheiro
                if (ficheiro.estado == Ficheiro.EstadoFicheiro.submetido)
                {
                    ficheiro.estado = Ficheiro.EstadoFicheiro.pendente;
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

            
            return this.RedirectToAction("Index"); 
        }

        //
        // GET: /Cancelar/2
        //
        public ActionResult Cancelar(int id)
        {
            UserProfile user = usersRepository.Find(WebSecurity.CurrentUserId);

            Ficheiro ficheiro = null;
            try
            {
                ficheiro = ficheirosRepository.All.Single(f => f.ficheiroId == id);
                //Altera estado do ficheiro
                if (ficheiro.estado == Ficheiro.EstadoFicheiro.submetido)
                {
                    ficheiro.estado = Ficheiro.EstadoFicheiro.cancelado;
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

            return this.RedirectToAction("Index"); 
        }
    }
}
