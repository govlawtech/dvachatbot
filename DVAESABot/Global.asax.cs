using System.Configuration;
using System.Web;
using System.Web.Http;
using Autofac;
using DVAESABot.Utilities;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Internals.Fibers;

namespace DVAESABot
{
    public class WebApiApplication : HttpApplication
    {
        public object Dialog_Manager { get; private set; }

        protected void Application_Start()
        {
            RegisterBotModules();

            GlobalConfiguration.Configure(WebApiConfig.Register);

            var builder = new ContainerBuilder();
            builder.RegisterType<AppInsightsActivityLogger>()
                .AsImplementedInterfaces()
                .InstancePerDependency();
            builder.Update(Conversation.Container);

            TelemetryConfiguration.Active.InstrumentationKey = ConfigurationManager.AppSettings["AppInsightsKey"];
        }

        private void RegisterBotModules()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule(new ReflectionSurrogateModule());

            builder.RegisterModule<GlobalMessageHandlersBotModule>();

            builder.Update(Conversation.Container);
        }
    }
}