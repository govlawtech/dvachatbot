using System.Collections.Generic;
using System.Linq;

namespace DVAESABot.Heuristics
{
    /// <summary>
    /// The heuristic that checks if the user is a member.
    /// </summary>
    public class IsMemberHeuristic : IHeuristic
    {
        private List<string> appliesTo;

        public IsMemberHeuristic()
        {
            // All MRC factsheets, except MRC17 and MRC18
            appliesTo = new List<string>();
            appliesTo.Add("MRC01");
            appliesTo.Add("MRC04");
            appliesTo.Add("MRC05");
            appliesTo.Add("MRC07");
            appliesTo.Add("MRC08");
            appliesTo.Add("MRC09");
            appliesTo.Add("MRC10");
            appliesTo.Add("MRC25");
            appliesTo.Add("MRC27");
            appliesTo.Add("MRC30");
            appliesTo.Add("MRC31");
            appliesTo.Add("MRC33");
            appliesTo.Add("MRC34");
            appliesTo.Add("MRC35");
            appliesTo.Add("MRC36");
            appliesTo.Add("MRC39");
            appliesTo.Add("MRC40");
            appliesTo.Add("MRC41");
            appliesTo.Add("MRC42");
            appliesTo.Add("MRC43");
            appliesTo.Add("MRC45");
            appliesTo.Add("MRC47");
            appliesTo.Add("MRC49");
            appliesTo.Add("MRC50");
        }

        public List<string> applyHeuristic(bool userResponse, List<string> factsheets)
        {
            if (!userResponse)
            {
                // User is not member, remove MRC factsheets
                return factsheets.Except(appliesTo).ToList();
            }
            return factsheets;
        }

        public string defaultQuestion()
        {
            return "Are you a member or former member? (Includes Permanent Force, Reservist, Peacekeeper, Commonwealth or Allied Veteran, Mariner, Civilian, Member)";
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