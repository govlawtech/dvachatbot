using System;
using System.Linq;
using System.Threading.Tasks;
using DVAESABot.Domain;
using DVAESABot.ScheduledHeuristics;
using DVAESABot.Search;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace DVAESABot.Dialogs
{
    public enum SearchSelection
    {
        TopicSelected,
        NotInterestedExpressed,
        SomethingElseTyped
    }

    /// <summary>
    ///     Dialog that interacts with Azure Search.
    /// </summary>
    [Serializable]
    public class AzureSearchDialog : IDialog<Tuple<SearchSelection, string>>
    {
        private readonly string NOT_INTERESTED_TEXT = "NI";
        private readonly int RESULTS_TO_RETRIEVE = 50;

        public async Task StartAsync(IDialogContext context)
        {
            await context.SayAsync("Please describe the topic you are interested in.");   
            context.Wait(MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> item)
        {
            await context.SayAsync("Looking for topics...");
            var searchQuery = (await item).Text;

            var cc = context.GetChatContextOrDefault();

            using (var ac = FactSheetSearchClient.CreateDefault())
            {
                var results = await ac.GetTopMatchingFactsheets(searchQuery, RESULTS_TO_RETRIEVE);
                cc.FactsheetShortlist = results.Results.Select(r => new FactSheetWithScore(r)).ToList();
                var hf = new HeuristicsFacade();
                hf.ApplyHeuristics(cc);
                context.UserData.SetValue(typeof(ChatContext).Name, cc);
            }

            if (cc.FactsheetShortlist.Any())
            {
                context.Call(new TopicSelectionDialog(), Resume);
            }
            else
            {
                await context.PostAsync("Can't find anything at all on that.");
                context.Wait(MessageReceivedAsync);
            }
        }

        private async Task Resume(IDialogContext context, IAwaitable<Tuple<SearchSelection, string>> result)
        {
            context.Done(result.GetAwaiter().GetResult());
        }
    }
}