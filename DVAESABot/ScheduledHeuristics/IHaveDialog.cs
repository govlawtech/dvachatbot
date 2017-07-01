using DVAESABot.Domain;
using Microsoft.Bot.Builder.Dialogs;

namespace DVAESABot.ScheduledHeuristics
{
    interface IHaveDialog<TDialogResult>
    {
        IDialog<TDialogResult> GetDialog();
        
        void ApplyResult(TDialogResult result, ChatContext chatContext);
    }
}