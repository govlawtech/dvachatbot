using System.Linq;
using System.Threading.Tasks;
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

        public IDialog<bool> Dialog => _dialog;

      


        public void SetResult(ChatContext chatContext, bool result)
        {
            chatContext.User.SeekingTreatmentOrRehab = result;
        }

        public bool IsRelevant(ChatContext chatContext)
        {
            var categoriesForTreatmentAndRehab = new[] {"HSV", "MRC"};

            var shortlistedCatagories = chatContext.FactsheetShortlist.GetCategories();

            var relevant = chatContext.User.UserType == UserType.Member &&
                           !chatContext.User.SeekingTreatmentOrRehab.HasValue &&
                           !shortlistedCatagories.All(c => categoriesForTreatmentAndRehab.Contains(c));

            return relevant;

        }

    }
}