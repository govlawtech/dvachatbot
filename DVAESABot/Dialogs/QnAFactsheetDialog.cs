using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using AdaptiveCards;
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
            "Try asking something else or putting your question differently.",
            "Can't find anything on that in this topic.  Ask something else?",
            "Don't know anything about that.  Try asking something else."
        };

        private readonly string _factSheetTitle;
        private readonly string _factSheetUrl;
        private readonly string _factSheetCode;
        private string _kbId;


        [NonSerialized] private QnaMakerKb _qnaMaker;

        [NonSerialized] private static readonly Random _random = new Random();

        public QnAFactsheetDialog()
        {
        }

        public QnAFactsheetDialog(string factSheetTitle, string factSheetUrl)
        {
            _factSheetTitle = factSheetTitle;
            _factSheetUrl = factSheetUrl;
            _factSheetCode = DialogHelper.ExtractFactsheetCodeFromFactSheeTitle(factSheetTitle);
            _kbId = KbId.kbIDs[_factSheetCode];
            _qnaMaker = new QnaMakerKb();
        }

        public async Task StartAsync(IDialogContext context)
        {
            var factSheetName = DialogHelper.ExtractTopicFromFactSheetTitle(_factSheetTitle);
            var hintMessage = new StringBuilder();
            
            var questionsAndAnswers = await _qnaMaker.GetQuestionsAndAnswers(_kbId);

            if (questionsAndAnswers.Any())
            {
                if (questionsAndAnswers.ContainsKey("Purpose"))
                {
                    hintMessage.AppendLine(RemoveFactsheetReferenceFromPurposeSection(questionsAndAnswers["Purpose"]));
                }

                IEnumerable<string> options = questionsAndAnswers.Keys.Where(q => q != "Purpose").ToList();
                if (options.Any())
                {
                    hintMessage.AppendLine("<br>Suggested questions:");
                    var suggestions = String.Join("", options.Select(o => $"<li>{o}</li>"));
                    var suggestionText = $"<ul>{suggestions}</ul>";
                    hintMessage.Append(suggestionText);
                }

                await context.PostAsync(hintMessage.ToString());
                context.Wait(QnAQuestionReceived);
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
            _qnaMaker = new QnaMakerKb();
        }

        private async Task QnAQuestionReceived(IDialogContext context, IAwaitable<IMessageActivity> item)
        {
            var message = await item;

            var qnaResult = await _qnaMaker.SearchFaqAsync(message.Text, _kbId);
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
        }

        private static string RemoveFactsheetReferenceFromPurposeSection(string purposeText)
        {
            return purposeText.Replace("This Factsheet", "This topic");
        }

    }
}