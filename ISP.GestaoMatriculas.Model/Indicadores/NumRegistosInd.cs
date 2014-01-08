using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ISP.GestaoMatriculas.Model;

namespace ISP.GestaoMatriculas.Model.Indicadores
{
    public class NumRegistosInd : Indicador
    {
        public override Indicador calcular()
        {
            valor = entidade.apolices.Count;

            return this;
        }
    }
}