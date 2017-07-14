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
        public async Task StartAsync(IDialogContext context)
        {

            List<UserType> choices = Enum.GetValues(typeof(UserType)).Cast<UserType>().ToList();
            PromptDialog.Choice(
                context,
                ResponseReceived,
                choices,
                "Which best describes you?",
                "Try again",
                3
            );
        }

        private async Task ResponseReceived(IDialogContext context, IAwaitable<UserType> activity)
        {
            var userType = await activity;
            context.Done(userType);
        }
    }
}