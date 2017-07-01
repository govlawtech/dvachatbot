using System;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Microsoft.Azure.Search.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.Azure.Search;
using System.Linq;

namespace DVAESABot.Domain
{
    [SerializePropertyNamesAsCamelCase]
    public class FactSheet
    { 
        
        [Key]
        public string Key { get; set; }

        public string Url { get; set; }

        public string FactsheetId { get; set; }

        public string Purpose { get; set; }
        
        public IEnumerable<string> CuratedKeyWords { get; set; }
    }
}
