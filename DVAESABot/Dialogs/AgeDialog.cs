using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using DVAESABot.Domain;
using Microsoft.Bot.Builder.Dialogs;

namespace DVAESABot.Dialogs
{
    public class AgeDialog : IDialog<int?>
    {
        public async Task StartAsync(IDialogContext context)
        {
            if (context.GetChatContextOrDefault().User.Age.HasValue)
                context.Done(context.GetChatContextOrDefault().User.Age);
            else
            {
                try
                {

                    PromptDialog.Number(
                        context: context,
                        resume: Resume,
                        prompt: "What is your age in years?",
                        retry: "For example: '68'",
                        min: 16,
                        max: 120);
                }
                catch (TooManyAttemptsException e)
                {
                    await context.SayAsync("...skipping that question...");
                    int? nullint = null;
                    context.Done(nullint);
                }
            }
        }

        private async Task Resume(IDialogContext context, IAwaitable<double> result)
        {
            var cc = context.GetChatContextOrDefault();
            cc.User.Age = (int)result.GetAwaiter().GetResult(); 
            context.SetChatContext(cc);
            context.Done((int)result.GetAwaiter().GetResult());
        }
    }
}