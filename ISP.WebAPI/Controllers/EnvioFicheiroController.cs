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
using System.Data.Entity;
using ISP.WebAPI.Filters;
using WebMatrix.WebData;
using ISP.WebAPI.Models;

namespace ISP.WebAPI.Controllers
{
    public class EnvioFicheiroController : ApiController
    {
        private IUserProfileRepository usersRepository;
        private IFicheiroRepository ficheirosRepository;
        private IEntidadeRepository entidadesRepository;
        private INotificacaoRepository notificacoesRepository;
        private IValorSistemaRepository valoresSistemaRepository;

        public EnvioFicheiroController(IUserProfileRepository usersRepository, IFicheiroRepository ficheirosRepository,
            IValorSistemaRepository valoresSistemaRepository, IEntidadeRepository entidadesRepository, INotificacaoRepository notificacoesRepository)
        {
            this.usersRepository = usersRepository;
            this.ficheirosRepository = ficheirosRepository;
            this.entidadesRepository = entidadesRepository;
            this.notificacoesRepository = notificacoesRepository;
            this.valoresSistemaRepository = valoresSistemaRepository;
        }

        /// <summary>
        /// Realiza o Upload de um ficheiro XML de operações sobre períodos segurados
        /// </summary>
        [BasicAuthorize]
        [HttpPost]
        [ActionName("ProtectedAction")]
        public Task<ResultResponse<EnvioFicheiroResult>> Post()
        {
            ResultResponse<EnvioFicheiroResult> resposta = new ResultResponse<EnvioFicheiroResult>();


            HttpRequestMessage request = this.Request;
            if (!request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.UnsupportedMediaType));
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

                user = usersRepository.All.Include("entidade").Single(u => u.UserName == username);
            }

            if (user == null)
            {
                resposta.Success = false;
                resposta.ErrorMessage += "Erro de autenticação.";
            }


            //List<ValorSistema> paramSistema = valoresSistemaRepository.All.Where(v => v.tipologia == "ESTADO_FICHEIRO"
            //                                                                    || v.tipologia == "TIPO_NOTIFICACAO"
            //                                                                    || v.tipologia == "PARAM_PASTA_FICHEIROS").ToList();
            //List<ValorSistema> estadosFicheiro = paramSistema.Where(v => v.tipologia == "ESTADO_FICHEIRO").ToList();
            //List<ValorSistema> tiposNotificao = paramSistema.Where(v => v.tipologia == "TIPO_NOTIFICACAO").ToList();
            //ValorSistema pastaUpload = paramSistema.Where(v => v.tipologia == "PARAM_PASTA_FICHEIROS").Single();

            List<ValorSistema> estadosFicheiro = valoresSistemaRepository.GetPorTipologia("ESTADO_FICHEIRO");
            List<ValorSistema> tiposNotificao = valoresSistemaRepository.GetPorTipologia("TIPO_NOTIFICACAO");
            ValorSistema pastaUpload = valoresSistemaRepository.GetPorTipologia("PARAM_PASTA_FICHEIROS").Single();
            

            var root = pastaUpload.valor;
            //string root = System.Web.HttpContext.Current.Server.MapPath(destination);
            var provider = new MultipartFormDataStreamProvider(root);

            if (resposta.Success == false)
            {
                request.Content.ReadAsMultipartAsync(provider).
                ContinueWith<ResultResponse<EnvioFicheiroResult>>(o =>
                { return resposta; });
            }

            var task = request.Content.ReadAsMultipartAsync(provider).
                ContinueWith<ResultResponse<EnvioFicheiroResult>>(o =>
                {
                    try
                    {
                        
                        FileInfo finfo = new FileInfo(provider.FileData.First().LocalFileName);

                        DateTime dataSubmissao = DateTime.Now;

                        string guid = Guid.NewGuid().ToString();
                        string filePath = provider.FileData.First().Headers.ContentDisposition.FileName.Replace("\"", "");
                        filePath = filePath.Substring(filePath.LastIndexOf('\\') + 1);

                        var filename = Path.GetFileNameWithoutExtension(filePath);
                        var fileExtension = Path.GetExtension(filePath);
                        var newFilename = filename + "_" + dataSubmissao.ToString("yyyyMMddHHmmssfff") + fileExtension;
                        var path = Path.Combine(root, newFilename);
                        File.Move(finfo.FullName, path);


                        Ficheiro newFicheiro = new Ficheiro
                        {
                            nomeFicheiro = filename,
                            localizacao = path,
                            dataUpload = dataSubmissao,
                            dataAlteracao = dataSubmissao,
                            entidadeId = user.entidadeId,
                            estadoId = estadosFicheiro.Where(f => f.valor == "PENDENTE").Single().valorSistemaId,
                            userName = user.UserName
                        };

                        Notificacao notificacao = new Notificacao
                        {
                            dataCriacao = DateTime.Now,
                            entidadeId = user.entidadeId,
                            tipologiaId = tiposNotificao.Where(f => f.valor == "SUCESSO_RECECAO_FICHEIRO").Single().valorSistemaId,
                            mensagem = "Ficheiro '" +filename+ "' recebido com sucesso (via WebService)",
                        };

                        ficheirosRepository.Insert(newFicheiro);

                        Entidade entidade = entidadesRepository.Find((int)notificacao.entidadeId);

                        entidade.notificacoes.Add(notificacao);
                        entidadesRepository.Save();
                        notificacoesRepository.Save();
                        ficheirosRepository.Save();


                        EnvioFicheiroResult resultado = new EnvioFicheiroResult { dataRecepcao = newFicheiro.dataUpload, nomeFicheiro = filePath, seguradora = entidade.nome };

                        resposta.Success = true;
                        resposta.Result = resultado;

                        return resposta;
                    }
                    catch (Exception e)
                    {
                        resposta.Success = false;
                        resposta.ErrorMessage += "Erro na recepção do Ficheiro";
                        resposta.Result = null;
                        
                        return resposta;
                    }
                }
            );
            return task;
        }


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
