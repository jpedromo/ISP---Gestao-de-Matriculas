using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using ISP.GestaoMatriculas.ViewModels;
using ISP.GestaoMatriculas.Model;
using Everis.Web.Mvc;
using ISP.GestaoMatriculas.Contracts;
using ISP.GestaoMatriculas.Repositories;

namespace ISP.GestaoMatriculas.Controllers
{
    public class SitePublicoController : SKController
    {
        //
        // GET: /SitePublico/
        private IApoliceRepository apolicesRepository;
        private IApoliceIsentoRepository apolicesIsentosRepository;
        public SitePublicoController(IApoliceRepository apolicesRepository, IApoliceIsentoRepository apolicesIsentosRepository)
        {
            this.apolicesRepository = apolicesRepository;
            this.apolicesIsentosRepository = apolicesIsentosRepository;
        }

        public ActionResult Index()
        {
            return View(new PesquisaPublicaViewModel { dataPesquisa = DateTime.Now });
        }

        [HttpPost]
        public ActionResult Index(PesquisaPublicaViewModel form)
        {
            if (ModelState.IsValid)
            {
                List<Apolice> apolices = apolicesRepository.All.Include("entidade").Include("veiculo").Where(a => (a.veiculo.numeroMatricula == form.matricula || a.veiculo.numeroMatriculaCorrigido == form.matricula) && a.dataInicio <= form.dataPesquisa && a.dataFim >= form.dataPesquisa).ToList();
                List<ApoliceIsento> isentos = apolicesIsentosRepository.All.Where(i => (i.matricula == form.matricula || i.matriculaCorrigida == form.matricula|| i.matriculaCorrigida == form.matricula.Replace("-", "")) && i.dataInicio <= form.dataPesquisa && i.confidencial == false && i.arquivo == false).ToList();

                List<ApolicePublicaView> apolicesPublicas = new List<ApolicePublicaView>();
                foreach (Apolice a in apolices)
                {
                    apolicesPublicas.Add(new ApolicePublicaView
                    {
                        dataInicio = a.dataInicio,
                        dataFim = a.dataFim,
                        marcaVeiculo = a.veiculo.marcaVeiculo,
                        modeloVeiculo = a.veiculo.modeloVeiculo,
                        numeroApolice = a.numeroApolice,
                        seguradora = a.entidade.nome
                    });
                }

                foreach (ApoliceIsento i in isentos)
                {
                    if (i.dataFim != null)
                    {
                        if (i.dataFim < form.dataPesquisa)
                        {
                            continue;
                        }
                    }
                    ApolicePublicaView apoliceToView = new ApolicePublicaView
                    {
                        dataInicio = i.dataInicio,
                        seguradora = "Veículo Isento de Seguro"
                    };

                    if(i.dataFim != null){
                        apoliceToView.dataFim = (DateTime)i.dataFim;
                    }else{
                        apoliceToView.dataFim = DateTime.Now;
                    }

                    apolicesPublicas.Add(apoliceToView);
                }

                form.resultado = apolicesPublicas.OrderByDescending( a => a.dataInicio );
            }
                return this.View(form);
            
        }

    }
}
