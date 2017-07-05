using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Chronic;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using NodaTime;

namespace DVAESABot.Dialogs
{
    public class GetDateDialog : IDialog<LocalDate>
    {
        private readonly string _promptMessage;

        public GetDateDialog(string promptMessage)
        {
            _promptMessage = promptMessage;
        }

        public async Task StartAsync(IDialogContext context)
        {
            await context.SayAsync(_promptMessage); 
            context.Wait(DateResponseRecevied);
        }

        private async Task DateResponseRecevied(IDialogContext context, IAwaitable<IMessageActivity> message)
        {
            var text = await message;
            var parser = new Chronic.Parser();
            var span = parser.Parse(text.Text);
            var dt = span.Start;
            if (!dt.HasValue)
            {
                await context.SayAsync("For example: 12 Jan 1985.");
                context.Wait(DateResponseRecevied);
            }
            context.Done(new LocalDate(dt.Value.Year, dt.Value.Month, dt.Value.Day));
        }

    }
}