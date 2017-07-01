using System;
using System.Collections.Generic;
using System.Linq;
using DVAESABot.Domain;
using DVAESABot.ScheduledHeuristics;
using DVAESABot.ScheduledHeuristics.Heuristics;
using DVAESABot.Search;
using DVAESABot.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NodaTime;

namespace DVAESABot.Tests
{
    [TestClass]
    public class ScheduledHeuristicsTests
    {
       
        [TestMethod]
        public void MemberTypeHeuristicTest()
        {
             IList<IScheduledHeuristic> heuristics = new List<IScheduledHeuristic>() {new MemberTypeHeuristic()};
            var searchClient = new FactSheetSearchClient("3B8D7E200F249FC4C1CFA469799348F8", "dvafactsheets", "dvafactsheetsindex");
            var searchResults = searchClient
                .GetTopMatchingFactsheets("Ive got mental health issues after deployment", 256).Result;
            var c = new ChatContext();
            c.FactsheetShortlist = new FactsheetShortlist(searchResults.Results);
            c.User.UserType = Option.Some(UserType.Member);
            int firstRun = c.FactsheetShortlist.Shortlist.Count;
            Console.WriteLine("Number of shortlisted facsheets before heuristic run: " + c.FactsheetShortlist.Shortlist.Count);
            Scheduler scheduler = new Scheduler(heuristics, c);
            scheduler.Run();
            int secondRun = c.FactsheetShortlist.Shortlist.Count;
            Console.WriteLine("Number of shortlisted facsheets after heuristic run: " + c.FactsheetShortlist.Shortlist.Count);
            Assert.IsTrue(secondRun < firstRun);
        }
    }



 
}