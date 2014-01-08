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
using WebMatrix.WebData;
using System.Data.Entity;
using ISP.WebAPI.Filters;
using ISP.WebAPI.Models;

namespace ISP.WebAPI.Controllers
{
    public class EnvioOcorrenciasController : ApiController
    {
        private IUserProfileRepository usersRepository;
        private IEntidadeRepository entidadesRepository;
        private INotificacaoRepository notificacoesRepository;
        private IEventoStaggingRepository eventosStaggingRepository;
        private IValorSistemaRepository valoresSistemaRepository;

        public EnvioOcorrenciasController(IUserProfileRepository usersRepository, IValorSistemaRepository valoresSistemaRepository,
            IEntidadeRepository entidadesRepository, INotificacaoRepository notificacoesRepository, IEventoStaggingRepository eventosStaggingRepository)
        {
            this.usersRepository = usersRepository;
            this.entidadesRepository = entidadesRepository;
            this.notificacoesRepository = notificacoesRepository;
            this.eventosStaggingRepository = eventosStaggingRepository;
            this.valoresSistemaRepository = valoresSistemaRepository;
        }


        /// <summary>
        /// Realiza o Upload de operações sobre períodos segurados
        /// </summary>
        /// <param name="operation">operação sobre período seguro</param>
        [BasicAuthorize]
        [HttpPost]
        [ActionName("ProtectedAction")]
        public ResultResponse<EnvioOcorrenciasResult> Post([FromBody]ReporteOcorrenciasMatricula ocorrencias)
        {
            ResultResponse<EnvioOcorrenciasResult> resposta = new ResultResponse<EnvioOcorrenciasResult>();

            //List<ValorSistema> paramSistema = valoresSistemaRepository.All.Where(v => v.tipologia == "ESTADO_EVENTO_STAGGING"
            //                                                                    || v.tipologia == "OPERACAO_EVENTO"
            //                                                                    || v.tipologia == "PARAM_PASTA_FICHEIROS").ToList();
            //List<ValorSistema> estadosEvento = paramSistema.Where(v => v.tipologia == "ESTADO_EVENTO_STAGGING").ToList();
            //List<ValorSistema> operacoesEvento = paramSistema.Where(v => v.tipologia == "OPERACAO_EVENTO").ToList();
            //ValorSistema pastaUpload = paramSistema.Where(v => v.tipologia == "PARAM_PASTA_FICHEIROS").Single();

            List<ValorSistema> estadosEvento = valoresSistemaRepository.GetPorTipologia("ESTADO_EVENTO_STAGGING");
            List<ValorSistema> operacoesEvento = valoresSistemaRepository.GetPorTipologia("OPERACAO_EVENTO");
            ValorSistema pastaUpload = valoresSistemaRepository.GetPorTipologia("PARAM_PASTA_FICHEIROS").Single();

            
            if (ocorrencias == null || ocorrencias.Ocorrencia == null)
            {
                resposta.Success = false;
                resposta.ErrorMessage = "Erro na recepção de ocorrências. Mensagem mal formada.";
            }

            UserProfile user = null;

            if (WebSecurity.IsAuthenticated)
            {
                user = usersRepository.GetUserByIDIncludeEntidade(WebSecurity.CurrentUserId);
            }
            else
            {
                string username = string.Empty;

                IEnumerable<string> headerVals;
                if (Request.Headers.TryGetValues("Authorization", out headerVals))
                {
                    string authHeader = headerVals.FirstOrDefault();
                    char[] delims = { ' ' };
                    string[] authHeaderTokens = authHeader.Split(new char[] { ' ' });
                    if (authHeaderTokens[0].Contains("Basic"))
                    {
                        string decodedStr = BasicAuthorizeAttribute.DecodeFrom64(authHeaderTokens[1]);
                        string[] unpw = decodedStr.Split(new char[] { ':' });
                        username = unpw[0];
                    }
                    else
                    {
                        if (authHeaderTokens.Length > 1)
                            username = BasicAuthorizeAttribute.DecodeFrom64(authHeaderTokens[1]);
                    }
                }

                user = usersRepository.GetUserByUsernameIncludeEntidade(username);
            }

            if (user == null)
            {
                resposta.Success = false;
                resposta.ErrorMessage += "Erro de autenticação.";
            }

            var root = System.Configuration.ConfigurationManager.AppSettings["FilesUploadDestination"];
            //string root = System.Web.HttpContext.Current.Server.MapPath(destination);
            var provider = new MultipartFormDataStreamProvider(root);

            if (resposta.Success == false)
            {
                return resposta;
            }

            DateTime dataRecepcao = DateTime.Now;

            ReporteOcorrenciasFNMPAS reporteOcorrenciasFNMPAS = new ReporteOcorrenciasFNMPAS(ocorrencias);
            reporteOcorrenciasFNMPAS.ordenaOcorrencias();

            int nrOcorrencias = 0;
            for (int i = 0; i < reporteOcorrenciasFNMPAS.OcorrenciaOrdenada.Count(); i++)
            {
                ReporteOcorrenciasMatriculaOcorrenciaOrdenada ocorrencia = reporteOcorrenciasFNMPAS.OcorrenciaOrdenada[i];

                //evento Duplicado e esmagado com os novos dados. (C M - stagging/producao) (A - producao/stagging) 
                //Não foi ainda inserido na Base de dados nem houve delete logico do anterior. 
                //Fazer deletes logicos, e inserções após verificações.
                //Anulações podem necessitar logica adicional caso a validação falhe. (duplicar registo em stagging)
                EventoStagging evento = new EventoStagging();

                int codigoOperacaoId = 0;
                switch (ocorrencia.CodigoOperacao)
                {
                    case ReporteOcorrenciasMatriculaOcorrenciaCodigoOperacao.C: codigoOperacaoId = operacoesEvento.Where(o => o.valor == "C").Single().valorSistemaId; break;
                    case ReporteOcorrenciasMatriculaOcorrenciaCodigoOperacao.M: codigoOperacaoId = operacoesEvento.Where(o => o.valor == "M").Single().valorSistemaId; break;
                    case ReporteOcorrenciasMatriculaOcorrenciaCodigoOperacao.A: codigoOperacaoId = operacoesEvento.Where(o => o.valor == "A").Single().valorSistemaId; break;
                    default: codigoOperacaoId = operacoesEvento.Where(o => o.valor == "C").Single().valorSistemaId; break;
                }

                Ficheiro ficheiroVirtual = new Ficheiro { dataAlteracao = dataRecepcao, dataUpload = dataRecepcao, entidadeId = user.entidadeId };

                evento.esmagaDados(ocorrencia, ficheiroVirtual, codigoOperacaoId);
                evento.utilizadorReporte = user.UserName;

                evento.estadoEventoId = estadosEvento.Where(e => e.valor == "PENDENTE").Single().valorSistemaId;

                eventosStaggingRepository.InsertOrUpdate(evento);
                nrOcorrencias++;
            }

                eventosStaggingRepository.Save();
                resposta.Success = true;
                resposta.Result = new EnvioOcorrenciasResult { dataRecepcao = dataRecepcao, nrOcorrencias = nrOcorrencias, seguradora = user.entidade.nome };
            
            return resposta;
        }

        ///// <summary>
        ///// Realiza o Upload de operações sobre períodos segurados
        ///// </summary>
        ///// <param name="operation">operação sobre período seguro</param>
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
