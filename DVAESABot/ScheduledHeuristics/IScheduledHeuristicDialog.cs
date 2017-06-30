using DVAESABot.Domain;
using Microsoft.Bot.Builder.Dialogs;

namespace DVAESABot.ScheduledHeuristics
{
    interface IScheduledHeuristicDialog<TDialogResult>
    {
        IScheduledHeuristic ScheduledHeuristic { get; set; }
        void ApplyResult(TDialogResult result, ChatContext chatContext);
        IDialog<TDialogResult> GetDialog();
    }
}