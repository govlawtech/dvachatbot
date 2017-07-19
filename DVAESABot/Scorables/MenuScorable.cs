using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DVAESABot.Dialogs;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Builder.Internals.Fibers;
using Microsoft.Bot.Builder.Scorables.Internals;
using Microsoft.Bot.Connector;

namespace DVAESABot.Scorables
{
    public class MenuScorable : ScorableBase<IActivity, string, double>
    {
        private readonly IDialogTask task;

        public MenuScorable(IDialogTask task)
        {
            SetField.NotNull(out this.task, nameof(task),task);
        }

        protected override Task DoneAsync(IActivity item, string state, CancellationToken token)
        {
            return Task.CompletedTask;
        }

        protected override async Task<string> PrepareAsync(IActivity item, CancellationToken token)
        {
            var triggers = new[] {"menu", "cancel","help","stop","go back","back"};

            var message = item as IMessageActivity;
            if (message != null && !string.IsNullOrWhiteSpace(message.Text))
            {
                if (triggers.Any(t => t == message.Text.Trim().ToLowerInvariant()))
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
            return 1.0;
        }

        protected override async Task PostAsync(IActivity item, string state, CancellationToken token)
        {
            var message = item as IMessageActivity;
            if (message != null)
            {
                var globalMenuDialog = new GlobalMenuDialog();
                var interruption = globalMenuDialog.Void<MenuOptionChosen, IMessageActivity>();

                this.task.Call(interruption, null);

                await task.PollAsync(token);
            }
        }
               
    }
}