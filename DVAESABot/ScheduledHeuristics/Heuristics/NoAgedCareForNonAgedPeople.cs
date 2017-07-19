using System;
using DVAESABot.Domain;

namespace DVAESABot.ScheduledHeuristics.Heuristics
{
    class NoAgedCareForNonAgedPeople : IScheduledHeuristic
    {
        public string Description => "Don't show aged care topics for people under 60";

        public Predicate<ChatContext> Condition => c => c.User.UserType == UserType.Member &&
                                                        c.User.Age < 60;

        public Action<ChatContext> Action => c =>
        {
            c.FactsheetShortlist = c.FactsheetShortlist.RemoveFactsheetsWithKeyWords("Aged Care");
        };
    }
}