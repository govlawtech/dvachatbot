using System.Collections.Generic;
using System.Linq;
using DVAESABot.Domain;
using DVAESABot.Utilities;

namespace DVAESABot.ScheduledHeuristics
{
    class Scheduler
    {
        
        readonly List<IScheduledHeuristic> _availableHeuristics = new List<IScheduledHeuristic>();
        List<IScheduledHeuristic> _agenda = new List<IScheduledHeuristic>();
        readonly List<IScheduledHeuristic> _firedLog = new List<IScheduledHeuristic>();
        private readonly ChatContext _chatContext;
        private IList<IScheduledHeuristic> _heuristicsRuleBase;

        public Scheduler(IList<IScheduledHeuristic> heuristics, ChatContext chatContext)
        {
            _heuristicsRuleBase = heuristics;
            AvailableHeuristics.AddRange(_heuristicsRuleBase);
            _chatContext = chatContext;
        }

        public List<IScheduledHeuristic> AvailableHeuristics => _availableHeuristics;


        public void Run()
        {
            ActivateHeuristics(_chatContext);
            _agenda = _agenda.OrderByDescending(h => h.Salience).ToList();

            while (_agenda.Count > 0)
            {
                FireRulesOnAgenda();
                ActivateHeuristics(_chatContext);
            }
        }
        private void FireRulesOnAgenda()
        {
            while (_agenda.Count > 0)
            {
                Fire(_agenda.First());
            }
        }

        private void Fire(IScheduledHeuristic heuristic)
        {
            heuristic.Action(_chatContext);
            _firedLog.Add(heuristic);
            _agenda.Remove(heuristic);
        }

        private void ActivateHeuristics(ChatContext chatContext)
        {
            foreach (var h in AvailableHeuristics)
            {
                if (h.Condition(chatContext))
                    _agenda.Add(h);
            }
            foreach (var h in _agenda)
            {
                AvailableHeuristics.Remove(h);
            }
        }
    }
}