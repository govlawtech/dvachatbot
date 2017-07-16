using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using DVAESABot.Domain;
using Microsoft.Bot.Builder.Dialogs;

namespace DVAESABot.Dialogs
{
    [Serializable]
    public class SeekingTreatmentDialog : IDialog<bool>
    {
        public async Task StartAsync(IDialogContext context)
        {
            PromptDialog.Confirm(context, 
                (dialogContext, result) =>
                {
                    var cc = dialogContext.GetChatContextOrDefault();
                    cc.User.SeekingTreatmentOrRehab = result.GetAwaiter().GetResult();
                    dialogContext.SetChatContext(cc);
                    dialogContext.Done(result.GetAwaiter().GetResult());
                    return Task.CompletedTask;
                },
                "Are you seeking treatment or rehabilitation for an injury or illness?",
                "Yes or no?",
                3);
        }
    }
}