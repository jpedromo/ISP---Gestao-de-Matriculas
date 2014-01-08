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
using System.Globalization;


namespace ISP.WebAPI.Controllers
{
    public class ConsultaPublicaController : ApiController
    {
        private IApoliceRepository apolicesRepository;
        private IApoliceIsentoRepository apolicesIsentosRepository;

        public ConsultaPublicaController(IApoliceRepository apolicesRepository, IApoliceIsentoRepository apolicesIsentosRepository)
        {
            this.apolicesRepository = apolicesRepository;
            this.apolicesIsentosRepository = apolicesIsentosRepository;
        }


        [HttpGet]
        [ActionName("PublicAction")]
        public ResultResponse<ConsultaPublicaResult> Get(string data, string hora, string matricula)
        {
            ResultResponse<ConsultaPublicaResult> resposta = new ResultResponse<ConsultaPublicaResult>();

            DateTime dataParsed;
            TimeSpan horaParsed;

            DateTime dataHoraPesquisa;

            //tem matrícula válida - check
            if (matricula == null || !Regex.Match(matricula, "[-\\/?#() A-Za-z0-9]+").Success)
            {
                resposta.Success = false;
                resposta.ErrorMessage += "Número de matrícula '" + matricula + "' inválido.\n";
            }
            //tem data inicio válida - check
           
            if (!DateTime.TryParseExact(data, "yyyyMMdd", new CultureInfo("pt-PT"), DateTimeStyles.None, out dataParsed))
            {
                resposta.Success = false;
                resposta.ErrorMessage += "Data de pesquisa '" + data + "' inválida";
            }

            if (!TimeSpan.TryParseExact(hora, "hhmmss", new CultureInfo("pt-PT"), TimeSpanStyles.None, out horaParsed))
            {
                resposta.Success = false;
                resposta.ErrorMessage += "Hora de pesquisa '" + hora + "' inválida";
            }

            if(resposta.Success == false)
            {
                return resposta;
            }

            dataHoraPesquisa = dataParsed.Add(horaParsed);

            List<Apolice> apolices = apolicesRepository.All.Include("entidade").Include("veiculo").Where(a => (a.veiculo.numeroMatricula == matricula || a.veiculo.numeroMatriculaCorrigido == matricula) && a.dataInicio <= dataHoraPesquisa && a.dataFim >= dataHoraPesquisa).ToList();
            List<ApoliceIsento> isentos = apolicesIsentosRepository.All.Where(i => (i.matricula == matricula || i.matriculaCorrigida == matricula || i.matriculaCorrigida == matricula.Replace("-", "")) && i.dataInicio <= dataHoraPesquisa && i.confidencial == false && i.arquivo == false).ToList();

            List<ConsultaPublicaItem> apolicesPublicas = new List<ConsultaPublicaItem>();

            foreach (Apolice a in apolices)
                {
                    apolicesPublicas.Add(new ConsultaPublicaItem
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
                        if (i.dataFim < dataHoraPesquisa)
                        {
                            continue;
                        }
                    }
                    ConsultaPublicaItem apoliceToView = new ConsultaPublicaItem
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

                resposta.Result = new ConsultaPublicaResult{ listagem = apolicesPublicas };


            return resposta;
        }


    }
}
