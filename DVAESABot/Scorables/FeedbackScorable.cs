using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Microsoft.ApplicationInsights;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Builder.Internals.Fibers;
using Microsoft.Bot.Builder.Scorables.Internals;
using Microsoft.Bot.Connector;

namespace DVAESABot.Scorables
{
    public class FeedbackScorable : ScorableBase<IActivity, string, double>
    {
        private readonly IDialogTask task;
        private static List<string> FEEDBACK_PHRASES = new List<string> { "feedback: ", "feedback" };

        public FeedbackScorable(IDialogTask task)
        {
            SetField.NotNull(out this.task, nameof(task), task);
        }

        protected override async Task<string> PrepareAsync(IActivity activity, CancellationToken token)
        {
            var message = activity as IMessageActivity;

            if (message != null && !string.IsNullOrWhiteSpace(message.Text))
            {
                if (FEEDBACK_PHRASES.Any(p => message.Text.ToLowerInvariant().StartsWith(p)))
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
            return 0.8;
        }

        protected override async Task PostAsync(IActivity item, string state, CancellationToken token)
        {
            // no op as everything logged anyway
        }
        protected override Task DoneAsync(IActivity item, string state, CancellationToken token)
        {
            return Task.CompletedTask;
        }
    }
}
