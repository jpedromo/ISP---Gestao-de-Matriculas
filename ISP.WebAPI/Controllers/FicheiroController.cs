using ISP.GestaoMatriculas.Model;
using ISP.GestaoMatriculas.Repositories;
using ISP.GestaoMatriculas.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ISP.WebAPI.Filters;
//using Microsoft.ApplicationServer.Caching;


namespace ISP.WebAPI.Controllers
{
    [RequireHttpsAttribute]
    public class FicheiroController : ApiController
    {
        static readonly IFicheiroRepository repository = new FicheiroRepository();
        //private DataCache m_cache = CacheUtil.GetCache();

        [Authorize]
        public IEnumerable<Ficheiro> GetAllFicheiros()
        {
            return repository.All;
        }

        public Ficheiro GetFicheiro(int id)
        {
            Ficheiro item = repository.Find(id);
            if (item == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return item;
        }

        public HttpResponseMessage PostFicheiro(Ficheiro item)
        {
            item = repository.Insert(item);
            var response = Request.CreateResponse<Ficheiro>(HttpStatusCode.Created, item);

            //string uri = Url.Link("DefaultApi", new { id = item.ficheiroId });
            //response.Headers.Location = new Uri(uri);
            return response;
        }

        public void PutFicheiro(int id, Ficheiro ficheiro)
        {
            ficheiro.ficheiroId = id;
            if (ficheiro.ficheiroId == default(int) || !repository.Update(ficheiro))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

    }
}
