using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DVAESABot.Domain;
using DVAESABot.ScheduledHeuristics.Heuristics.Questions;
using Microsoft.Bot.Builder.Dialogs;

namespace DVAESABot.ScheduledHeuristics
{
    public class ScheduledHeuristicsFacade
    {
        private readonly ChatContext _chatContext;
        private Scheduler _scheduler;

        public ScheduledHeuristicsFacade(ChatContext chatContext)
        {
            _chatContext = chatContext;
            var memberTypeHeuristic = new MemberTypeQuestion();
            _scheduler = new Scheduler(new List<IScheduledHeuristic>() {memberTypeHeuristic}, chatContext);
            _scheduler.Run();
        }

        public IHeuristicQnA GetNextQnaPair()
        {
            IEnumerable<IHeuristicQnA> questionAndAnswers = from h in _scheduler.AvailableHeuristics
                where h is IHaveDialogs
                from qnas in ((IHaveDialogs) h).HeuristicQnAs
                select qnas;

            var stillRequired = questionAndAnswers
                .Where(qna => !qna.IsRelevant(_chatContext));

            return stillRequired.FirstOrDefault();
        }
        
    }
}