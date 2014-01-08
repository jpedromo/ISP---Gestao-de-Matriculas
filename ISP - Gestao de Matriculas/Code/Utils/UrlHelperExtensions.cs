using System.ComponentModel;
using System.Web.Mvc;
using System.Web.Routing;
using ISP.GestaoMatriculas.BaseListViewModel;

namespace ISP.GestaoMatriculas.Code.Util
{
    public static class UrlHelperExtentions
    {
        public static RouteValueDictionary GetRouteValueDictionaryForList(this UrlHelper urlHelper, IListViewModel model, string sortColumn = "", string direction = "")
        {
            var dictionary = new RouteValueDictionary
                                 {
                                     {"tabNr", model.TabNumber},
                                     {"page", model.CurrentPageNumber},
                                     {"sort", string.IsNullOrEmpty(sortColumn) ? model.SortColumn : sortColumn},
                                     {"direction", string.IsNullOrEmpty(direction) ? (model.SortDirection == ListSortDirection.Ascending ? "asc" : "desc") : direction}                                    
                                 };
            foreach (var filter in model.GetFilters())
            {
                if (!dictionary.ContainsKey(filter.UrlFilterName))
                {
                    dictionary.Add(filter.UrlFilterName, filter.Value);
                }
            }

            return dictionary;
        }
    }
}