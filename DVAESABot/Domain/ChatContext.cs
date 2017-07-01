using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Azure.Search.Models;

namespace DVAESABot.Domain
{
    public class ChatContext
    {
        public ChatContext(User user, FactsheetShortlist factsheetShortlist)
        {
            User = user;
            FactsheetShortlist = factsheetShortlist;
        }

        public ChatContext()
        {
            User = new User();
            FactsheetShortlist = new FactsheetShortlist(new List<SearchResult<FactSheet>>());
        }

        public User User { get; set; }
        public FactsheetShortlist FactsheetShortlist { get; set; }

    }
}