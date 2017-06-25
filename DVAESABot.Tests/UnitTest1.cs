using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DVAESABot.Search;
using System.Diagnostics;

namespace DVAESABot.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void RunSearchQuery()
        {
            var underTest = new FactSheetSearchClient("3B8D7E200F249FC4C1CFA469799348F8", "dvafactsheets", "dvafactsheetsindex");
            var query = "F111 deseal";
            var result = underTest.GetTopMatchingFactsheets(query, 5).Result;
            
            foreach (var r in result.Results)
            {
                Debug.Print(r.Document.FactsheetId + ": " + r.Score);
            }
        }
    }
}
