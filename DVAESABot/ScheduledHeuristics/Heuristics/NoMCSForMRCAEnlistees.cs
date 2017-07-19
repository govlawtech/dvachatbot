using System;
using DVAESABot.Domain;
using NodaTime;

namespace DVAESABot.ScheduledHeuristics.Heuristics
{
    public class NoMCSForMRCAEnlistees : IScheduledHeuristic
    {
        public string Description => "Military Compensation Scheme not relevant to pre 1 July 2004 enlistees";

        public Predicate<ChatContext> Condition => c => c.User.UserType == UserType.Member &&
                                                        c.User.EnlistmentDate.HasValue &&
                                                        c.User.EnlistmentDate.Value
                                                            .CompareTo(new LocalDate(2004, 7, 1)) >= 1;

        public Action<ChatContext> Action => c => c.FactsheetShortlist = c.FactsheetShortlist.RemoveCategories("MCS");
    }
}