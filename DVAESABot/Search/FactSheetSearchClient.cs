using DVAESABot.Domain;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace DVAESABot.Search
{
    class FactSheetSearchClient
    {
        private readonly SearchIndexClient _searchIndexClient;

        public FactSheetSearchClient()
        {
            string searchApiKey = ConfigurationManager.AppSettings["SearchServiceQueryApiKey"];  // ok to be public
            string searchServiceName = ConfigurationManager.AppSettings["SearchServiceName"];
            string searchIndexName = ConfigurationManager.AppSettings["SearchIndexName"];
            _searchIndexClient = new SearchIndexClient(searchServiceName, searchServiceName,
                new SearchCredentials(searchApiKey));
                            
        }


        public async Task<IList<string>> GetTopMatchingFactsheetIds(string searchQuery, int topNumber)
        {
            SearchParameters searchParameters = new SearchParameters()
            {
                SearchMode = SearchMode.Any,
                QueryType = QueryType.Simple,
                Top = topNumber
            };

            _searchIndexClient.Documents.Search<FactSheet>(searchQuery,)

            //_searchIndexClient.
        }
        

    }
}