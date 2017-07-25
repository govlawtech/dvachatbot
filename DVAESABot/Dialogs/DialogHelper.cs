using DVAESABot.QnaMaker;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Chronic.Tags.Repeaters;

namespace DVAESABot.Dialogs
{
    /// <summary>
    /// Helper methods for maximising reuse
    /// </summary>
    public class DialogHelper
    {

        // Helper method for extracting 'Factsheet XXXNN - ' and just return the text after the '-'
        public static string ExtractTopicFromFactSheetTitle(string factSheetTitle)
        {
            var stripped = Regex.Replace(factSheetTitle, "Factsheet [A-Z0-9-]+ -?", "").Trim();
            return stripped;
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