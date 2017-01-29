using Microsoft.Practices.Unity;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using SpaUserControl.Startup;
using SpaUserControl.Api.Helpers;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Owin;
using SpaUserControl.Domain.Models.Contracts.Services;
using SpaUserControl.Api.Security;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SpaUserControl.Api
{
    public class Startup
    {
        //Configuração obrigatoria
        //Recebe um IAppBuilder via injeção de dependencia que está dentro do using Owin;
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();

            //chamando a dependencia do Projeto Spa.StartUp
            var container = new UnityContainer();
            DependencyResolver.Resolve(container);
            config.DependencyResolver = new UnityResolver(container);

            //Chamando a minha configuração do WEBAPI
            ConfigureWebApi(config);

            //Chamando o método de autenticação do OAth
            ConfigureOAuth(app, container.Resolve<IUserService>());
            
            //Deixa o serviço publico sem nenhuma restrição de acesso entre dominios diferentes
            //Configurações iniciais  para que meu serviço rode
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseWebApi(config);
        }
        //Configuração obrigatória do WebAPI
        public static void ConfigureWebApi(HttpConfiguration config)
        {
            //Se eu estou criando serviço REST, não vou usar XML nele
            //Remove o XML
            var formatters = config.Formatters;
            formatters.Remove(formatters.XmlFormatter);


            //Modifica a identação
            //Deixa no padrão JavaScript toda variavel começa com minusculo
            var jsonSettings = formatters.JsonFormatter.SerializerSettings;
            jsonSettings.Formatting = Formatting.Indented;
            jsonSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            //Modifica a serialização
            //Para quem trabalha com eagerloading eu escolho os obejtos que quero iniciar
            //Server para seriealizar objeto dentro de objeto
            formatters.JsonFormatter.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects;

            //Configura a rota padrão do WebAPI, esse mesmo código fica no arquivo WebApiConfig
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                    name: "DefaultApi",
                    routeTemplate: "api/{controller}/{id}",
                    defaults: new { id = RouteParameter.Optional }
                );
        }

        public void ConfigureOAuth(IAppBuilder app, IUserService service)
        {
            //Opções de autorização no meu servidor
            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                //Permite chamadas inseguras Http
                AllowInsecureHttp = true,
                //Endpoint que gera um token através de um usuário e senha
                TokenEndpointPath = new PathString("/api/security/token"),
                //Esse token terar duração de 2 horas
                AccessTokenExpireTimeSpan = TimeSpan.FromHours(2),
                //Gera um token baseado no retorno da classe AuthorizationServerProvider
                Provider = new AuthorizationServerProvider(service)
            };
            // Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }


    }
}