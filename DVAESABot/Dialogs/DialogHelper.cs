﻿using DVAESABot.QnaMaker;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;

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
            string factsheetCode = _ExtractFactsheetCodeFromIntent(ref intentName);

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
            context.UserData.SetValue<string>(FACTSHEET_NAME, ExtractFactsheetTitleFromIntent(ref intentName));
            context.UserData.SetValue<string>(KB_ID, GetKbIdFromFactsheetCode(_ExtractFactsheetCodeFromIntent(ref intentName)));
        }


        // Helper method for extracting 'Factsheet XXXNN - ' and just return the text after the '-'
        public static string ExtractFactsheetTitleFromIntent(ref string intentName)
        {
            return intentName.Substring(intentName.IndexOf(" - ") + 3);
        }

        // Helper method for extracting the Factsheet Code from the intentName
        private static string _ExtractFactsheetCodeFromIntent(ref string intentName)
        {
            string factsheetCode = null;
            if (intentName.StartsWith("factsheet", StringComparison.OrdinalIgnoreCase))
            {
                // Comes from Azure Search, e.g. "Factsheet MRC04 - ...."
                intentName = intentName.Substring(intentName.IndexOf(" ") + 1);
            }
            factsheetCode = intentName.Substring(0, intentName.IndexOf(" "));

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
                // Default one with all questions
                return ConfigurationManager.AppSettings["QnaKnowledgeBaseId"];
            }
        }
    }
}