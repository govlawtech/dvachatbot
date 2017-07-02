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
            if (!c.User.UserType.MatchSome(out UserType userType)) return false;
            if (userType != UserType.Member && userType != UserType.DependentOnDeceasedMember &&
                userType != UserType.DependentOnMember) return false;
            if (!c.User.Age.MatchSome(out int age)) return false;
            return age > 67;
        };
    

    public Action<ChatContext> Action => c => c.FactsheetShortlist.RemoveCategories("ISS");
    }
}