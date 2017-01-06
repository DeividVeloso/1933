using Microsoft.Owin.Security.OAuth;
using SpaUserControl.Common.Resources;
using SpaUserControl.Domain.Models.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace SpaUserControl.Api.Security
{
    public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        //Ele tem o método de autenticação(email, senha), por isso eu uso ele aqui no AuthorizationServerProvider
        private readonly IUserService _service;

        //Passo ele no construtor com DI e pelo classe StartUp.cs -  Provider = new AuthorizationServerProvider(service)
        public AuthorizationServerProvider(IUserService service)
        {
            _service = service;
        }

        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            //Valida um token já existente
            context.Validated();
        }

        //Cria um token através dos dados do método service.Authenticate
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            //Habilito o Cors
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            try
            {
                //Verifica usuário e senha
                var user = _service.Authenticate(context.UserName, context.Password);

                //Se for invalido retornar mensagem de erro
                if (user == null)
                {
                    context.SetError("invalid_grant", Errors.InvalidCredentials);
                    return;
                }

                //Começo a criar o ClaimsIdentity
                var identity = new ClaimsIdentity(context.Options.AuthenticationType);

                //O dado mais importante para identificar um usuário você coloca no ClaimTypes.Name
                identity.AddClaim(new Claim(ClaimTypes.Name, user.Email));
                identity.AddClaim(new Claim(ClaimTypes.GivenName, user.Name));

                //Gera a autenticação para Thread atual, senão colocar ele não autentica
                GenericPrincipal principal = new GenericPrincipal(identity, null);
                Thread.CurrentPrincipal = principal;

                context.Validated(identity);
            }
            catch (Exception ex)
            {
                context.SetError("invalid_grant", Errors.InvalidCredentials);
            }
        }
    }
}