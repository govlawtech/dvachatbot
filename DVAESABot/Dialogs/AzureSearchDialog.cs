using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdaptiveCards;
using Chronic;
using DVAESABot.Domain;
using DVAESABot.Heuristics;
using DVAESABot.Search;
using Microsoft.Azure.Search.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace DVAESABot.Dialogs
{
    /// <summary>
    ///     Dialog that interacts with Azure Search.
    /// </summary>
    [Serializable]
    public class AzureSearchDialog : IDialog<Tuple<bool,string>>
    {
        private readonly string _initialSearchText;
        private const int RESULTS_TO_DISPLAY = 5;

        
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> item)
        {
            var userInput = (await item).Text;

            if (context.UserData.TryGetValue(typeof(ChatContext).Name, out ChatContext cc))
            {
                using (var ac = FactSheetSearchClient.CreateDefault())
                {
                    var results = await ac.GetTopMatchingFactsheets(userInput, 5);
                    cc.FactsheetShortlist = results.Results.Select(r => new FactSheetWithScore(r)).ToList();
                    context.UserData.SetValue(typeof(ChatContext).Name,cc);
                }
            }

            if (cc.FactsheetShortlist.Any())
            {
                var replyToConversation = context.MakeMessage();
                replyToConversation.Attachments = new List<Attachment>()
                {
                    new Attachment()
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
                context.Done(new Tuple<bool,string>(false,userInput));
            }
            
        }


        private AdaptiveCard BuildCard(string message, List<FactSheet> factsheets)
        {
            var card = new AdaptiveCard();
            
            card.Body.Add(new TextBlock
            {
                Text = message 
            });

            factsheets.ForEach(sheet => card.Actions.Add(new SubmitAction
            {
                Title = $"{DialogHelper.GetWrappedFactsheetTitle(sheet.FactsheetId, 30)}",
                Data = $"{sheet.FactsheetId}"
            }));

            return card;
        }

        private async Task ReplyToFactSheetQuestion(IDialogContext context, IAwaitable<IMessageActivity> item)
        {
            var reply = await item;

            if (context.UserData.TryGetValue(typeof(ChatContext).Name, out ChatContext cc))
            {
                
                if (cc.FactsheetShortlist.Select(fs => fs.FactSheet.FactsheetId).Any(i => i == reply.Text))
                {
                    context.Done(new Tuple<bool,string>(true,reply.Text));
                }

                else
                {
                    context.Done(new Tuple<bool,string>(false,reply.Text));
                }
            }
        }
    }
}