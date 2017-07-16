using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVAESABot.Domain;
using Microsoft.Bot.Builder.Dialogs;

namespace DVAESABot.ScheduledHeuristics
{
    public interface IHeuristicQnA<out TDialogResult>
    {
        IDialog<TDialogResult> Dialog { get; }
        bool IsRelevant(ChatContext chatContext);
    }


}
