using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using DVAESABot.Domain;
using DVAESABot.ScheduledHeuristics;
using DVAESABot.ScheduledHeuristics.HeuristicQnAs;
using Microsoft.Bot.Builder.Dialogs;
using NodaTime;

namespace DVAESABot.Dialogs
{
    [Serializable]
    public class HeuristicsParentDialog : IDialog<string>
    {
        private readonly HeuristicsFacade _heuristicsFacade = new HeuristicsFacade();

        public async Task StartAsync(IDialogContext context)
        {
            context.Call(new UserTypeDialog(),Resume);
        }


        private async Task Resume(IDialogContext context, IAwaitable<UserType> result)
        {
           
                if (result.GetAwaiter().GetResult() == UserType.Member)
                {
                    context.Call(new EnlistmentDateDialog(), (dialogContext, awaitable) => FinishHeuristics(dialogContext));

                }
                else
                {
                    FinishHeuristics(context);
                }
           
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