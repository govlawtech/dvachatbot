using System;
using DVAESABot.Domain;

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