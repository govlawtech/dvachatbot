using System;
using System.Collections.Generic;
using DVAESABot.Domain;

namespace DVAESABot.ScheduledHeuristics.Heuristics
{
    internal class SeekingTreatmentQuestion : IScheduledHeuristic
    {
       
        public void ApplyResult(bool result, ChatContext chatContext)
        {
            chatContext.User.SeekingTreatmentOrRehab = result;
        }

        public string Description => "Member seeking treatment or rehab, therefore limit to MRC, HSV and VVCS";

        public Predicate<ChatContext> Condition => c =>
        {
            if (c.User.UserType == UserType.Member)
                if (c.User.SeekingTreatmentOrRehab == true)
                    return true;
            return false;
        };

        public Action<ChatContext> Action => c => c.FactsheetShortlist = c.FactsheetShortlist.RemoveAllCategoriesOtherThan("MRC", "HSV",
            "VVCS");

       
    }
}