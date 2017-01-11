using SpaUserControl.Api.Models;
using SpaUserControl.Domain.Models;
using SpaUserControl.Domain.Models.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SpaUserControl.Api.Controllers
{
    public class AccountController : ApiController
    {

        //Injetando o Service para usar o métodos de Conta do Serviço
        private IUserService _service;
        public AccountController(IUserService service)
        {
            _service = service;
        }

        //Vou registrar o usuário e retorna-lo, ao invés de cadastrar e depois dar um select.
        //Rota, api/Account/

        [HttpPost]
        public RegisterUserModel Register(RegisterUserModel model)
        {
            try
            {
                 _service.Register(model.Name, model.Email, model.Password, model.ConfirmPassword);
                 model.Password = "";
                 model.ConfirmPassword = "";
                 return model;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        //Garante que não vai ficar nada aberto do controller quando não estiver mais usando
        protected override void Dispose(bool disposing)
        {
            _service.Dispose();
        }
    }
}
