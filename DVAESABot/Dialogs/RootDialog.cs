using System;
using System.Linq;
using System.Threading.Tasks;
using DVAESABot.Domain;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace DVAESABot.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            context.SetChatContext(ChatContext.CreateEmpty());
            context.Call(new CuratedQuestionsDialog(), ResumeAfterCuratedQuestionsDialog);
        }

        private async Task ResumeAfterCuratedQuestionsDialog(IDialogContext context,
            IAwaitable<Tuple<bool, string>> answerWasDisplayed)
        {
            var answerShown = await answerWasDisplayed;
            if (answerShown.Item1)
                context.Call(new CuratedQuestionsDialog(), (dialogContext, result) => StartAsync(context));
            else
                await context.Forward(new AzureSearchDialog(), ResumeAfterSearchDialog,
                    new Activity {Text = answerShown.Item2});
        }

        private async Task ResumeAfterSearchDialog(IDialogContext context, IAwaitable<Tuple<bool, string>> result)
        {
            var awaitedResult = await result;
            var factsheetWasChosen = awaitedResult.Item1;
            if (factsheetWasChosen)
            {
                var chosenFactSheetTitle = awaitedResult.Item2;
                
                var factsheet = context.GetChatContextOrDefault().FactsheetShortlist.FirstOrDefault(f => f.FactSheet.FactsheetId == chosenFactSheetTitle);
                if (factsheet != null)
                {
                    var url = factsheet.FactSheet.Url;
                    context.Call(new QnAFactsheetDialog(chosenFactSheetTitle, url), LandingPad);
                }
            }

            else
            {
                context.Call(new HeuristicsParentDialog(), LandingPad);
            }
        }

        private async Task LandingPad(IDialogContext context, IAwaitable<object> result)
        {
            context.Call(new CuratedQuestionsDialog(), ResumeAfterCuratedQuestionsDialog);
        }
    }
}