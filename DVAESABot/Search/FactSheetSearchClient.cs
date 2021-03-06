﻿using DVAESABot.Domain;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace DVAESABot.Search
{
    public class FactSheetSearchClient : IDisposable
    {
        private readonly SearchIndexClient _searchIndexClient;

        public FactSheetSearchClient(string searchApiKey, string searchServiceName, string searchIndexName)
        {
            _searchIndexClient = new SearchIndexClient(searchServiceName, searchIndexName,
                new SearchCredentials(searchApiKey));
        }

        public async Task<DocumentSearchResult<FactSheet>> GetTopMatchingFactsheets(string searchQuery, int topNumber)
        {
            SearchParameters searchParameters = new SearchParameters()
            {
                SearchMode = SearchMode.Any,
                QueryType = QueryType.Simple,
                Top = topNumber
 
            };

            var sw = Stopwatch.StartNew();
            var result = await _searchIndexClient.Documents.SearchAsync<FactSheet>(searchQuery, searchParameters);
            sw.Stop();
            Debug.Print("SEARCH TIME: " + sw.ElapsedMilliseconds);
            return result;            
        }


        public static FactSheetSearchClient CreateDefault()
        {
            return new FactSheetSearchClient(
                ConfigurationManager.AppSettings["SearchServiceQueryApiKey"],
                ConfigurationManager.AppSettings["SearchServiceName"],
                ConfigurationManager.AppSettings["SearchIndexName"]);
        }

        public void Dispose()
        {
            _searchIndexClient?.Dispose();
        }
    }
}