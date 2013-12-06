using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Data;
using System.Data.Entity;
using System.Web;

using ISP.GestaoMatriculas.Contracts;
using ISP.GestaoMatriculas.Model;

namespace ISP.GestaoMatriculas.Repositories
{
    public class ApoliceHistoricoRepository : Respository<DomainModels, ApoliceHistorico>, IApoliceHistoricoRepository
    {
        public ApoliceHistoricoRepository():base()
        {
            DbSet = Context.ApolicesHistorico;
        }
    }
}
