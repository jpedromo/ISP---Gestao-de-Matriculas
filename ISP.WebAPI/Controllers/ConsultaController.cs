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
using ISP.WebAPI.Models;
using System.Text.RegularExpressions;
using System.Data.Entity;
using WebMatrix.WebData;
using ISP.WebAPI.Filters;
using System.Globalization;


namespace ISP.WebAPI.Controllers
{
    public class ConsultaController : ApiController
    {
        private IApoliceRepository apolicesRepository;
        private IUserProfileRepository usersRepository;

        public ConsultaController(IApoliceRepository apolicesRepository, IUserProfileRepository usersRepository)
        {
            this.apolicesRepository = apolicesRepository;
            this.usersRepository = usersRepository;
        }

        [BasicAuthorize]
        [HttpGet]
        [ActionName("ProtectedAction")]
        public ResultResponse<ConsultaPrivadaResult> Get(string dataInicio, string horaInicio, string dataFim, string horaFim, string matricula = null, string apolice = null,
            bool? avisos = null)
        {
            //WebSecurity.Login("admin1", "administrador");

            ResultResponse<ConsultaPrivadaResult> resposta = new ResultResponse<ConsultaPrivadaResult>();

            DateTime dataInicioParsed;
            TimeSpan horaInicioParsed;

            DateTime dataFimParsed;
            TimeSpan horaFimParsed;

            DateTime dataHoraInicio;
            DateTime dataHoraFim;

            //tem data inicio válida - check

            if (!DateTime.TryParseExact(dataInicio, "yyyyMMdd", new CultureInfo("pt-PT"), DateTimeStyles.None, out dataInicioParsed))
            {
                resposta.Success = false;
                resposta.ErrorMessage += "Data de início de pesquisa '" + dataInicio + "' inválida";
            }
            if (!DateTime.TryParseExact(dataFim, "yyyyMMdd", new CultureInfo("pt-PT"), DateTimeStyles.None, out dataFimParsed))
            {
                resposta.Success = false;
                resposta.ErrorMessage += "Data de fim de pesquisa '" + dataFim + "' inválida";
            }

            if (!TimeSpan.TryParseExact(horaInicio, "hhmmss", new CultureInfo("pt-PT"), TimeSpanStyles.None, out horaInicioParsed))
            {
                resposta.Success = false;
                resposta.ErrorMessage += "Hora de inicio de pesquisa '" + horaInicio + "' inválida";
            }
            if (!TimeSpan.TryParseExact(horaFim, "hhmmss", new CultureInfo("pt-PT"), TimeSpanStyles.None, out horaFimParsed))
            {
                resposta.Success = false;
                resposta.ErrorMessage += "Hora de fim de pesquisa '" + horaFim + "' inválida";
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
                if(Request.Headers.TryGetValues("Authorization", out headerVals))
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
                    else{
                        if (authHeaderTokens.Length > 1)
                            username = BasicAuthorizeAttribute.DecodeFrom64(authHeaderTokens[1]);
                        }
                }

                user = usersRepository.All.Include("entidade").Single(u => u.UserName == username);
            }

            if(user == null){
                resposta.Success = false;
                resposta.ErrorMessage += "Erro de autenticação.";
            }

            if(resposta.Success == false)
            {
                return resposta;
            }

            //try
            //{
            //    user = usersRepository.All.Include("entidade").Single(u => u.UserName == "admin1");
            //}

            dataHoraInicio = dataInicioParsed.Add(horaInicioParsed);
            dataHoraFim = dataFimParsed.Add(horaFimParsed);

            Entidade ent = user.entidade;
            IQueryable<Apolice> queryApolices;

            if (ent.scope.valor == "GLOBAL")
            {
                queryApolices = apolicesRepository.All.Include("veiculo").Include("tomador").Include("concelho").Include("entidade");
            }
            else
            {
                //Seguradoras (Acesso Local)
                queryApolices = apolicesRepository.All.Include("veiculo").Include("tomador").Include("concelho").Include("entidade").Where(a => a.entidadeId == ent.entidadeId);
            }

            queryApolices = queryApolices.Where(a =>((dataHoraInicio >= a.dataInicio && dataHoraInicio <= a.dataFim) ||
                (dataHoraFim >= a.dataInicio && dataHoraFim <= a.dataFim) ||
                (dataHoraInicio <= a.dataInicio && dataHoraFim >= a.dataFim)));           

            if (!(apolice == null || apolice == ""))
            {
                queryApolices = queryApolices.Where(a => a.numeroApolice.Contains(apolice));      
            }
            if (!(matricula == null || matricula == ""))
            {
                queryApolices = queryApolices.Where(a => a.veiculo.numeroMatricula.Contains(matricula));
            }
            if (avisos != null && avisos == true)
            {
                queryApolices = queryApolices.Where(a => a.avisos == true);
            }

            queryApolices = queryApolices.OrderByDescending(a => a.dataReporte);

            List<ConsultaPrivadaItem> apolicesView = new List<ConsultaPrivadaItem>();

            foreach (Apolice a in queryApolices.ToList())
                {
                    apolicesView.Add(new ConsultaPrivadaItem
                    {
                        numeroApolice = a.numeroApolice,
                        dataInicio = a.dataInicio,
                        dataFim = a.dataFim,
                        numeroMatricula = a.veiculo.numeroMatricula,
                        categoriaVeiculo = a.veiculo.categoria.codigoCategoriaVeiculo,
                        marcaVeiculo = a.veiculo.marcaVeiculo,
                        modeloVeiculo = a.veiculo.modeloVeiculo,
                        nomeTomador = a.tomador.nome,
                        identificacaoTomador = a.tomador.numeroIdentificacao,
                        moradaTomador = a.tomador.morada,
                        codigoPostalTomador = a.tomador.codigoPostal,

                        seguradora = a.entidade.nome
                    });
                }

            resposta.Result = new ConsultaPrivadaResult { listagem = apolicesView };

            return resposta;
        }

    }
}
