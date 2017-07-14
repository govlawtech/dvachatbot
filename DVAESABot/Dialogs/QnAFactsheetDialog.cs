using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using DVAESABot.QnaMaker;
using DVAESABot.QnAMaker;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace DVAESABot.Dialogs
{
    [Serializable]
    public class QnAFactsheetDialog : IDialog<string>
    {
        private static readonly List<string> FailureMessagesSelection = new List<string>
        {
            "Don't have an answer to that within this topic specifically."
        };

        [NonSerialized] private static readonly Random _random = new Random();
        private readonly string _factSheetCode;

        private readonly string _factSheetTitle;
        private readonly string _factSheetUrl;
        private readonly string _kbId;


        [NonSerialized] private QnaMakerClient _qnaMaker;

        public QnAFactsheetDialog()
        {
        }

        public QnAFactsheetDialog(string factSheetTitle, string factSheetUrl)
        {
            _factSheetTitle = factSheetTitle;
            _factSheetUrl = factSheetUrl;
            _factSheetCode = DialogHelper.ExtractFactsheetCodeFromFactSheeTitle(factSheetTitle);
            _kbId = KbId.kbIDs[_factSheetCode];
            _qnaMaker = new QnaMakerClient();
        }

        public async Task StartAsync(IDialogContext context)
        {
            
            var questionsAndAnswers = await _qnaMaker.GetQuestionsAndAnswers(_kbId);

            if (questionsAndAnswers.Any())
            {
                if (questionsAndAnswers.ContainsKey("Purpose"))
                {
                    var outlineMessage = context.MakeMessage();
                    outlineMessage.TextFormat = "plain";
                    outlineMessage.Text = RemoveFactsheetReferenceFromPurposeSection(questionsAndAnswers["Purpose"]);
                    await context.PostAsync(outlineMessage);
                }
                
                IEnumerable<string> qsInKb = questionsAndAnswers.Keys.Where(q => q != "Purpose").ToList();

                PromptDialog.Choice(
                    context,
                    QnAQuestionReceived,
                    qsInKb,
                    "Pick one:",
                    "Try again:",
                    3
                );
            }
            else
            {
                await context.SayAsync(
                    $"Try <a href=\"{_factSheetUrl}\" target=\"_blank\">this factsheet</a> on DVA's website.");
                context.Done("Done");
            }
        }

        

        [OnDeserialized]
        internal void _deserialized(StreamingContext context)
        {
            _qnaMaker = new QnaMakerClient();
        }


        private async Task QnAQuestionReceived(IDialogContext context, IAwaitable<string> activity)
        {
            var message = (await activity);

            var qnaResult = await _qnaMaker.SearchFaqAsync(message, _kbId);
            string replyContent;

            if (qnaResult == null || !qnaResult.Answers.Any() ||
                qnaResult.Answers.First().Answer == "No good match found in the KB")
            {
                replyContent = FailureMessagesSelection[_random.Next(0, FailureMessagesSelection.Count)];
                await context.PostAsync(replyContent);
            }
            else
            {
                replyContent = qnaResult.Answers.First().Answer;
                await context.PostAsync(replyContent);
            }

            context.Wait(async (dialogContext, result) =>
            {
                var nextMessage = (await result).Text;
                await QnAQuestionReceived(dialogContext, Awaitable.FromItem(nextMessage));
            });

        }

        private static string RemoveFactsheetReferenceFromPurposeSection(string purposeText)
        {
            return purposeText.Replace("This Factsheet", "This topic");
        }
    }
}