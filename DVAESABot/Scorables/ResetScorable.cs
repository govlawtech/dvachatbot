using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Builder.Internals.Fibers;
using Microsoft.Bot.Builder.Scorables.Internals;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DVAESABot.Scorables
{
    // Allows users to reset the conversation
    public class ResetScorable : ScorableBase<IActivity, string, double>
    {
        private readonly IDialogTask task;
        private static List<string> RESET_PHRASES = new List<string> { "start over", "reset", "restart" };

        public ResetScorable(IDialogTask task)
        {
            SetField.NotNull(out this.task, nameof(task), task);
        }

        protected override async Task<string> PrepareAsync(IActivity activity, CancellationToken token)
        {
            var message = activity as IMessageActivity;

            if (message != null && !string.IsNullOrWhiteSpace(message.Text))
            {
                if (RESET_PHRASES.Contains(message.Text, StringComparer.OrdinalIgnoreCase))
                {
                    return message.Text;
                }
            }

            return null;
        }

        protected override bool HasScore(IActivity item, string state)
        {
            return state != null;
        }

        protected override double GetScore(IActivity item, string state)
        {
            return 0.9;
        }

        protected override async Task PostAsync(IActivity item, string state, CancellationToken token)
        {
            this.task.Reset();
        }
        protected override Task DoneAsync(IActivity item, string state, CancellationToken token)
        {
            return Task.CompletedTask;
        }
    }
}