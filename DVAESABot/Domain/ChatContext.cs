using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Azure.Search.Models;

namespace DVAESABot.Domain
{
    [Serializable]
    [SerializePropertyNamesAsCamelCase]
    public class ChatContext
    {
        public ChatContext()
        {
            
        }

        public static ChatContext CreateEmpty()
        {
            return new ChatContext(new User(), new List<FactSheetWithScore>(),new Selections());
        }
        
        public ChatContext(User user, List<FactSheetWithScore> factsheetShortlist, Selections selections)
        {
            User = user;
            FactsheetShortlist = factsheetShortlist;
            Selections = selections;
        }
        
        public User User { get; set; }
        public List<FactSheetWithScore> FactsheetShortlist { get; set; }
        public Selections Selections { get; set; }

    }
}