using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;

namespace DVAESABot.Dialogs
{
    public class YesNoDialog : IDialog<bool>
    {
        private readonly string _prompt;

        public YesNoDialog(string prompt)
        {
            _prompt = prompt;
        }

        public async Task StartAsync(IDialogContext context)
        {
           
            PromptDialog.Confirm(context,
                AnswerReceived,
                _prompt,
                "Yes or no.",
                99
                );
        }

        private async Task AnswerReceived(IDialogContext context, IAwaitable<bool> message)
        {
            var result = await message;
            context.Done(result);
        }
    }
}