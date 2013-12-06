using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISP.WebAPI.Models
{
    public class ResultResponse<T>
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }

        public T Result { get; set; }
    }
}