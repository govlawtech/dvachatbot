using System;
using DVAESABot.Domain;

namespace DVAESABot.ScheduledHeuristics.Heuristics
{
    public class IncomeSupportNotRelevant : IScheduledHeuristic
    {
        public string Description => "User over 67, therefore no income support.";
        public int Salience => 40;

        public Predicate<ChatContext> Condition => c =>
        {
            if (!c.User.UserType.HasValue) return false;
            if (c.User.UserType != UserType.Member && c.User.UserType != UserType.DependentOnDeceasedMember &&
                c.User.UserType != UserType.DependentOnMember) return false;
            if (!c.User.Age.HasValue) return false;
            return c.User.Age.Value > 67;
        };

    public Action<ChatContext> Action => c => c.FactsheetShortlist = c.FactsheetShortlist.RemoveCategories("ISS");
    }
}