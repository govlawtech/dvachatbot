using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DVAESABot.Domain
{
    public class ChatContext
    {
        public ChatContext()
        {
            User = new User();
            FactsheetShortlist = new FactsheetShortlist();
        }

        public User User { get; set; }
        public FactsheetShortlist FactsheetShortlist { get; set; }

    }
}