using DVAESABot.QnaMaker;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Chronic.Tags.Repeaters;

namespace DVAESABot.Dialogs
{
    /// <summary>
    /// Helper methods for maximising reuse
    /// </summary>
    public class DialogHelper
    {
        // Key for storing shortlisted factsheets, use the getters and setters in this class
        private const string SHORTLISTED_FACTSHEETS = "ShortlistedFactsheets";

        public const string FACTSHEET_NAME = "FactsheetName";
        public const string KB_ID = "KbId";

        public static List<string> POSITIVE_RESPONSES = new List<string> { "yes", "y" };

        public static async Task PostMessageWithFeedback(IDialogContext context, string message)
        {
            await PostMessage(context, message + "\n\n\n\nWas this answer useful?");
        }

        public static async Task PostMessage(IDialogContext context, string message)
        {
            var activity = context.MakeMessage();
            activity.Text = message;
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

        // Parses the Factsheet Code from the intentName, and shortists it for this user
        public static void AddFactsheetCodeToShortlist(IDialogContext context, string intentName)
        {
            string factsheetCode = ExtractFactsheetCodeFromFactSheeTitle(intentName);

            List<string> shortlist = context.UserData.GetValueOrDefault<List<string>>(SHORTLISTED_FACTSHEETS, new List<string>());
            shortlist.Add(factsheetCode);
            context.UserData.SetValue<List<string>>(SHORTLISTED_FACTSHEETS, shortlist);
        }

        // Returns the list of shortlisted factsheets
        public static List<string> GetFactsheetShortlist(IDialogContext context)
        {
            return context.UserData.GetValueOrDefault<List<string>>(SHORTLISTED_FACTSHEETS, new List<string>());
        }

        // Helper method to reset the shortlist
        public static void SetFactsheetShortlist(IDialogContext context, List<String> factsheets)
        {
            context.UserData.SetValue<List<string>>(DialogHelper.SHORTLISTED_FACTSHEETS, factsheets);
        }

        // Parses the Factsheet Code and Knowledge Base ID from the intentName and stores it for the session
        public static void StoreKbDetails(IDialogContext context, string intentName)
        {
            context.UserData.SetValue<string>(FACTSHEET_NAME, ExtractTopicFromFactSheetTitle(intentName));
            context.UserData.SetValue<string>(KB_ID, GetKbIdFromFactsheetCode(ExtractFactsheetCodeFromFactSheeTitle(intentName)));
        }

        // Helper method for extracting 'Factsheet XXXNN - ' and just return the text after the '-'
        public static string ExtractTopicFromFactSheetTitle(string factSheetTitle)
        {
            return factSheetTitle.Substring(factSheetTitle.IndexOf(" - ") + 3);
        }

        public static string GetWrappedFactsheetTitle(string fullFactSheetTitle, int cols)
        {
            var shortTitle = ExtractTopicFromFactSheetTitle(fullFactSheetTitle);
            var wrapped = Wrap(shortTitle, cols);
            return String.Join(" ", wrapped);
        }

        // Helper method for extracting the Factsheet Code from the intentName
        public static string ExtractFactsheetCodeFromFactSheeTitle(string factSheetTitle)
        {
            string factsheetCode = null;
            if (factSheetTitle.StartsWith("factsheet", StringComparison.OrdinalIgnoreCase))
            {
                // Comes from Azure Search, e.g. "Factsheet MRC04 - ...."
                factSheetTitle = factSheetTitle.Substring(factSheetTitle.IndexOf(" ") + 1);
            }
            factsheetCode = factSheetTitle.Substring(0, factSheetTitle.IndexOf(" "));

            return factsheetCode;
        }

        // Maps Factsheet Code (e.g. MRC04) to the Knowledge Base ID
        public static string GetKbIdFromFactsheetCode(string factsheetCode)
        {
            if (KbId.kbIDs.ContainsKey(factsheetCode))
            {
                return KbId.kbIDs[factsheetCode];
            }
            else
            {
                throw new ArgumentException($"No QnA KB id for factsheet code: " + factsheetCode);
            }
        }

        public static string Wrap(string text, int lineWidth)
        {
            return string.Join(string.Empty,
                Wrap(
                    text.Split(new char[0],
                        StringSplitOptions
                            .RemoveEmptyEntries),
                    lineWidth));
        }

        public static IEnumerable<string> Wrap(IEnumerable<string> words,
            int lineWidth)
        {
            var currentWidth = 0;
            foreach (var word in words)
            {
                if (currentWidth != 0)
                {
                    if (currentWidth + word.Length < lineWidth)
                    {
                        currentWidth++;
                        yield return " ";
                    }
                    else
                    {
                        currentWidth = 0;
                        yield return Environment.NewLine;
                    }
                }
                currentWidth += word.Length;
                yield return word;
            }
        }
    }
}