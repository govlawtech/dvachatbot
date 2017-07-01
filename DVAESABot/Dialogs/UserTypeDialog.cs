using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using DVAESABot.Domain;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace DVAESABot.Dialogs
{
    public class UserTypeDialog : IDialog<UserType>
    {
        public Task StartAsync(IDialogContext context)
        {
            var activity = context.MakeMessage();
            activity.Text = "Which best describes you?";
            activity.Type = ActivityTypes.Message;
            activity.TextFormat = TextFormatTypes.Plain;
            throw new NotImplementedException();



            
        }
    }
}