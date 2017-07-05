using System;
using AdaptiveCards;
using Microsoft.Azure.Search.Models;

namespace DVAESABot.Domain
{
    [SerializePropertyNamesAsCamelCase]
    [Serializable]
    public class FactSheetWithScore 
    {
        public FactSheetWithScore()
        {
            
        }
        public FactSheet FactSheet { get; set; }
        public double? Score { get; set; }

        public FactSheetWithScore(SearchResult<FactSheet> searchResult)
        {
            FactSheet = searchResult.Document;
            Score = searchResult.Score;
        }
    }
}