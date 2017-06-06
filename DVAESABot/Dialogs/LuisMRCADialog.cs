using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DVAESABot.Dialogs
{
    /// <summary>
    /// This class handles MRCA LUIS intents, with some ability to ask clarification questions and request feedback.
    /// </summary>
    [Serializable]
    public partial class LuisMRCADialog : LuisDialog<object>
    {
        // Keeps track of how many times the clarification question is asked, so we don't keep spamming it
        private int clarificationCount = 0;

        // The URLs to display to users
        private Dictionary<string, string> urls;

        // A couple of ways to inject LUIS ID and Key, using this one:
        // https://blogs.msdn.microsoft.com/benjaminperkins/2017/03/14/how-to-change-app-settings-azure-app-service-bot-service/
        public LuisMRCADialog() : base(new LuisService(new LuisModelAttribute(ConfigurationManager.AppSettings["LuisAppId"], ConfigurationManager.AppSettings["LuisAPIKey"], verbose: true)))
        {
            urls = new Dictionary<string, string>();
            urls.Add("MRC01", "https://www.dva.gov.au/factsheet-mrc01-overview-military-rehabilitation-and-compensation-act-2004-mrca");
            urls.Add("MRC04", "https://www.dva.gov.au/factsheet-mrc04-compensation-payment-rates");
            urls.Add("MRC05", "https://www.dva.gov.au/factsheet-mrc05-rehabilitation");
            urls.Add("MRC07", "https://www.dva.gov.au/factsheet-mrc07-permanent-impairment-compensation-payments");
            urls.Add("MRC08", "https://www.dva.gov.au/factsheet-mrc08-incapacity-work");
            urls.Add("MRC09", "https://www.dva.gov.au/factsheet-mrc09-special-rate-disability-pension-safety-net-payment-srdp");
            urls.Add("MRC10", "https://www.dva.gov.au/factsheet-mrc10-motor-vehicle-compensation-scheme-mvcs");
            urls.Add("MRC17", "https://www.dva.gov.au/factsheet-mrc17-funeral-expenses");
            urls.Add("MRC18", "https://www.dva.gov.au/factsheet-mrc18-bereavement-payments");
            urls.Add("MRC25", "https://www.dva.gov.au/factsheet-mrc25-how-make-claim-under-military-rehabilitation-and-compensation-act-2004");
            urls.Add("MRC27", "https://www.dva.gov.au/factsheet-mrc27-reconsideration-and-review-decisions");
            urls.Add("MRC30", "https://www.dva.gov.au/factsheet-mrc30-information-reservists");
            urls.Add("MRC31", "https://www.dva.gov.au/factsheet-mrc31-information-about-cadets-officers-cadets-and-instructors-cadets");
            urls.Add("MRC33", "https://www.dva.gov.au/factsheet-mrc33-common-law-action-compensation-service-related-injuries-and-diseases");
            urls.Add("MRC34", "https://www.dva.gov.au/factsheet-mrc34-needs-assessment");
            urls.Add("MRC35", "https://www.dva.gov.au/factsheet-mrc35-common-law-action-compensation-service-related-deaths");
            urls.Add("MRC36", "https://www.dva.gov.au/factsheet-mrc36-voluntary-work");
            urls.Add("MRC39", "https://www.dva.gov.au/factsheet-mrc39-comparison-benefits-dependants");
            urls.Add("MRC40", "https://www.dva.gov.au/factsheet-mrc40-mrca-and-srca-supplements");
            urls.Add("MRC41", "https://www.dva.gov.au/factsheet-mrc41-attendant-care");
            urls.Add("MRC42", "https://www.dva.gov.au/factsheet-mrc42-household-services");
            urls.Add("MRC43", "https://www.dva.gov.au/factsheet-mrc43-compensation-payment-rates");
            urls.Add("MRC45", "https://www.dva.gov.au/factsheet-mrc45-student-start-scholarship-and-relocation-scholarship");
            urls.Add("MRC47", "https://www.dva.gov.au/factsheet-mrc47-education-schemes");
            urls.Add("MRC49", "https://www.dva.gov.au/factsheet-mrc49-income-support-bonus");
            urls.Add("MRC50", "https://www.dva.gov.au/factsheet-mrc50-compensation-dependants-under-military-rehabilitation-and-compensation-act-2004");
        }

        [LuisIntent("")]
        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            string message = "I am sorry, I don't understand.\n\nCan you describe your situation or question in different words?";
            await context.PostAsync(message);
            context.Wait(MessageReceived);
        }

        [LuisIntent("MRC01 - Overview of MRCA")]
        [LuisIntent("MRC04 - MRCA Compensation Payment Rates")]
        [LuisIntent("MRC05 - Rehabilitation")]
        [LuisIntent("MRC07 - Permanent Impairment Compensation Payments")]
        [LuisIntent("MRC08 - Incapacity for Work")]
        [LuisIntent("MRC09 - SRDP")]
        [LuisIntent("MRC10 - Motor Vehicle Compensation Scheme (MVCS)")]
        [LuisIntent("MRC17 - Funeral Expenses")]
        [LuisIntent("MRC18 - Bereavement Payments")]
        [LuisIntent("MRC25 - How to Make a Claim under the MRCA")]
        [LuisIntent("MRC27 - Reconsideration and Review of Decisions")]
        [LuisIntent("MRC30 – Information for Reservists")]
        [LuisIntent("MRC31 - Cadets, Officers of Cadets etc.")]
        [LuisIntent("MRC33 - Common Law Action for Compensation Injury")]
        [LuisIntent("MRC34 - Needs Assessment")]
        [LuisIntent("MRC35 - Common Law Action for Compensation Deaths")]
        [LuisIntent("MRC36 - Voluntary Work")]
        [LuisIntent("MRC39 - Comparison of benefits for dependants")]
        [LuisIntent("MRC40 - MRCA and SRCA Supplements")]
        [LuisIntent("MRC41 - Attendant Care")]
        [LuisIntent("MRC42 - Household Services")]
        [LuisIntent("MRC43 - Compensation Payment Rates")]
        [LuisIntent("MRC43 - SRCA Compensation Payment Rates")]
        [LuisIntent("MRC45 - Student Scholarship")]
        [LuisIntent("MRC47 - Education Schemes")]
        [LuisIntent("MRC49 - Income Support Bonus")]
        [LuisIntent("MRC50 - Compensation for Dependants under MRCA")]
        public async Task IntentFactsheet(IDialogContext context, LuisResult result)
        {
            var topFactsheet = result.TopScoringIntent.Intent;
            var factsheetCode = topFactsheet.Substring(0, 5);
            string message;
            if (urls.ContainsKey(factsheetCode))
            {
                message = $"Factsheets that match:\n\n" +
                    $"[{topFactsheet}]({urls[factsheetCode]}) ({result.TopScoringIntent.Score * 100}%)\n\n";
            }
            else
            {
                message = $"Factsheets that match:\n\n" +
                    $"{topFactsheet} ({result.TopScoringIntent.Score * 100}%)\n\n";
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
                } else
                {
                    message = "Great to hear!";
                    await context.PostAsync(message);
                    context.Wait(MessageReceived);
                }
            } else
            {
                await context.PostAsync(message);
                context.Wait(MessageReceived);
            }
        }

        [LuisIntent("Greeting")]
        public async Task IntentGreeting(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Hello");
            context.Wait(MessageReceived);
        }

        [LuisIntent("Clarification Question")]
        public async Task IntentClarification(IDialogContext context, LuisResult result)
        {
            var topic = result.Entities[0].Entity;

            if (clarificationCount == 0)
            {
                clarificationCount++;
                await context.PostAsync($"Can you tell me more about your {topic}?");
                context.Wait(MessageReceived);
            }
            else
            {
                clarificationCount = 0;
                // Present the next likely match
                var topFactsheet = result.Intents[1].Intent;
                var factsheetCode = topFactsheet.Substring(0, 5);
                string message;
                if (urls.ContainsKey(factsheetCode))
                {
                    message = $"We are not super confident, but we think this factsheet will help:\n\n" +
                        $"[{topFactsheet}]({urls[factsheetCode]}) ({result.Intents[1].Score * 100}%)\n\n";
                }
                else
                {
                    // Avoid 'key not found' if we forget to populate the urls dictionary
                    message = $"We are not super confident, but we think this factsheet will help:\n\n" +
                        $"{topFactsheet} ({result.Intents[1].Score * 100}%)\n\n";
                }
                await PostResponseWithFeedback(context, message);
            }
        }

        // Helper method for posting a message with a standard feedback question
        private async Task PostResponseWithFeedback(IDialogContext context, string message)
        {
            await PostMessage(context, message);
            context.Wait(FeedbackReceived);
        }

        // Helper method for displaying response from QnA with feedback
        private async Task ResumeAfterQnA(IDialogContext context, IAwaitable<string> result)
        {
            var resultFromQnA = await result;
            await PostMessage(context, resultFromQnA + "\n\n\n\n");
            context.Wait(this.QnAFeedbackReceived);
        }

        private async Task PostMessage(IDialogContext context, string message)
        {
            var activity = context.MakeMessage();
            activity.Text = message + "Was this answer useful?";
            activity.Type = ActivityTypes.Message;
            activity.TextFormat = TextFormatTypes.Markdown;

            activity.SuggestedActions = new SuggestedActions()
            {
                Actions = new List<CardAction>()
                    {
                        new CardAction(){ Title = "Yes", Type=ActionTypes.ImBack, Value="Yes" },
                        new CardAction(){ Title = "No", Type=ActionTypes.ImBack, Value="No" }
                    }
            };
            await context.PostAsync(activity);
        }
    }
}