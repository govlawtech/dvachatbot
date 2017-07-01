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
            if (c.User.UserType.MatchSome(out UserType userType))
            {
                if (userType == UserType.Member || userType == UserType.DependentOnDeceasedMember ||
                    userType == UserType.DependentOnMember)
                {
                    if (c.User.Age.MatchSome(out int age))
                    {
                        if (age > 67) return true;

                    }
                }

            }
            return false;
        };

        public Action<ChatContext> Action => c => c.FactsheetShortlist.RemoveCategories("ISS");
    }
}