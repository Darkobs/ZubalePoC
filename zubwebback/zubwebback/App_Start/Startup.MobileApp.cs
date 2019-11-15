using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Web.Http;
using Microsoft.Azure.Mobile.Server;
using Microsoft.Azure.Mobile.Server.Authentication;
using Microsoft.Azure.Mobile.Server.Config;
using zubwebback.DataObjects;
using zubwebback.Models;
using Owin;

namespace zubwebback
{
    public partial class Startup
	{
        public static void ConfigureMobileApp(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();

            //For more information on Web API tracing, see http://go.microsoft.com/fwlink/?LinkId=620686 
            config.EnableSystemDiagnosticsTracing();

            new MobileAppConfiguration()
                .UseDefaultConfiguration()
                .ApplyTo(config);

            // Use Entity Framework Code First to create database tables based on your DbContext
            Database.SetInitializer(new zubwebInitializer());

            // To prevent Entity Framework from modifying your database schema, use a null database initializer
            // Database.SetInitializer<ZUMOAPPNAMEContext>(null);

            MobileAppSettingsDictionary settings = config.GetMobileAppSettingsProvider().GetMobileAppSettings();

            if (string.IsNullOrEmpty(settings.HostName))
            {
                // This middleware is intended to be used locally for debugging. By default, HostName will
                // only have a value when running in an App Service application.
                app.UseAppServiceAuthentication(new AppServiceAuthenticationOptions
                {
                    SigningKey = ConfigurationManager.AppSettings["SigningKey"],
                    ValidAudiences = new[] { ConfigurationManager.AppSettings["ValidAudience"] },
                    ValidIssuers = new[] { ConfigurationManager.AppSettings["ValidIssuer"] },
                    TokenHandler = config.GetAppServiceTokenHandler()
                });
            }
            app.UseWebApi(config);
        }
    }

    public class zubwebInitializer : CreateDatabaseIfNotExists<zubwebContext>
    {
        protected override void Seed(zubwebContext context)
        {
            List<ZubItem> zubItems = new List<ZubItem>
            {
                new ZubItem { Id = Guid.NewGuid().ToString(), Nombre = "Carlos", Correo = "carlos@correo.com", Empresa = "05_Mascota", Direccion = "Coacalco", Cuestionario = "012B", Pregunta_1 = true, Pregunta_2 = "Empleados limpios", Pregunta_3 = "Muchos accesorios" },
                new ZubItem { Id = Guid.NewGuid().ToString(), Nombre = "Tania", Correo = "tania@correo.com", Empresa = "05_Mascota", Direccion = "Coacalco", Cuestionario = "012B", Pregunta_1 = false, Pregunta_2 = "No hay Empleados", Pregunta_3 = "Pocos accesorios" },
            };

            foreach (ZubItem zubItem in zubItems)
            {
                context.Set<ZubItem>().Add(zubItem);
            }

            base.Seed(context);
        }
    }
}