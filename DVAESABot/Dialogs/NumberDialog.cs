using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web;
using Chronic.Tags.Repeaters;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace DVAESABot.Dialogs
{
    public class NumberDialog : IDialog<int>
    {
        private readonly string _promptMessage;

        public NumberDialog(string promptMessage)
        {
            _promptMessage = promptMessage;
        }

        public async Task StartAsync(IDialogContext context)
        {
            await context.SayAsync(_promptMessage);
            context.Wait(NumberReceived);
        }

        private async Task NumberReceived(IDialogContext context, IAwaitable<IMessageActivity> activity)
        {
            var text = (await activity).Text;
            int number;
            if (Int32.TryParse(text, out number))
            {
                context.Done(number);
            }
            else
            {
                await context.SayAsync("Try a number, like '68'.");
                context.Wait(NumberReceived);
            }
        }

    }
}