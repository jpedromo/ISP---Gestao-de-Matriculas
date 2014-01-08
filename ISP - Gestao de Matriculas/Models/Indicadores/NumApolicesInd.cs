using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISP.GestaoMatriculas.Models.Indicadores
{
    public class NumApolicesInd : Indicador
    {
        public override Indicador calcular(DomainModels db)
        {

            valor = entidade.apolices.Select(a => a.numApolice).Distinct().Count();

            return this;
        }
    }
}