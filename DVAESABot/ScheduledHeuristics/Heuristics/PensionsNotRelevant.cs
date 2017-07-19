using System;
using System.Linq;
using DVAESABot.Domain;

namespace DVAESABot.ScheduledHeuristics.Heuristics
{
    public class PensionsNotRelevant : IScheduledHeuristic
    {
        public string Description => "User 65 or under, therefore no pensions";

        public Predicate<ChatContext> Condition => c =>
        {
            if (!c.User.UserType.HasValue) return false;
            if (c.User.UserType != UserType.Member && c.User.UserType != UserType.DependentOnDeceasedMember &&
                c.User.UserType != UserType.DependentOnMember) return false;
            if (!c.User.Age.HasValue) return false;
            return c.User.Age.Value <= 65;
        };

        public Action<ChatContext> Action => c =>
        {
            c.FactsheetShortlist =
                c.FactsheetShortlist.Where(f => !f.FactSheet.FactsheetId.ToLowerInvariant().Contains("pension"))
                    .ToList();
        };
    }
}