﻿using Microsoft.Practices.Unity;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using SpaUserControl.Startup;
using SpaUserControl.Api.Helpers;

namespace SpaUserControl.Api
{
    public class StartUp
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
            
            //Deixa o serviço publico sem nenhuma restrição de acesso entre dominios diferentes
            //Configurações iniciais  para que meu serviço rode
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseWebApi(config);
        }
        //Configuração obrigatória do WebAPI
        public static void ConfigureWebApi(HttpConfiguration config)
        {
            //Configura a rota padrão do WebAPI, esse mesmo código fica no arquivo WebApiConfig
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                    name: "DefaultApi",
                    routeTemplate: "api/{controller}/{id}",
                    defaults: new { id = RouteParameter.Optional }
                );
        }

    }
}