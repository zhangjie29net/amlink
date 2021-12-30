using System.Web.Http;
using WebActivatorEx;
using Esim7;
using Swashbuckle.Application;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace Esim7
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;
            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                {
                    c.SingleApiVersion("v1", "Esim7");
                    c.IncludeXmlComments(GetXmlCommentsPath());
                })
                .EnableSwaggerUi(c =>
                {
                       
                });
        }

        private static string GetXmlCommentsPath()
        {
            return string.Format("{0}/bin/Esim7.XML", System.AppDomain.CurrentDomain.BaseDirectory);
        }
    }
}
