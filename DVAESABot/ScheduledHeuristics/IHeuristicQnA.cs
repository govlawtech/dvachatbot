using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVAESABot.Domain;
using Microsoft.Bot.Builder.Dialogs;

namespace DVAESABot.ScheduledHeuristics
{
    public interface IHeuristicQnA
    {
        IDialog Dialog { get; }
        void ApplyResult(ChatContext chatContext, object dialogResult);
        bool IsAlreadyAnswered(ChatContext chatContext);
    }
}
