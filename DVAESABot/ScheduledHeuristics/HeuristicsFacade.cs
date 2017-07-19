using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DVAESABot.Domain;
using DVAESABot.ScheduledHeuristics.Heuristics;

namespace DVAESABot.ScheduledHeuristics
{
    public class HeuristicsFacade
    {
        private List<IScheduledHeuristic> _heuristics;
        public HeuristicsFacade()
        {
            _heuristics = new List<IScheduledHeuristic>()
            {
                new MemberTypeQuestion(),
                new PensionsNotRelevant(),
                new DisabilityPensionNotRelevant(),
                new NoF111Deseal(),
                new NoAgedCareForNonAgedPeople(),
                new NoMCSForMRCAEnlistees()
            };

        }

        public void ApplyHeuristics(ChatContext chatContext)
        {
            var scheduler = new Scheduler(_heuristics, chatContext);
            scheduler.Run();
        }
    }
}