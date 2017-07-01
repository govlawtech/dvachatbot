using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DVAESABot.Domain;
using NodaTime;

namespace DVAESABot.ScheduledHeuristics.Heauristics
{
    class NoF111Deseal : IScheduledHeuristic
    {
        public string Description => "Remove F111 deseal factsheets because member and enlisted after deseal.";
        public int Salience => 51;

        public Predicate<ChatContext> Condition => c =>
        {
            if (c.User.UserType.MatchSome(out UserType userType))
                if (userType == UserType.Member)
                    if (c.User.EnlistmentDate.MatchSome(out LocalDate enlistDate))
                        if (enlistDate.CompareTo(new LocalDate(2001, 1, 1)) >= 0)
                            return true;
            return false;
        };

        public Action<ChatContext> Action => c => c.FactsheetShortlist.RemoveCategories("F111");
    }

}