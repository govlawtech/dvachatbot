using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Chronic;
using DVAESABot.Domain;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using NodaTime;
using NodaTime.Text;

namespace DVAESABot.Dialogs
{
    [Serializable]
    public class EnlistmentDateDialog : IDialog<LocalDate?>
    {
        private int attempts = 0;
     
        public async Task StartAsync(IDialogContext context)
        {
            if (context.GetChatContextOrDefault().User.EnlistmentDate.HasValue)
            {
                context.Done(context.GetChatContextOrDefault().User.EnlistmentDate);
            }
            else
            {
                await context.SayAsync("What date did you enlist?");
                context.Wait(DateResponseRecevied);
            }
        }

        private async Task DateResponseRecevied(IDialogContext context, IAwaitable<IMessageActivity> message)
        {
            if (attempts > 3)
            {
                await context.SayAsync("...skipping this question...");
                LocalDate? nullDate = null;
                context.Done(nullDate);
            }

            var text = await message;
            var parserOptions = new Options()
            {
                Context = Pointer.Type.Past,
            };
            var parser = new Chronic.Parser(parserOptions);
            
            var span = parser.Parse(text.Text);
            if (span != null && span.Start.HasValue)
            {
                var dt = span.Start.Value;
                var d = new LocalDate(dt.Year, dt.Month, dt.Day);

                var ldp = LocalDatePattern.CreateWithInvariantCulture("D");
                await context.SayAsync($"Understood: {ldp.Format(d)}, Canberra/Australia time.");

                var cc = context.GetChatContextOrDefault();
                cc.User.EnlistmentDate = d;
                context.SetChatContext(cc);
                context.Done(d);
            }
            else
            {
                attempts++;
                await context.SayAsync("For example: 12 Jan 1985.");
                context.Wait(DateResponseRecevied);
            }
        }
    }
}