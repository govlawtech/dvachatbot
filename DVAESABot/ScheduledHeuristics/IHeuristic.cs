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
}