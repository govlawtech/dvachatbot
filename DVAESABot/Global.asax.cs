using Autofac;
using DVAESABot.Utilities;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.History;
using System.Configuration;
using System.Web.Http;

namespace DVAESABot
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        public object Dialog_Manager { get; private set; }

        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            var builder = new ContainerBuilder();
            builder.RegisterType<AppInsightsActivityLogger>().AsImplementedInterfaces().InstancePerDependency();
            builder.Update(Conversation.Container);

            TelemetryConfiguration.Active.InstrumentationKey = ConfigurationManager.AppSettings["AppInsightsKey"];
        }
    }
}
