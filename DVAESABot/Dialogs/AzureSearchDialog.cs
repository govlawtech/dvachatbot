using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdaptiveCards;
using Chronic;
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
        private readonly int RESULTS_TO_DISPLAY = 5;


        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> item)
        {
            var userInput = (await item).Text;

            if (context.UserData.TryGetValue(typeof(ChatContext).Name, out ChatContext cc))
                using (var ac = FactSheetSearchClient.CreateDefault())
                {
                    var results = await ac.GetTopMatchingFactsheets(userInput, 50);
                    cc.FactsheetShortlist = results.Results.Select(r => new FactSheetWithScore(r)).ToList();
                    var hf = new HeuristicsFacade();
                    hf.ApplyHeuristics(cc);
                    context.UserData.SetValue(typeof(ChatContext).Name, cc);
                }

            if (cc.FactsheetShortlist.Any())
            {
                var replyToConversation = context.MakeMessage();
                replyToConversation.Attachments = new List<Attachment>
                {
                    new Attachment
                    {
                        ContentType = AdaptiveCard.ContentType,
                        Content = BuildCard("Suggested topics:",
                            cc.FactsheetShortlist.Take(RESULTS_TO_DISPLAY).Select(fs => fs.FactSheet).ToList())
                    }
                };

                await context.PostAsync(replyToConversation);
                context.Wait(ReplyToFactSheetQuestion);
            }
            else
            {
                await context.PostAsync("Can't find anything at all on that.");
                context.Done(new Tuple<bool, string>(false, userInput));
            }
        }


        private AdaptiveCard BuildCard(string message, List<FactSheet> factsheets)
        {
            var card = new AdaptiveCard();

            card.Body.Add(new TextBlock
            {
                Text = message
            });
            
            factsheets.Take(3).ForEach(sheet => card.Actions.Add(new SubmitAction
            {
                Title = $"{DialogHelper.GetWrappedFactsheetTitle(sheet.FactsheetId, 30)}",
                Data = $"{sheet.FactsheetId}"
            }));
            

            var notInterestedAction = new SubmitAction
            {
                Title = "Not interested in any of these",
                Data = NOT_INTERESTED_TEXT
            };

            card.Actions.Add(notInterestedAction);

            return card;
        }

        private async Task ReplyToFactSheetQuestion(IDialogContext context, IAwaitable<IMessageActivity> item)
        {
            var reply = (await item).Text;
            var cc = context.GetChatContextOrDefault();
            if (cc.FactsheetShortlist.Select(fs => fs.FactSheet.FactsheetId).Any(i => i == reply))
                context.Done(new Tuple<SearchSelection, string>(SearchSelection.TopicSelected, reply));
            
            else if (reply == NOT_INTERESTED_TEXT)
            {
                context.Done(new Tuple<SearchSelection,string>(SearchSelection.NotInterestedExpressed, reply));
            }

            else
                context.Done(new Tuple<SearchSelection, string>(SearchSelection.SomethingElseTyped, reply));
        }
    }
}