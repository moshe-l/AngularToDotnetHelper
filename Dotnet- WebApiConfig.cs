******************
Install-Package Microsoft.AspNet.WebApi.Cors
******************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.WebHost;


namespace Trustech_EasyWebApi
{
    public static class WebApiConfig
    {
		public static void Register(HttpConfiguration config)
        {
			// Web API configuration and services
			config.EnableCors();
		
			// Web API routes
			config.MapHttpAttributeRoutes();
			//config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

			config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "{controller}/{action}",
                defaults: new { id = RouteParameter.Optional }
            );

			 
		}
    }
}
