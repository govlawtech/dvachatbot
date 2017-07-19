using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using AdaptiveCards;
using Chronic;
using DVAESABot.Domain;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace DVAESABot.Dialogs
{
    [Serializable]
    public class TopicSelectionDialog : IDialog<Tuple<SearchSelection, string>>
    {
        public static readonly int RESULTS_TO_DISPLAY = 3;
        private readonly string NOT_INTERESTED_TEXT = "NI";
        public async Task StartAsync(IDialogContext context)
        {
            Contract.Requires(context.GetNumberOfFactSheetsInShortlist() > 0);
            var cc = context.GetChatContextOrDefault();
            var topicSelectenMessage = context.MakeMessage();
            topicSelectenMessage.Attachments = new List<Attachment>
            {
                new Attachment
                {
                    ContentType = AdaptiveCard.ContentType,
                    Content = BuildCard("Suggested topics:",
                        cc.FactsheetShortlist.Take(RESULTS_TO_DISPLAY).Select(fs => fs.FactSheet).ToList())
                }
            };

            await context.PostAsync(topicSelectenMessage);
            context.Wait(ReplyToTopicSelectionQuestion);

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
                Title = factsheets.Count > 1 ? "Not interested in any of these" : "Not interested in this",
                Data = NOT_INTERESTED_TEXT
            };

            card.Actions.Add(notInterestedAction);

            return card;
        }

        private async Task ReplyToTopicSelectionQuestion(IDialogContext context, IAwaitable<IMessageActivity> item)
        {
            var reply = (await item).Text;
            var cc = context.GetChatContextOrDefault();
            if (cc.FactsheetShortlist.Select(fs => fs.FactSheet.FactsheetId).Any(i => i == reply))
                context.Done(new Tuple<SearchSelection, string>(SearchSelection.TopicSelected, reply));

            else if (reply == NOT_INTERESTED_TEXT)
            {
                cc.FactsheetShortlist = cc.FactsheetShortlist.Skip(RESULTS_TO_DISPLAY).ToList();
                context.SetChatContext(cc);
                context.Done(new Tuple<SearchSelection, string>(SearchSelection.NotInterestedExpressed, reply));
            }

            else
                context.Done(new Tuple<SearchSelection, string>(SearchSelection.SomethingElseTyped, reply));
        }



    }
}