using DVAESABot.QnAMaker;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace DVAESABot.Dialogs
{
    /// <summary>
    /// This class handles MRCA QnA processing. Code inspired by IgniteBot.
    /// </summary>
    [Serializable]
    public class QnAMRCADialog : IDialog<string>
    {
        [NonSerialized]
        private QnaMakerKb _qnaMaker;

        public QnAMRCADialog()
        {
            _setup();
        }

        [OnDeserialized]
        internal void _deserialized(StreamingContext context)
        {
            try
            {
                _setup();
            }
            catch (Exception e)
            {
                // XXX: Code from IgniteBot
                var message = e.Message;
            }
        }

        void _setup()
        {
            _qnaMaker = new QnaMakerKb();
        }

        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Great to hear! You can ask a question within this factsheet:");
            context.Wait(MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> item)
        {
            var message = await item;
            var qnaResult = await _qnaMaker.SearchFaqAsync(message.Text);
            string replyContent = "No good match found in the factsheet";

            if (qnaResult == null || qnaResult.Score <= 30 ||
                qnaResult.Answer == "No good match found in the KB")
            {
                // Nothing else to do
            }
            else
            {
                replyContent = qnaResult.Answer;
            }

            context.Done(replyContent);
        }
    }
}