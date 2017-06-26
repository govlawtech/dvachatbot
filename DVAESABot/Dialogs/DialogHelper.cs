using DVAESABot.QnaMaker;
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
        public static async Task PostMessage(IDialogContext context, string message)
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

        // Parses the Factsheet Code and Knowledge Base ID from the intentName and stores it for the session
        public static void StoreKbDetails(IDialogContext context, string intentName)
        {
            string factsheetCode = null;
            if (intentName.StartsWith("factsheet", StringComparison.OrdinalIgnoreCase))
            {
                // Comes from Azure Search, e.g. "Factsheet MRC04 - ...."
                intentName = intentName.Substring(intentName.IndexOf(" ") + 1);
            }
            factsheetCode = intentName.Substring(0, intentName.IndexOf(" "));
            context.UserData.SetValue<string>("FactsheetCode", factsheetCode);
            context.UserData.SetValue<string>("KbId", GetKbIdFromFactsheetCode(factsheetCode));
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