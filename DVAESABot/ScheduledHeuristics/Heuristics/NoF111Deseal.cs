using System;
using DVAESABot.Domain;
using NodaTime;

namespace DVAESABot.ScheduledHeuristics.Heuristics
{
    internal class NoF111Deseal : IScheduledHeuristic
    {
        public string Description => "Remove F111 deseal factsheets because member and enlisted after deseal.";
        public int Salience => 51;

        public Predicate<ChatContext> Condition => c =>
        {
            if (c.User.UserType == UserType.Member)
                if (c.User.EnlistmentDate?.CompareTo(new LocalDate(2001, 1, 1)) >= 0)
                    return true;
            return false;
        };

        public Action<ChatContext> Action => c => c.FactsheetShortlist = c.FactsheetShortlist.RemoveCategories("F111");
    }
}