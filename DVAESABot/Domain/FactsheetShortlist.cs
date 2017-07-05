using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Microsoft.Azure.Search.Models;

namespace DVAESABot.Domain
{
   
    public static class FactsheetShortlistExensions
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

        public static List<FactSheetWithScore> RemoveAllExceptWithKeyWords(this List<FactSheetWithScore> factSheetWithScores, params string[] keywords)
        {
            var cutDown = from doc in factSheetWithScores
                where doc.FactSheet.CuratedKeyWords.Any(akw => keywords.ToList().Contains(akw))
                select doc;

            return cutDown.ToList();
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
}