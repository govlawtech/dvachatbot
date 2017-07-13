using System.Threading.Tasks;
using DVAESABot.Domain;
using DVAESABot.ScheduledHeuristics.HeuristicQnAs;
using Microsoft.Bot.Builder.Dialogs;

namespace DVAESABot.Dialogs
{
    public class HeuristicsParentDialog : IDialog<object>
    {
        private readonly MemberTypeHeuristicQnA _memberTypeHeuristicQnA = new MemberTypeHeuristicQnA();
        private readonly SeekingTreatmentQnA _seekingTreatmentQnA = new SeekingTreatmentQnA();

        public async Task StartAsync(IDialogContext context)
        {
            context.Call(_memberTypeHeuristicQnA.Dialog, ResumeAfterUserTypeQuestion);
        }

        private async Task ResumeAfterUserTypeQuestion(IDialogContext dialogContext, IAwaitable<UserType> userType)
        {
            var cc = dialogContext.GetChatContextOrDefault();
            _memberTypeHeuristicQnA.SetResult(cc,
                userType.GetAwaiter().GetResult());
            dialogContext.SetChatContext(cc);

            if (_seekingTreatmentQnA.IsRelevant(cc))
            {
                dialogContext.Call(_seekingTreatmentQnA.Dialog, ResumeAfterTreatmentQuestion);
            }
            else
            {
                dialogContext.Done(true);
            }
        }

        private async Task ResumeAfterTreatmentQuestion(IDialogContext dialogContext,
            IAwaitable<bool> isSeekingTreatment)
        {
            _seekingTreatmentQnA.SetResult(dialogContext.GetChatContextOrDefault(),
                isSeekingTreatment.GetAwaiter().GetResult());

            dialogContext.Done(true);
        }
    }
}