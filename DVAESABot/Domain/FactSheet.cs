using System;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Microsoft.Azure.Search.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.Azure.Search;
using System.Linq;

namespace SearchExperiment
{
    [SerializePropertyNamesAsCamelCase]
    public class FactSheet
    {
        
        [Key]
        public string Key { get; set; }

        public string FactsheetId { get; set; }

        [IsSearchable]
        public string Purpose { get; set; }
             
    

    }
}
