using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

using ISP.GestaoMatriculas.Model;
using System.Collections;

namespace ISP.GestaoMatriculas.Contracts
{
    public interface IApoliceRepository : IDisposable
    {
        IQueryable<Apolice> All { get; }
        IQueryable<Apolice> AllIncluding(params Expression<Func<Apolice, object>>[] includeProperties);
        Apolice Find(int id);
        Apolice FindCache(int id);
        void InsertOrUpdate(Apolice apolice);
        void Delete(int id);
        void Save();

        /// <summary>
        /// Return a list.
        /// </summary>
        /// <param name="filters">A lambda expression used to filter the result.</param>
        /// <param name="sorting">The sort expression, example: ColumnName desc</param>
        /// <param name="includeList">The list of related entities to load.</param>
        /// <returns></returns>
        IList<Apolice> Search(Expression<Func<Apolice, bool>> filters, string sorting, List<string> includeList);
    }
}
