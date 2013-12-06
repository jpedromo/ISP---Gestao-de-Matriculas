using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISP.GestaoMatriculas.Model;
using ISP.GestaoMatriculas.Repositories;
using ISP.GestaoMatriculas.Contracts;
using ISP.GestaoMatriculas.Model.Exceptions;

namespace ISP.GestaoMatriculas.FileAgent
{
    public class FNPASFileAgentUtils
    {
        public FNPASFileAgentUtils()
        {

        }

        public static void FNPASFileAgentExecute(string[] args)
        {
            try
            {
                FicheiroRepository ficheiroRepository = new FicheiroRepository();
                DomainModels context = new DomainModels();
                var x = context.Ficheiros;
                IQueryable<Ficheiro> query = null;

                query = ficheiroRepository.All;
                query = ficheiroRepository.All.Where(f => f.estado == Ficheiro.EstadoFicheiro.pendente);
                int d = 3;
                foreach (Ficheiro file in query)
                {
                    file.estado = Ficheiro.EstadoFicheiro.emProcessamento;

                    List<ErroFicheiro> errosFicheiro = new List<ErroFicheiro>();
                    try
                    {
                        file.validar();
                    }
                    catch (ErroFicheiroException ex)
                    {
                        file.errosFicheiro = ex.errosFicheiro;
                        file.estado = Ficheiro.EstadoFicheiro.erro;
                        continue;
                    }

                    file.carregaEventos();

                }
            }
            catch (Exception ex){ }
        }


    }
}
