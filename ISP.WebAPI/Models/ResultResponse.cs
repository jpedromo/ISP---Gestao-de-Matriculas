using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISP.WebAPI.Models
{
    public class ResultResponse<T>
    {
<<<<<<< HEAD
        public ResultResponse()  
        {
            this.Success = true;
            this.ErrorMessage = string.Empty;
        }

=======
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }

        public T Result { get; set; }
    }
}