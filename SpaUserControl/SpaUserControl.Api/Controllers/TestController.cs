using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SpaUserControl.Api.Controllers
{
    public class TestController : ApiController
    {
        [Authorize]
        public string Get() 
        { 
            return "Teste";
        }
    }
}
