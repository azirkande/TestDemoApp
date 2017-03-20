using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace Folio1Demo.Web.Infrastructure
{
    public static class AutofacSetup
    {
        public static void Setup()
        {
            var builder = new ContainerBuilder();

            // Get your HttpConfiguration.
            var config = GlobalConfiguration.Configuration;

            // Register your Web API controllers.
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());//for web api
            builder.RegisterControllers(Assembly.GetExecutingAssembly());//for mvc

            builder.RegisterFilterProvider();
            builder.RegisterModule<CommonIocModule>();

            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);//for web api
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));//for mv
        }
    }
}