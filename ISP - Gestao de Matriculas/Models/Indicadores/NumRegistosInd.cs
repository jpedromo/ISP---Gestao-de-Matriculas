using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISP.GestaoMatriculas.Models.Indicadores
{
    public class NumRegistosInd : Indicador
    {
        public override Indicador calcular(DomainModels db)
        {
            valor = entidade.apolices.Count;

            return this;
        }
    }
}