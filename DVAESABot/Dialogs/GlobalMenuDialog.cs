using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AdaptiveCards;
using DVAESABot.Domain;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Fact = AdaptiveCards.Fact;

namespace DVAESABot.Dialogs
{
    public class GlobalMenuDialog : IDialog<MenuOptionChosen>
    {
        public async Task StartAsync(IDialogContext context)
        {
            var promptOptions = new PromptOptions<string>(
                
                prompt: "Choose one:",
                attempts: 1,
                retry: "Try again:",
                tooManyAttempts: "Select one of the options.",
                options: new List<string>() { "Restart", "Feedback", "Contact"},
                descriptions: new List<string>() {"New topic: restart", "Give feedback", "Contact a person"});
            
            PromptDialog.Choice(context,
                OptionChosen,
                promptOptions
            );
        }

        private async Task OptionChosen(IDialogContext context, IAwaitable<string> optionChosen)
        {

            string chosenOptionString = null;
            
            try
            {
                chosenOptionString = await optionChosen;
            }
            catch (TooManyAttemptsException e)
            {
                await this.StartAsync(context);
            }

            var chosenOption = (MenuOptionChosen)Enum.Parse(typeof(MenuOptionChosen), chosenOptionString);
            
            if (chosenOption == MenuOptionChosen.Contact)
            {
                var contactMessage = context.MakeMessage();
                contactMessage.Attachments.Add(new Attachment() {Content = CreateContactCard(), ContentType = AdaptiveCard.ContentType});
                await context.PostAsync(contactMessage);
                context.Done(chosenOption);
            }

            else if (chosenOption == MenuOptionChosen.Feedback)
            {
                context.Call(new FeedbackDialog(), Resume);
            }

            else if (chosenOption == MenuOptionChosen.Restart)
            {
                var root = new RootDialog(context.GetChatContextOrDefault());
                context.Call(root,Resume);
            }


        }

        private async Task Resume(IDialogContext context, IAwaitable<object> result)
        {
            await this.StartAsync(context);
        }


        private AdaptiveCard CreateContactCard()
        {
            var card = new AdaptiveCard();

            var contactInfoContainer = new Container();
            contactInfoContainer.Items.Add(new FactSet()
            {
                Facts = new List<Fact>() { new Fact("📱 Metro Phone:", "133 254"), new Fact("☎ Regional Phone:", "1800 555 254"), new Fact("📧 Email:", "GeneralEnquiries@dva.gov.au")}
            });
            
            card.Body.Add(contactInfoContainer);

            card.Actions.Add(new OpenUrlAction() {Title = "More contact options", Url = "https://www.dva.gov.au/contact" });
            return card;
        }
    }
}