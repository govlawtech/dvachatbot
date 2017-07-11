using DVAESABot.Dialogs;
using DVAESABot.Domain;
using Microsoft.Bot.Builder.Dialogs;

namespace DVAESABot.ScheduledHeuristics.HeuristicQnAs
{
    public class SeekingTreatmentQnA : IHeuristicQnA<bool,bool>
    {
        public IDialog<bool> Dialog => new YesNoDialog("Are you seeking treatment or rehabilitation for a medical condition?");

        public void ApplyResult(ChatContext chatContext, bool dialogResult)
        {
            chatContext.User.SeekingTreatmentOrRehab = dialogResult;
        }

        public bool IsStillRelevant(ChatContext chatContext)
        {
            return (chatContext.User.UserType == UserType.Member && !chatContext.User.SeekingTreatmentOrRehab.HasValue);
        }
    }
}