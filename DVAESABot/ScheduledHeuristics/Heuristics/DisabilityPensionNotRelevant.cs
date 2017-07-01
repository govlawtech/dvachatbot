using System;
using DVAESABot.Domain;
using NodaTime;

namespace DVAESABot.ScheduledHeuristics.Heauristics
{
    class DisabilityPensionNotRelevant : IScheduledHeuristic
    {
        public string Description => "Member enlisted before 1 July 2004, therefore disability pension not relevant";
        public int Salience => 60;

        public Predicate<ChatContext> Condition => c =>
        {
            if (c.User.UserType.MatchSome(out UserType userType))
            {
                if (userType == UserType.Member)
                {
                    if (c.User.EnlistmentDate.MatchSome(out LocalDate enlistDate))
                    {
                        if (enlistDate.CompareTo(new LocalDate(2004, 7, 1)) >= 0)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        };

        public Action<ChatContext> Action => c => c.FactsheetShortlist.RemoveCategories("DP");
    }
}