using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using System.IO;
using System.Web;
using System.Security.Principal;
using ISP.WebAPI.Filters;
using ISP.GestaoMatriculas.Contracts;
using ISP.GestaoMatriculas.Model;

namespace ISP.WebAPI.Controllers
{
    public class ADLoginController : ApiController
    {
        private IUserProfileRepository usersRepository;

        public ADLoginController(IUserProfileRepository usersRepository)
        {
            this.usersRepository = usersRepository;
        }

        //
        // GET: /ADLogin/

        [Authorize]
        [HttpGet]
        [ActionName("AuthAction")]
        public HttpResponseMessage Get()
        {
            HttpResponseMessage response;
            List<UserProfile> users = usersRepository.All.Where(u => u.utilizadorAD == User.Identity.Name && u.ativo == true).ToList();
           
            UserProfile user = null;

            if(users.Count > 0){
                user = users.First();
            }

            if (user == null)
            {
                response = new HttpResponseMessage(HttpStatusCode.Forbidden);
            }
            else
            {
                response = new HttpResponseMessage(HttpStatusCode.OK);
            }
            
            return response;
        }

    }
}
