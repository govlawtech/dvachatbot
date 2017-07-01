using System;
using DVAESABot.Domain;

namespace DVAESABot.ScheduledHeuristics.Heauristics
{
    class SeekingTreatmentOrRehab : IScheduledHeuristic
    {
        public string Description => "Member seeking treatment or rehab, therefore limit to MRC, HSV and VVCS";
        public int Salience => 150;

        public Predicate<ChatContext> Condition => c =>
        {
            if (c.User.UserType.MatchSome(out UserType userType))
            {
                if (userType == UserType.Member)
                {
                    if (c.User.SeekingTreatmentOrRehab.MatchSome(out bool seekingTorR))
                    {
                        if (seekingTorR)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;

        };
        public Action<ChatContext> Action => c => c.FactsheetShortlist.RemoveAllCategoriesOtherThan("MRC", "HSV", "VVCS");
    }
}