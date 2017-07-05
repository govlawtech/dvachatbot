using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Azure.Search.Models;

namespace DVAESABot.Domain
{
    [Serializable]
    [SerializePropertyNamesAsCamelCase]
    public class Selections
    {
        public List<string> FactsheetTitlesSelected { get; set; }
    }
}