using System;
using DVAESABot.Domain;
using DVAESABot.Utilities;
using Microsoft.Bot.Builder.Dialogs;

namespace DVAESABot.ScheduledHeuristics.Heuristics
{
    internal class SeekingTreatmentOrRehab : IScheduledHeuristic, IHaveDialog<bool>
    {
        public IDialog<bool> GetDialog()
        {
            throw new NotImplementedException();
        }

        public void ApplyResult(bool result, ChatContext chatContext)
        {
            chatContext.User.SeekingTreatmentOrRehab = result;
        }

        public string Description => "Member seeking treatment or rehab, therefore limit to MRC, HSV and VVCS";
        public int Salience => 150;

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