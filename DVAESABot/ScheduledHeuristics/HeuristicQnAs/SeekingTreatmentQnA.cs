using System.Linq;
using DVAESABot.Dialogs;
using DVAESABot.Domain;
using Microsoft.Bot.Builder.Dialogs;

namespace DVAESABot.ScheduledHeuristics.HeuristicQnAs
{
    public class SeekingTreatmentQnA : IHeuristicQnA<bool>
    {
        private YesNoDialog _dialog;

        public SeekingTreatmentQnA()
        {
            _dialog = new YesNoDialog("Are you seeking treatment or rehabilitation for an injury or illness?");
        }

        IDialog<bool> IHeuristicQnA<bool>.Dialog => _dialog;

        public void ApplyResult(ChatContext chatContext, object dialogResult)
        {
            chatContext.User.SeekingTreatmentOrRehab = dialogResult as bool?;
        }

        public bool IsRelevant(ChatContext chatContext)
        {
            var categoriesForTreatmentAndRehab = new[] {"HSV", "MRC"};

            return chatContext.User.UserType == UserType.Member &&
                   !chatContext.User.SeekingTreatmentOrRehab.HasValue &&
                   chatContext.FactsheetShortlist.GetCategories().Any(c => !categoriesForTreatmentAndRehab.Contains(c));
        }

    }
}