using System;
using System.Collections.Generic;
using System.Linq;
using DVAESABot.Domain;
using DVAESABot.ScheduledHeuristics;
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
        public void BasicTest()
        {
            IList<IScheduledHeuristic> heuristics = new List<IScheduledHeuristic>() {new MrcaHeuristic()};
            var c = new ChatContext();
            Scheduler scheduler = new Scheduler(heuristics, c);
            scheduler.Run();
            Assert.IsTrue(c.User.IsMRCAEnlistee.Tag == OptionType.None);

            c.User.EnlistmentDate = Option.Some(new LocalDate(2010, 1, 1));
            c.User.UserType = Option.Some(UserType.Member);

            scheduler.Run();
            c.User.IsMRCAEnlistee.MatchSome(out bool shouldBeTrue);
            Assert.IsTrue(shouldBeTrue);
        }


        [TestMethod]
        public void MemberTypeHeuristicTest()
        {
             IList<IScheduledHeuristic> heuristics = new List<IScheduledHeuristic>() {new MemberTypeHeuristic()};
            var searchClient = new FactSheetSearchClient("3B8D7E200F249FC4C1CFA469799348F8", "dvafactsheets", "dvafactsheetsindex");
            var searchResults = searchClient.GetTopMatchingFactsheets("Ive got mental health issues after deployment", 50);
            var c = new ChatContext();
            c.FactsheetShortlist = new FactsheetShortlist(searchResults.Result.Results);
            c.User.UserType = Option.Some(UserType.Member);
            Console.WriteLine("Number of shortlisted facsheets before heuristic run: " + c.FactsheetShortlist.Shortlist.Count);
            Scheduler scheduler = new Scheduler(heuristics, c);
            scheduler.Run();

            Console.WriteLine("Number of shortlisted facsheets after heuristic run: " + c.FactsheetShortlist.Shortlist.Count);
            
        }
    }

    class MemberTypeHeuristic : IScheduledHeuristic
    {
        public string Description => "Member Type";

        public int Salience => 200;

        public Predicate<ChatContext> Condition => c => c.User.UserType.Tag == OptionType.Some;

        public Action<ChatContext> Action => c =>
        {
            if (c.User.UserType.MatchSome(out UserType userType))
            {
                switch (userType)
                {
                    case UserType.Member:
                    {
                        c.FactsheetShortlist.RemoveCategories("GS","HIP");
                        break;
                    }
                    case UserType.DependentOnDeceasedMember:
                    {
                        c.FactsheetShortlist.RemoveAllExceptWithKeyWords("Dependent","Defacto","Bereavement");
                        break;
                    }
                    case UserType.DependentOnMember:
                    {
                        c.FactsheetShortlist.RemoveAllExceptWithKeyWords("Dependent", "Children");
                        break;
                    }
                    case UserType.Organisation:
                    {
                        c.FactsheetShortlist.RemoveAllCategoriesOtherThan("HIP","GS","IP","FIP","DVA");
                        break;
                    }
                }
            }
        };
    }

    class MrcaHeuristic : IScheduledHeuristic
    {
        public string Description => "Post MRCA Enlistment";
        public int Salience => 100;
        public Predicate<ChatContext> Condition => c =>
        {
            if (c.User.UserType.MatchSome(out UserType userType))
            {
                if (userType == UserType.Member)
                {
                    if (c.User.EnlistmentDate.MatchSome(out LocalDate enlistDate))
                    {
                        return enlistDate.CompareTo(new LocalDate(2004,7,1)) >= 0;
                    }
                }
            }
            return false;
        };  

        public Action<ChatContext> Action => c =>
        {
            c.User.IsMRCAEnlistee = Option.Some(true);
        };
    }
}