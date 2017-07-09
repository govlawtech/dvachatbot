using System.Collections.Generic;
using DVAESABot.Domain;
using Microsoft.Bot.Builder.Dialogs;

namespace DVAESABot.ScheduledHeuristics
{
    interface IHaveDialogs
    {
        IList<IHeuristicQnA> HeuristicQnAs { get; }
    }
}