using System;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Microsoft.Azure.Search.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.Azure.Search;
using System.Linq;
using System.Text.RegularExpressions;

namespace DVAESABot.Domain
{
    [Serializable]
    [SerializePropertyNamesAsCamelCase]
    public class FactSheet
    { 
        
        [Key]
        public string Key { get; set; }

        public string Url { get; set; }

        public string FactsheetId { get; set; }

        public string Purpose { get; set; }
        
        public IEnumerable<string> CuratedKeyWords { get; set; }

        public string GetCategoryCode()
        {
            var factsheetCode = Regex.Match(FactsheetId, "\\s[A-Z]+[0-9-]+\\s");
            if (factsheetCode.Success)
            {
                var withNumbersStripped = factsheetCode.Value.Trim().Reverse()
                    .SkipWhile(c => Regex.IsMatch(c.ToString(), "[0-9-]")).Reverse();
                return String.Join("", withNumbersStripped);
            }
            return null;
        }
    }
}
