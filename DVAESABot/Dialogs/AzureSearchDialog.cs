using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;
using DVAESABot.Search;
using Microsoft.Azure.Search.Models;
using DVAESABot.Domain;
using AdaptiveCards;
using DVAESABot.Heuristics;

namespace DVAESABot.Dialogs
{
    /// <summary>
    /// Dialog that interacts with Azure Search.
    /// </summary>
    [Serializable]
    public class AzureSearchDialog : IDialog<string>
    {
        // TODO Consider https://docs.microsoft.com/en-us/bot-framework/dotnet/bot-builder-dotnet-search-azure

        private const double MIN_SCORE = 0.6;
        private const int RESULTS_TO_DISPLAY = 3;
        private const string NOT_INTERESTED = "not-interested";
        private const string HEURISTIC_HISTORY = "HeuristicHistory";
        private const string CONTACT_DVA = "I am sorry I could not help you with your questions.\n\n\n\nHere's how to contact DVA - https://www.dva.gov.au/contact";

        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceived);
        }

        // Call Azure Search
        private async Task MessageReceived(IDialogContext context, IAwaitable<IMessageActivity> item)
        {
            var userInput = await item;

            FactSheetSearchClient client = FactSheetSearchClient.CreateDefault();
            DocumentSearchResult<FactSheet> result = await client.GetTopMatchingFactsheets(userInput.Text, 10);
            int count = 0;

            if (result.Results.Count == 0)
            {
                await context.PostAsync("I am sorry, I don't understand.\n\n\n\nCan you describe your situation or question in different words?");
                context.Wait(MessageReceived);
            }
            else if (result.Results.First().Score < MIN_SCORE)
            {
                // TODO Get exact wording
                await context.PostAsync($"I am not confident about answering your question accurately.\n\n\n\nIs there anything else that you need? ");
                context.Wait(MessageReceived);
            }
            else
            {
                // Reset shortlist and heuristic history
                DialogHelper.SetFactsheetShortlist(context, new List<string>());
                ResetHeuristicHistory(context);

                var replyToConversation = context.MakeMessage();
                replyToConversation.Attachments = new List<Attachment>();

                AdaptiveCard card = new AdaptiveCard();

                // Add text to the card.
                card.Body.Add(new TextBlock()
                {
                    Text = "I can give you information on these topics:"
                });

                // Add buttons to the card.
                foreach (var r in result.Results)
                {
                    var factsheetKey = r.Document.FactsheetId;
                    // Get rid of results that do not meet the cutoff, or we reached our max to display
                    if ((r.Score < MIN_SCORE) || (count >= RESULTS_TO_DISPLAY))
                    {
                        // Add it to shortlist, in case we need it later for heuristics
                        DialogHelper.AddFactsheetCodeToShortlist(context, factsheetKey);
                    }
                    else
                    {
                        count++;
                        card.Actions.Add(new SubmitAction()
                        {
                            Title = $"{DialogHelper.ExtractFactsheetTitleFromIntent(ref factsheetKey)})",
                            Data = $"{factsheetKey}"
                        });
                    }
                }

                // If we are here, there will be at least one with > 0.8
                card.Actions.Add(new SubmitAction()
                {
                    Title = $"Not interested in any of these",
                    Data = NOT_INTERESTED
                });

                // Create the attachment.
                Attachment attachment = new Attachment()
                {
                    ContentType = AdaptiveCard.ContentType,
                    Content = card
                };

                replyToConversation.Attachments.Add(attachment);
                await context.PostAsync(replyToConversation);
                context.Wait(FactsheetNominated);
            }
        }

        // Process when a user has nominated a factsheet
        private async Task FactsheetNominated(IDialogContext context, IAwaitable<IMessageActivity> item)
        {
            var userInput = await item;
            if (NOT_INTERESTED.Equals(userInput.Text, StringComparison.OrdinalIgnoreCase))
            {
                // Process heuristics
                // TODO Apply randomly
                // TODO Refactor and use something like the visitor pattern?
                IHeuristic jobLinedUpHeuristic = new JobLinedUpHeuristic();
                IHeuristic isMemberHeuristic = new IsMemberHeuristic();
                IHeuristic enlistBefore1July2004Heuristic = new EnlistBefore1July2004Heuristic();
                if (jobLinedUpHeuristic.isApplies(DialogHelper.GetFactsheetShortlist(context)))
                {
                    context.UserData.SetValue<string>(HeuristicYesNoDialog.HEURISTIC_NAME, jobLinedUpHeuristic.name());
                    context.Call(new HeuristicYesNoDialog(), ResumeAfterHeuristic);
                }
                else if (isMemberHeuristic.isApplies(DialogHelper.GetFactsheetShortlist(context)))
                {
                    context.UserData.SetValue<string>(HeuristicYesNoDialog.HEURISTIC_NAME, isMemberHeuristic.name());
                    context.Call(new HeuristicYesNoDialog(), ResumeAfterHeuristic);
                }
                else if (enlistBefore1July2004Heuristic.isApplies(DialogHelper.GetFactsheetShortlist(context)))
                {
                    context.UserData.SetValue<string>(HeuristicYesNoDialog.HEURISTIC_NAME, enlistBefore1July2004Heuristic.name());
                    context.Call(new HeuristicYesNoDialog(), ResumeAfterHeuristic);
                }
                else
                {
                    // No heuristics can be applied
                    await NoMatch(context);
                }
            }
            else
            {
                DialogHelper.StoreKbDetails(context, userInput.Text);
                context.Call(new QnAFactsheetDialog(), ResumeAfterQnA);
            }
        }

        // Helper method for displaying response from heuristic
        private async Task ResumeAfterHeuristic(IDialogContext context, IAwaitable<string> result)
        {
            var resultFromHeuristic = await result;

            // Track history of heuristics that are applied
            AddHeuristicToHistory(context, resultFromHeuristic);

            // Check if there are any factsheets left
            if (DialogHelper.GetFactsheetShortlist(context).Count == 0)
            {
                await NoMatch(context);
            }

            // Apply next heuristic
            // TODO Refactor and use something like the visitor pattern?
            IHeuristic isMemberHeuristic = new IsMemberHeuristic();
            IHeuristic enlistBefore1July2004Heuristic = new EnlistBefore1July2004Heuristic();
            // First one (JobLinedUpHeuristic) already checked previously
            if (!HeuristicWasRun(context, isMemberHeuristic.name()) &&
                isMemberHeuristic.isApplies(DialogHelper.GetFactsheetShortlist(context)))
            {
                context.UserData.SetValue<string>(HeuristicYesNoDialog.HEURISTIC_NAME, isMemberHeuristic.name());
                context.Call(new HeuristicYesNoDialog(), ResumeAfterHeuristic);
            }
            else if (!HeuristicWasRun(context, enlistBefore1July2004Heuristic.name()) &&
                enlistBefore1July2004Heuristic.isApplies(DialogHelper.GetFactsheetShortlist(context)))
            {
                context.UserData.SetValue<string>(HeuristicYesNoDialog.HEURISTIC_NAME, enlistBefore1July2004Heuristic.name());
                context.Call(new HeuristicYesNoDialog(), ResumeAfterHeuristic);
            }
            else
            {
                // Show results of heuristics, i.e. QnA for top result
                List<string> history = DialogHelper.GetFactsheetShortlist(context);
                // There will always be at least one item in the history at this stage
                context.UserData.SetValue<string>(DialogHelper.FACTSHEET_NAME, history.First());
                context.UserData.SetValue<string>(DialogHelper.KB_ID, DialogHelper.GetKbIdFromFactsheetCode(history.First()));
                context.Call(new QnAFactsheetDialog(), ResumeAfterQnA);
            }
        }

        // For processing feedback from users
        public async Task FeedbackReceived(IDialogContext context, IAwaitable<IMessageActivity> item)
        {
            await ProcessFeedbackInternal(context, item, true);
        }

        // Adds the heuristic (name) to history
        private void AddHeuristicToHistory(IDialogContext context, string heuristicName)
        {
            List<string> history = context.UserData.GetValueOrDefault<List<string>>(HEURISTIC_HISTORY, new List<string>());
            history.Add(heuristicName);
            context.UserData.SetValue<List<string>>(HEURISTIC_HISTORY, history);
        }

        // Reset the heuristic history
        private void ResetHeuristicHistory(IDialogContext context)
        {
            context.UserData.SetValue<List<string>>(HEURISTIC_HISTORY, new List<string>());
        }

        // Returns true if the heuristic was previously run
        private bool HeuristicWasRun(IDialogContext context, string heuristicName)
        {
            List<string> history = context.UserData.GetValueOrDefault<List<string>>(HEURISTIC_HISTORY, new List<string>());

            return history.Contains(heuristicName);
        }

        // Helper method for no matches from search
        private async Task NoMatch(IDialogContext context)
        {
            await context.PostAsync(CONTACT_DVA);
            context.Wait(MessageReceived);
        }

        // Helper method for processing feedback
        private async Task ProcessFeedbackInternal(IDialogContext context, IAwaitable<IMessageActivity> item, bool launchQnA = false)
        {
            var reply = (Activity)await item;
            string message = $"Sorry to hear that. We will incorporate your feedback.";

            if (DialogHelper.POSITIVE_RESPONSES.Contains(reply.Text, StringComparer.OrdinalIgnoreCase))
            {
                if (launchQnA)
                {
                    context.Call(new QnAFactsheetDialog(), ResumeAfterQnA);
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
            await DialogHelper.PostMessageWithFeedback(context, message);
            context.Wait(FeedbackReceived);
        }

        // Helper method for displaying response from QnA with feedback
        private async Task ResumeAfterQnA(IDialogContext context, IAwaitable<string> result)
        {
            var resultFromQnA = await result;
            if (resultFromQnA.Equals("3", StringComparison.OrdinalIgnoreCase))
            {
                await context.PostAsync(CONTACT_DVA);
            }
            else
            {
                await DialogHelper.PostMessageWithFeedback(context, resultFromQnA + "\n\n\n\n");
            }
            context.Wait(MessageReceived);
        }
    }
}