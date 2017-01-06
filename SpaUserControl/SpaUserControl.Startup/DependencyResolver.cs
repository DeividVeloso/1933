using Microsoft.Practices.Unity;
using SpaUserControl.Business.Services;
using SpaUserControl.Domain.Models;
using SpaUserControl.Domain.Models.Contracts.Repositories;
using SpaUserControl.Domain.Models.Contracts.Services;
using SpaUserControl.Infraestructure.Data;
using SpaUserControl.Infraestructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaUserControl.Startup
{
    public static class DependencyResolver
    {
        public static void Resolve(UnityContainer container) 
        {
            container.RegisterType<AppDataContext, AppDataContext>(new HierarchicalLifetimeManager());
            container.RegisterType<IUserRepository, UserRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IUserService, UserService>(new HierarchicalLifetimeManager());
        }
    }
}
