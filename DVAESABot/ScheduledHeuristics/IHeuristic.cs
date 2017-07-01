using System;
using DVAESABot.Domain;
using DVAESABot.Utilities;
using Microsoft.Bot.Builder.Dialogs;

namespace DVAESABot.ScheduledHeuristics
{
    interface IScheduledHeuristic
    {
        string Description { get; }
        int Salience { get; }
        Predicate<ChatContext> Condition { get; }
        Action<ChatContext> Action { get; }
    }

    interface IHaveDialog<in TDialogResult>
    {
        IHaveDialog<TDialogResult> GetDialog();
        
        void ApplyResult(TDialogResult result, ChatContext chatContext);
    }


}