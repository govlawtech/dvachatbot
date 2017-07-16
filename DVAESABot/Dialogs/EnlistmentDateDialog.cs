using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using DVAESABot.Domain;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using NodaTime;

namespace DVAESABot.Dialogs
{
    [Serializable]
    public class EnlistmentDateDialog : IDialog<LocalDate>
    {
        private int attempts = 0;
     
        public async Task StartAsync(IDialogContext context)
        {
            await context.SayAsync("What date did you enlist?");
            context.Wait(DateResponseRecevied);
        }

        private async Task DateResponseRecevied(IDialogContext context, IAwaitable<IMessageActivity> message)
        {
            if (attempts > 3)
            {
                throw new TooManyAttemptsException("Too many attempts to specify a date.");
            }

            var text = await message;
            var parser = new Chronic.Parser();
            var span = parser.Parse(text.Text);
            var dt = span.Start;
            if (!dt.HasValue)
            {
                attempts++;
                await context.SayAsync("For example: 12 Jan 1985.");
                context.Wait(DateResponseRecevied);
            }
            var d = new LocalDate(dt.Value.Year, dt.Value.Month, dt.Value.Day);


            var cc = context.GetChatContextOrDefault();
            cc.User.EnlistmentDate = d;
            context.SetChatContext(cc);
            context.Done(d);
        }
    }
}