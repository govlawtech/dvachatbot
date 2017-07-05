using System;
using DVAESABot.Domain;
using DVAESABot.Utilities;
using Microsoft.Bot.Builder.Dialogs;
using NodaTime;

namespace DVAESABot.ScheduledHeuristics.Heuristics
{
    class DisabilityPensionNotRelevant : IScheduledHeuristic, IHaveDialog<LocalDate>
    {
        public string Description => "Member enlisted before 1 July 2004, therefore disability pension not relevant";
        public int Salience => 60;

        public Predicate<ChatContext> Condition => c =>
        {
            if (c.User.UserType == UserType.Member)
            {
                if (c.User.EnlistmentDate?.CompareTo(new LocalDate(2004, 7, 1)) >= 0)
                {
                    return true;
                }
            }
            return false;
        };

        public Action<ChatContext> Action => c => c.FactsheetShortlist = c.FactsheetShortlist.RemoveCategories("DP");
        public IDialog<LocalDate> GetDialog()
        {
            throw new NotImplementedException();
        }

        public void ApplyResult(LocalDate result, ChatContext chatContext)
        {
            chatContext.User.EnlistmentDate = result;
        }

        
    }
}