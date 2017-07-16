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
            IAwaitable<Tuple<bool, string>> answerWasDisplayedAndAnswer)
        {
            var answerWasShown = answerWasDisplayedAndAnswer.GetAwaiter().GetResult().Item1;
            var answerShown = answerWasDisplayedAndAnswer.GetAwaiter().GetResult().Item2;
            if (answerWasShown)
                context.Call(new CuratedQuestionsDialog(), ResumeAfterCuratedQuestionsDialog);
            else
                await context.Forward(new AzureSearchDialog(), ResumeAfterSearchDialog,
                    new Activity {Text = answerShown});
        }

        private async Task ResumeAfterSearchDialog(IDialogContext context, IAwaitable<Tuple<SearchSelection, string>> result)
        {
            var awaitedResult = await result;
            var userResponseToSearchResults = awaitedResult.Item1;
            if (userResponseToSearchResults == SearchSelection.TopicSelected)
            {
                var chosenFactSheetTitle = awaitedResult.Item2;
                
                var factsheet = context.GetChatContextOrDefault().FactsheetShortlist.FirstOrDefault(f => f.FactSheet.FactsheetId == chosenFactSheetTitle);
                if (factsheet != null)
                {
                    var url = factsheet.FactSheet.Url;
                    context.Call(new QnAFactsheetDialog(chosenFactSheetTitle, url), LandingPad);
                }
            }
            else if (userResponseToSearchResults == SearchSelection.SomethingElseTyped)
            {
                await context.Forward(new AzureSearchDialog(), ResumeAfterSearchDialog,
                    new Activity { Text = awaitedResult.Item2 });
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