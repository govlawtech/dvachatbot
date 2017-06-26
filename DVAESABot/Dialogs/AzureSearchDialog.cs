using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;
using DVAESABot.Search;
using Microsoft.Azure.Search.Models;
using DVAESABot.Domain;

namespace DVAESABot.Dialogs
{
    /// <summary>
    /// Dialog that interacts with Azure Search.
    /// </summary>
    [Serializable]
    public class AzureSearchDialog : IDialog<string>
    {
        public async Task StartAsync(IDialogContext context)
        {
            // TODO Tidy up user interaction/prompts
            await context.PostAsync("I will try to return the most relevant factsheets");
            context.Wait(MessageReceived);
        }

        // Call Azure Search
        private async Task MessageReceived(IDialogContext context, IAwaitable<IMessageActivity> item)
        {
            var userInput = await item;

            FactSheetSearchClient client = FactSheetSearchClient.CreateDefault();
            DocumentSearchResult<FactSheet> result = await client.GetTopMatchingFactsheets(userInput.Text, 3);
            // Restricting to MRCA
            //string mrcaFactsheet = null;
            bool topResult = true;

            var message = "Factsheets that match:\n\n";
            foreach (var r in result.Results)
            {
                //if (r.Document.FactsheetId.Contains("MRC"))
                //{
                //    mrcaFactsheet = r.Document.FactsheetId;
                //}
                if (topResult)
                {
                    DialogHelper.StoreKbDetails(context, r.Document.FactsheetId);
                    topResult = false;
                }
                message += $"{r.Document.FactsheetId} ({r.Score})\n\n";
            }
            
            await PostResponseWithFeedback(context, message);
        }

        // For processing feedback from users
        public async Task FeedbackReceived(IDialogContext context, IAwaitable<IMessageActivity> item)
        {
            await ProcessFeedbackInternal(context, item, true);
        }

        // For processing QnA feedback from users
        public async Task QnAFeedbackReceived(IDialogContext context, IAwaitable<IMessageActivity> item)
        {
            await ProcessFeedbackInternal(context, item);
        }

        // Helper method for processing feedback
        private async Task ProcessFeedbackInternal(IDialogContext context, IAwaitable<IMessageActivity> item, bool launchQnA = false)
        {
            var reply = (Activity)await item;
            string message = $"Sorry to hear that. We will incorporate your feedback.";
            string userResponse = reply.Text.ToLowerInvariant();

            var positiveResponses = new List<string> { "yes", "y" };
            if (positiveResponses.Contains(userResponse, StringComparer.OrdinalIgnoreCase))
            {
                if (launchQnA)
                {
                    context.Call(new QnAMRCADialog(), ResumeAfterQnA);
                }
                else
                {
                    message = "Great to hear!";
                    await context.PostAsync(message);
                    context.Wait(MessageReceived);
                }
            }
            else
            {
                await context.PostAsync(message);
                context.Wait(MessageReceived);
            }
        }

        // Helper method for posting a message with a standard feedback question
        private async Task PostResponseWithFeedback(IDialogContext context, string message)
        {
            await DialogHelper.PostMessage(context, message);
            context.Wait(FeedbackReceived);
        }

        // Helper method for displaying response from QnA with feedback
        private async Task ResumeAfterQnA(IDialogContext context, IAwaitable<string> result)
        {
            var resultFromQnA = await result;
            await DialogHelper.PostMessage(context, resultFromQnA + "\n\n\n\n");
            context.Wait(this.QnAFeedbackReceived);
        }
    }
}