using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using AdaptiveCards;
using Microsoft.Azure.Search.Models;
using Microsoft.Bot.Builder.Dialogs;

namespace DVAESABot.Domain
{
   
    public static class FactsheetShortlistExtensions
    {
     
        public static List<FactSheetWithScore> RemoveAllCategoriesOtherThan(this List<FactSheetWithScore> factSheetWithScores, params string[] categoryCodes)
        {
            var cutDownList = from doc in factSheetWithScores
                where categoryCodes.ToList().Any(cc => doc.FactSheet.IsCategory(cc))
                select doc;

            return cutDownList.ToList();
        }

        public static List<FactSheetWithScore> RemoveCategories(this List<FactSheetWithScore> factSheetWithScores, params string[] categoryCodes)
        {
            var cutDown = from doc in factSheetWithScores
                where !categoryCodes.ToList().Any(cc => doc.FactSheet.IsCategory(cc))
                select doc;

            return cutDown.ToList();
        }

        public static void DropFactsheetWithTitle(this List<FactSheetWithScore> factSheetsWithScores, string title)
        {
            var toDrop = factSheetsWithScores.SingleOrDefault(f => f.FactSheet.FactsheetId == title);
            if (toDrop != null)
                factSheetsWithScores.Remove(toDrop);
        }

        public static List<FactSheetWithScore> RemoveAllExceptWithKeyWords(this List<FactSheetWithScore> factSheetWithScores, params string[] keywords)
        {
            var cutDown = from doc in factSheetWithScores
                where doc.FactSheet.CuratedKeyWords.Any(akw => keywords.ToList().Contains(akw))
                select doc;

            return cutDown.ToList();
        }
        
        public static IEnumerable<string> GetCategories(this List<FactSheetWithScore> factSheetWithScores)
        {
            return factSheetWithScores.Select(fs => fs.FactSheet.GetCategoryCode()).ToList();
        }
    }

    public static class FactsheetExtensions
    {
        public static bool IsCategory(this FactSheet factsheet, string categoryCode)
        {
            var factsheetCode = Regex.Match(factsheet.FactsheetId, "\\s[A-Z]+[0-9]+\\s");
            if (!factsheetCode.Success) return false;

            return factsheetCode.Value.Contains(categoryCode);
        }
    }

    public static class DialogContextExtensions
    {
        public static ChatContext GetChatContextOrDefault(this IDialogContext dialogContext)
        {
            dialogContext.UserData.TryGetValue(typeof(ChatContext).Name, out ChatContext cc);
            return cc;
        }

        public static void SetChatContext(this IDialogContext dialogContext, ChatContext chatContext)
        {
            dialogContext.UserData.SetValue(typeof(ChatContext).Name,chatContext);
        }
    }

    
}