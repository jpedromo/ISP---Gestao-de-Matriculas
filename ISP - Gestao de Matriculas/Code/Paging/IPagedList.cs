using System.Collections.Generic;

namespace ISP.GestaoMatriculas.Code.Paging
{
    public interface IPagedList<T>
    {
        int PageSize { get; }
        int TotalNumberOfItems { get; }
        IList<T> CurrentPage { get; }
        int CurrentPageNumber { get; }
        int TotalNumberOfPages { get; }
    }
}