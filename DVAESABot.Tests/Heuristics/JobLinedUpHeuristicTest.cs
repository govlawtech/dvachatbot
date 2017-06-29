using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using DVAESABot.Heuristics;

namespace DVAESABot.Tests.Heuristics
{
    [TestClass]
    public class JobLinedUpHeuristicTest
    {
        [TestMethod]
        public void TestIsAppliesOneMatching()
        {
            List<string> oneValue = new List<string>();
            oneValue.Add("IS94");
            oneValue.Add("CON05");

            JobLinedUpHeuristic heuristic = new JobLinedUpHeuristic();
            Assert.IsTrue(heuristic.isApplies(oneValue));
        }
    }
}
