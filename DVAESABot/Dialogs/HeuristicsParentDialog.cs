using System;
using System.Threading.Tasks;
using DVAESABot.Domain;
using DVAESABot.ScheduledHeuristics;
using Microsoft.Bot.Builder.Dialogs;

namespace DVAESABot.Dialogs
{
    [Serializable]
    public class HeuristicsParentDialog : IDialog<object>
    {
        private readonly HeuristicsFacade _heuristicsFacade = new HeuristicsFacade();

        public async Task StartAsync(IDialogContext context)
        {
            context.Call(new UserTypeDialog(), Resume);
        }


        private async Task Resume(IDialogContext context, IAwaitable<UserType> result)
        {
            if (result.GetAwaiter().GetResult() == UserType.Member)
                context.Call(new EnlistmentDateDialog(),
                    async (dialogContext, awaitable) =>
                    {
                        dialogContext.Call(new AgeDialog(), 
                            async (context1, result1) => FinishHeuristics(context1));
                    });
            else
                await FinishHeuristics(context);
        }

        private async Task FinishHeuristics(IDialogContext context)
        {
            var cc = context.GetChatContextOrDefault();
            _heuristicsFacade.ApplyHeuristics(cc);
            context.SetChatContext(cc);
            context.Done(true);
        }
    }
}