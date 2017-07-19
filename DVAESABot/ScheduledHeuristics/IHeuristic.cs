using System;
using DVAESABot.Domain;
using DVAESABot.Utilities;
using Microsoft.Bot.Builder.Dialogs;

namespace DVAESABot.ScheduledHeuristics
{
    interface IScheduledHeuristic
    {
        string Description { get; }
        Predicate<ChatContext> Condition { get; }
        Action<ChatContext> Action { get; }
    }
}