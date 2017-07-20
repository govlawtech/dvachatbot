using System;
using System.Diagnostics.Contracts;
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
        private ChatContext chatContext;

        public RootDialog(ChatContext existingChatContext = null)
        {
            chatContext = existingChatContext ?? ChatContext.CreateEmpty();
        }

        public async Task StartAsync(IDialogContext context)
        {
            context.SetChatContext(chatContext);
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
                await context.Forward(new AzureSearchDialog(), ResumeAfterTopicSelection,
                    new Activity {Text = answerShown});
        }

        private async Task ResumeAfterTopicSelection(IDialogContext context, IAwaitable<Tuple<SearchSelection, string>> result)
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
                    context.Call(new QnAFactsheetDialog(chosenFactSheetTitle, url), ResumeAfterQnADialog);
                }
            }
            else if (userResponseToSearchResults == SearchSelection.SomethingElseTyped)
            {
                await context.Forward(new CuratedQuestionsDialog(), ResumeAfterCuratedQuestionsDialog,
                    new Activity { Text = awaitedResult.Item2 });
            }
            else if (userResponseToSearchResults == SearchSelection.NotInterestedExpressed)
            {
                context.Call(new HeuristicsParentDialog(), ResumeAfterHeuristicsRun);
            }
        }

        private async Task ResumeAfterHeuristicsRun(IDialogContext context, IAwaitable<object> result)
        {
            if (context.GetNumberOfFactSheetsInShortlist() > 0)
                context.Call(new TopicSelectionDialog(), ResumeAfterHeuristicDrivenTopics);
            else
            {
                await GatherMoreDetail(context);
            }
        }

        private async Task GatherMoreDetail(IDialogContext context)
        {
            var previousQuery = context.GetChatContextOrDefault().PreviousQueries.LastOrDefault();
            Contract.Assert(previousQuery != null, "Should only reach this point if at least one search run.");
            await context.SayAsync(
                $"Previously when asked to describe the topic you were interested in, you said, '{previousQuery}'.  Please describe this is more detail.");
            context.Wait(ResumeAfterMoreDetailProvided);
        }

        private async Task ResumeAfterMoreDetailProvided(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var additionalDetail = result.GetAwaiter().GetResult().Text;
            var queryConcatenatedWithLast = context.GetLastSearchQuery() + additionalDetail;
            await context.Forward(new AzureSearchDialog(), ResumeAfterTopicSelection,
                new Activity() {Text = queryConcatenatedWithLast});
        }

        private async Task ResumeAfterHeuristicDrivenTopics(IDialogContext context, IAwaitable<Tuple<SearchSelection, string>> result)
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
                    context.Call(new QnAFactsheetDialog(chosenFactSheetTitle, url), ResumeAfterQnADialog);
                }
            }
            else if (userResponseToSearchResults == SearchSelection.SomethingElseTyped)
            {
                await context.Forward(new AzureSearchDialog(), ResumeAfterTopicSelection,
                    new Activity { Text = awaitedResult.Item2 });
            }
            else if (userResponseToSearchResults == SearchSelection.NotInterestedExpressed)
            {
                if (context.GetNumberOfFactSheetsInShortlist() > 0)
                {
                    context.Call(new TopicSelectionDialog(), ResumeAfterHeuristicDrivenTopics);
                }
                else
                {
                    await GatherMoreDetail(context);
                }
            }
        }

        private async Task ResumeAfterQnADialog(IDialogContext context, IAwaitable<object> result)
        {
            if (context.GetNumberOfFactSheetsInShortlist() > 0)
                context.Call(new TopicSelectionDialog(), ResumeAfterTopicSelection);
            else await GatherMoreDetail(context);
        }
    }
}