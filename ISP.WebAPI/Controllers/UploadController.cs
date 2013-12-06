using ISP.GestaoMatriculas.Model;
using ISP.GestaoMatriculas.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using System.IO;
using System.Web;

namespace ISP.WebAPI.Controllers
{
    public class UploadController : ApiController
    {
        //static readonly IOperationEntryRepository repository = new OperationEntryRepository();

        /// <summary>
        /// Obtém Ficheiros
        /// </summary>
        [HttpGet]
        [ActionName("selectFiles")]
        public string Get()
        {
            return "teste .............";
        }

        /// <summary>
        /// Obtém Ficheiro
        /// </summary>
        [HttpGet]
        [ActionName("selectFile")]
        public EventoStagging Get(int id)
        {
            return new EventoStagging();
        }

        /// <summary>
        /// Realiza o Upload de operações sobre períodos segurados
        /// </summary>
        /// <param name="operation">operação sobre período segurado</param>
        [HttpPost]
        [ActionName("UploadOperations")]
        public HttpResponseMessage Post([FromBody]EventoStagging operation)
        {

            return new HttpResponseMessage(HttpStatusCode.Created);
        }

        ///// <summary>
        ///// Realiza o Upload de operações sobre períodos segurados
        ///// </summary>
        ///// <param name="operation">operação sobre período segurado</param>
        //[HttpPost]
        //[ActionName("UploadOperations2")]
        //public HttpResponseMessage Post([FromBody]String operation)
        //{

        //    return new HttpResponseMessage(HttpStatusCode.Created);
        //}

        ///// <summary>
        ///// Realiza o Upload de um ficheiro XML de operações sobre períodos segurados
        ///// </summary>
        //[HttpPost]
        //[ActionName("UploadFile")]
        //public Task<HttpResponseMessage> Post() 
        //{
        //    HttpRequestMessage request = this.Request;
        //    if (!request.Content.IsMimeMultipartContent())
        //    {
        //        throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.UnsupportedMediaType));
        //    }

        //    string root = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/uploads");
        //    var provider = new MultipartFormDataStreamProvider(root);

        //    var task = request.Content.ReadAsMultipartAsync(provider).
        //        ContinueWith<HttpResponseMessage>(o =>
        //        {
        //            FileInfo finfo = new FileInfo(provider.FileData.First().LocalFileName);

        //            string guid = Guid.NewGuid().ToString();
        //            string filePath = provider.FileData.First().Headers.ContentDisposition.FileName.Replace("\"", "");
        //            filePath = filePath.Substring(filePath.LastIndexOf('\\')+1);
        //            File.Move(finfo.FullName, Path.Combine(root, guid + "_" + filePath));

        //            return new HttpResponseMessage(HttpStatusCode.Created);
        //        }
        //    );
        //    return task;
        //}

        

        //[HttpPost]
        //[ActionName("UploadFile")]
        //public HttpResponseMessage Post()
        //{
        //    HttpResponseMessage result = null;
        //    var httpRequest = HttpContext.Current.Request;
        //    if (httpRequest.Files.Count > 0)
        //    {
        //        var docfiles = new List<string>();
        //        foreach (string file in httpRequest.Files)
        //        {
        //            var postedFile = httpRequest.Files[file];
        //            var filePath = HttpContext.Current.Server.MapPath("~/App_Data/uploads" + postedFile.FileName);
        //            postedFile.SaveAs(filePath);

        //            docfiles.Add(filePath);
        //        }
        //        result = Request.CreateResponse(HttpStatusCode.Created, docfiles);
        //    }
        //    else
        //    {
        //        result = Request.CreateResponse(HttpStatusCode.BadRequest);
        //    }
        //    return result;
        //}
    }
}
