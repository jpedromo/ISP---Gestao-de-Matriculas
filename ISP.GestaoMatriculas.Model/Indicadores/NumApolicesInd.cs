using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ISP.GestaoMatriculas.Model;

namespace ISP.GestaoMatriculas.Model.Indicadores
{
    public class NumApolicesInd : Indicador
    {
        public override Indicador calcular()
        {

            valor = entidade.apolices.Select(a => a.numeroApolice).Distinct().Count();

            return this;
        }
    }
}