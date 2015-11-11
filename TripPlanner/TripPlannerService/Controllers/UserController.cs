using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.WebPages;
using TripPlannerService.App_Start;
using TripPlannerService.Models;

namespace TripPlannerService.Controllers
{
    public class UserController : ApiController
    {
        private UserContext context;

        public class UserCredentials
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public UserController()
        {
            context = new UserContext();
        }

        /*[HttpPost]
        [Route("user/token")]
        public string CreateToken(UserCredentials credentials)
        {
            if (credentials?.Email?.IsEmpty() != true || credentials.Password?.IsEmpty() != true)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            if (context.Users.Any(u => u.Email == credentials.Email && u.Hash == CreateHash(credentials.Password, u.Salt)))
            {
                return CreateHash(credentials.Email);
            }

            throw new HttpResponseException(HttpStatusCode.Unauthorized);
        }*/

        public void CreateAccount(UserCredentials credentials)
        {
            if (credentials?.Email?.IsEmpty() != true || credentials.Password?.IsEmpty() != true)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            
            
            throw new HttpResponseException(HttpStatusCode.Created);
        }

       /* private readonly SHA512Managed hashAlgorithm = new SHA512Managed();
        private const string secret = "lUTA9kqk2D2ZYAXrB9eips8Qc69eHV3rSBZdY7qe";
        private string CreateHash(string content)
        {
            return CreateHash(content, secret);
        }

        private string CreateHash(string content, string salt)
        {
            return Encoding.UTF8.GetString(hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(content + salt)));
        }*/
    }
}
