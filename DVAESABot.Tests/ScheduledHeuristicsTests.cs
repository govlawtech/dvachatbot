using System;
using System.Collections.Generic;
using System.Linq;
using DVAESABot.Domain;
using DVAESABot.ScheduledHeuristics;
using DVAESABot.ScheduledHeuristics.Heuristics;
using DVAESABot.ScheduledHeuristics.Heuristics.Questions;
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
             IList<IScheduledHeuristic> heuristics = new List<IScheduledHeuristic>() {new MemberTypeQuestion()};
            var searchClient = new FactSheetSearchClient("3B8D7E200F249FC4C1CFA469799348F8", "dvafactsheets", "dvafactsheetsindex");
            var searchResults = searchClient
                .GetTopMatchingFactsheets("Ive got mental health issues after deployment", 256).Result;
            var c = ChatContext.CreateEmpty();
            c.FactsheetShortlist = searchResults.Results.Select(r => new FactSheetWithScore(r)).ToList();
            c.User.UserType = UserType.Member;
            int firstRun = c.FactsheetShortlist.Count;
            Console.WriteLine("Number of shortlisted facsheets before heuristic run: " + c.FactsheetShortlist.Count);
            Scheduler scheduler = new Scheduler(heuristics, c);
            scheduler.Run();
            int secondRun = c.FactsheetShortlist.Count;
            Console.WriteLine("Number of shortlisted facsheets after heuristic run: " + c.FactsheetShortlist.Count);
            Assert.IsTrue(secondRun < firstRun);
        }

        [TestMethod]
        public void GetHeuristicQna()
        {
            ChatContext chatContext = ChatContext.CreateEmpty();
            ScheduledHeuristicsFacade scheduledHeuristicsFacade = new ScheduledHeuristicsFacade(chatContext);
            var nextQnaPair = scheduledHeuristicsFacade.GetNextQnaPair();
            var dialog = nextQnaPair.Dialog;
            Assert.IsTrue(dialog != null);
            chatContext.User.UserType = UserType.DependentOnMember;
            var again = scheduledHeuristicsFacade.GetNextQnaPair();
            Assert.IsTrue(again == null);

        }
        
    }



 
}