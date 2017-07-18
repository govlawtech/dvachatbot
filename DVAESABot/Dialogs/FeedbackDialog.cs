using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AdaptiveCards;
using Microsoft.ApplicationInsights;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace DVAESABot.Dialogs
{
    public class FeedbackDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            var feedbackPrompt = context.MakeMessage();
            feedbackPrompt.Attachments.Add(new Attachment(contentType: AdaptiveCard.ContentType,content: BuildNpsCard()));
            await context.PostAsync(feedbackPrompt);

            context.Wait(Resume);
        }

        private async Task Resume(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            await context.SayAsync("Thanks! ☺");

            dynamic cardResult = result.GetAwaiter().GetResult();
            dynamic score = cardResult.Value.nps;
            int scoreInt = (int)Convert.ToInt32(score.ToString());
            var tc = new TelemetryClient();
            tc.TrackTrace(cardResult.Value.ToString());
            context.Done(scoreInt);
        }

        private AdaptiveCard BuildNpsCard()
        {
            var card = new AdaptiveCard();
            var npsContainer = new Container();
            npsContainer.Items.Add(new TextBlock() {Text = "How likely is it that you would recommend Chappie to a friend or colleague?",Wrap = true});
            npsContainer.Items.Add(BuildNpsChoiceSet());

            var commentsContainer = new Container();
            commentsContainer.Items.Add(new TextBlock() {Text = "Comments:"});
            commentsContainer.Items.Add(new TextInput() {IsMultiline = true,IsRequired = false, Id = "comments"});

            card.Body.Add(npsContainer);
            card.Body.Add(commentsContainer);

            card.Actions.Add(new SubmitAction() {Title = "Submit"});
            return card;
        }

        private ChoiceSet BuildNpsChoiceSet()
        {
            var choiceItems = (from s in Enumerable.Range(0, 11)
                select new Choice() {IsSelected = false, Title = s.ToString(),Value = s.ToString()}).ToList();
            choiceItems[0].Title = "0: Not at all likely";
            choiceItems[10].Title = "10: Extremely likely";

            return new ChoiceSet() {Choices = choiceItems, IsMultiSelect = false, IsRequired = true,Id = "nps"};
        }
    }

}