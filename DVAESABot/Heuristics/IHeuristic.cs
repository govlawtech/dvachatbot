using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVAESABot.Heuristics
{
    interface IHeuristic
    {
        /// <returns>List of Factsheet Codes that apply to this heuristic</returns>
        List<string> getFactsheetCodesToWhichHeuristicApplies();

        /// <param name="userResponse">The bool user response</param>
        /// <param name="factsheets">The List of Factsheet Codes before applying the heuristic </param>
        /// <returns>The resultant List of Factsheet Codes after applying the heuristic</returns>
        List<string> applyHeuristic(bool userResponse, List<string> factsheets);

        /// <param name="factsheets">The List of Factsheet Codes to check</param>
        /// <returns>true if this heuristic applies to the input List of Factsheet Codes</returns>
        bool isApplies(List<string> factsheets);

        /// <returns>The default question for this heuristic to ask</returns>
        string defaultQuestion();

        /// <returns>Common name for this heuristic, can be used as a unique key</returns>
        string name();
    }
}
