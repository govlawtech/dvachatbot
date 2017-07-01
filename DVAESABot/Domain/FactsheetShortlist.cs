using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Microsoft.Azure.Search.Models;

namespace DVAESABot.Domain
{
    public class FactsheetShortlist
    {
        public FactsheetShortlist(IList<SearchResult<FactSheet>> initial)
        {
            Shortlist = initial;
        }
        public IList<SearchResult<FactSheet>> Shortlist { get; private set; }

        public void RemoveAllCategoriesOtherThan(params string[] categoryCodes)
        {
            var cutDownList = from doc in Shortlist
                where categoryCodes.ToList().Any(cc => doc.Document.IsCategory(cc))
                select doc;

            Shortlist = cutDownList.ToList();
        }

        public void RemoveCategories(params string[] categoryCodes)
        {
            var cutDown = from doc in Shortlist
                where !categoryCodes.ToList().Any(cc => doc.Document.IsCategory(cc))
                select doc;
            
            Shortlist = cutDown.ToList();
        }

        public void RemoveAllExceptWithKeyWords(params string[] keywords)
        {

            var cutDown = from doc in Shortlist
                where doc.Document.CuratedKeyWords.Any(akw => keywords.ToList().Contains(akw))
                select doc;

            Shortlist = cutDown.ToList();

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