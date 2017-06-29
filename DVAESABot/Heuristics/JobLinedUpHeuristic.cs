using System.Collections.Generic;
using System.Linq;

namespace DVAESABot.Heuristics
{
    /// <summary>
    /// The heuristic that checks if the user has a job lined up.
    /// </summary>
    public class JobLinedUpHeuristic : IHeuristic
    {
        private List<string> appliesTo;

        public JobLinedUpHeuristic()
        {
            // All IS factsheets
            appliesTo = new List<string>();
            appliesTo.Add("IS01");
            appliesTo.Add("IS02");
            appliesTo.Add("IS03");
            appliesTo.Add("IS04");
            appliesTo.Add("IS05");
            appliesTo.Add("IS06");
            appliesTo.Add("IS07");
            appliesTo.Add("IS08");
            appliesTo.Add("IS09");
            appliesTo.Add("IS10");
            appliesTo.Add("IS101");
            appliesTo.Add("IS103");
            appliesTo.Add("IS104");
            appliesTo.Add("IS105");
            appliesTo.Add("IS106");
            appliesTo.Add("IS115");
            appliesTo.Add("IS116");
            appliesTo.Add("IS117");
            appliesTo.Add("IS12");
            appliesTo.Add("IS121");
            appliesTo.Add("IS122");
            appliesTo.Add("IS125");
            appliesTo.Add("IS126");
            appliesTo.Add("IS135");
            appliesTo.Add("IS137");
            appliesTo.Add("IS138");
            appliesTo.Add("IS139");
            appliesTo.Add("IS140");
            appliesTo.Add("IS141");
            appliesTo.Add("IS142");
            appliesTo.Add("IS143");
            appliesTo.Add("IS144");
            appliesTo.Add("IS145");
            appliesTo.Add("IS147");
            appliesTo.Add("IS15");
            appliesTo.Add("IS150");
            appliesTo.Add("IS151");
            appliesTo.Add("IS154");
            appliesTo.Add("IS155");
            appliesTo.Add("IS156");
            appliesTo.Add("IS158");
            appliesTo.Add("IS159");
            appliesTo.Add("IS16");
            appliesTo.Add("IS160");
            appliesTo.Add("IS161");
            appliesTo.Add("IS163");
            appliesTo.Add("IS164");
            appliesTo.Add("IS165");
            appliesTo.Add("IS166");
            appliesTo.Add("IS167");
            appliesTo.Add("IS168");
            appliesTo.Add("IS18");
            appliesTo.Add("IS184");
            appliesTo.Add("IS185");
            appliesTo.Add("IS186");
            appliesTo.Add("IS187");
            appliesTo.Add("IS188");
            appliesTo.Add("IS19");
            appliesTo.Add("IS29");
            appliesTo.Add("IS30");
            appliesTo.Add("IS34");
            appliesTo.Add("IS35");
            appliesTo.Add("IS44");
            appliesTo.Add("IS45");
            appliesTo.Add("IS46");
            appliesTo.Add("IS47");
            appliesTo.Add("IS48");
            appliesTo.Add("IS50");
            appliesTo.Add("IS57");
            appliesTo.Add("IS58");
            appliesTo.Add("IS65");
            appliesTo.Add("IS71");
            appliesTo.Add("IS72");
            appliesTo.Add("IS73");
            appliesTo.Add("IS74");
            appliesTo.Add("IS75");
            appliesTo.Add("IS77");
            appliesTo.Add("IS79");
            appliesTo.Add("IS81");
            appliesTo.Add("IS82");
            appliesTo.Add("IS85");
            appliesTo.Add("IS86");
            appliesTo.Add("IS87");
            appliesTo.Add("IS88");
            appliesTo.Add("IS89");
            appliesTo.Add("IS90");
            appliesTo.Add("IS91");
            appliesTo.Add("IS92");
            appliesTo.Add("IS94");
            appliesTo.Add("IS95");
            appliesTo.Add("IS96");
            appliesTo.Add("IS97");
            appliesTo.Add("IS98");
            appliesTo.Add("IS99");
        }

        public List<string> applyHeuristic(bool userResponse, List<string> factsheets)
        {
            if (!userResponse)
            {
                // User is not transitioning, remove IS factsheets
                return factsheets.Except(appliesTo).ToList();
            }
            return factsheets;
        }

        public string defaultQuestion()
        {
            return "Are you transitioning out but don’t have a job lined up?";
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