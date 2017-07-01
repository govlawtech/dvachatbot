using System;
using DVAESABot.Domain;
using DVAESABot.Utilities;

namespace DVAESABot.ScheduledHeuristics.Heuristics
{
    class SeekingTreatmentOrRehab : IScheduledHeuristic, IHaveDialog<bool>
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
        public IHaveDialog<bool> GetDialog()
        {
            throw new NotImplementedException();
        }

        public void ApplyResult(bool result, ChatContext chatContext)
        {
            chatContext.User.SeekingTreatmentOrRehab = new Some<bool>(result);
        }
    }
}