using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DVAESABot.Heuristics
{
    /// <summary>
    /// The heuristic that checks if the user enlisted before 1 July 2004.
    /// </summary>
    public class EnlistBefore1July2004Heuristic : IHeuristic
    {
        private List<string> appliesTo;

        public EnlistBefore1July2004Heuristic()
        {
            // All DP and MCS factsheets
            appliesTo = new List<string>();
            appliesTo.Add("DP01");
            appliesTo.Add("DP02");
            appliesTo.Add("DP13");
            appliesTo.Add("DP15");
            appliesTo.Add("DP18");
            appliesTo.Add("DP22");
            appliesTo.Add("DP23");
            appliesTo.Add("DP28");
            appliesTo.Add("DP29");
            appliesTo.Add("DP30");
            appliesTo.Add("DP42");
            appliesTo.Add("DP43");
            appliesTo.Add("DP50");
            appliesTo.Add("DP60");
            appliesTo.Add("DP68");
            appliesTo.Add("DP71");
            appliesTo.Add("DP72");
            appliesTo.Add("DP73");
            appliesTo.Add("DP74");
            appliesTo.Add("DP75");
            appliesTo.Add("DP76");
            appliesTo.Add("DP78");
            appliesTo.Add("DP79");
            appliesTo.Add("DP81");
            appliesTo.Add("DP82");
            appliesTo.Add("DP83");
            appliesTo.Add("DP84");
            appliesTo.Add("DP85");
            appliesTo.Add("MCS01");
            appliesTo.Add("MCS07");
            appliesTo.Add("MCS13");
        }

        public List<string> applyHeuristic(bool userResponse, List<string> factsheets)
        {
            if (!userResponse)
            {
                // User is not transitioning, remove DP and MCS factsheets
                return factsheets.Except(appliesTo).ToList();
            }
            return factsheets;
        }

        public string defaultQuestion()
        {
            return "Did you enlist before 1 July 2004?";
        }

        public List<string> getFactsheetCodesToWhichHeuristicApplies()
        {
            return appliesTo;
        }

        public bool isApplies(List<string> factsheets)
        {
            // Return true if any matches
            return appliesTo.Any(x => factsheets.Any(y => y == x));
        }

        public string name()
        {
            return this.GetType().Name;
        }
    }
}