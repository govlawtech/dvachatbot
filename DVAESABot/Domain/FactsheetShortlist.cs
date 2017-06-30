using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Azure.Search.Models;

namespace DVAESABot.Domain
{
    public class FactsheetShortlist
    {
        public FactsheetShortlist()
        {
            Shortlist = new List<DocumentSearchResult<FactSheet>>();
        }
        public List<DocumentSearchResult<FactSheet>> Shortlist { get; set; }
    }
}