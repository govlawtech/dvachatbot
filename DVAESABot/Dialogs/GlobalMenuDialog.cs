using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DVAESABot.Domain;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace DVAESABot.Dialogs
{
    public class GlobalMenuDialog : IDialog<MenuOptionChosen>
    {
        public async Task StartAsync(IDialogContext context)
        {
            var promptOptions = new PromptOptions<MenuOptionChosen>(
                prompt: "Menu:",
                options: new List<MenuOptionChosen>() { MenuOptionChosen.Restart, MenuOptionChosen.Feedback, MenuOptionChosen.Contact, MenuOptionChosen.Continue},
                descriptions: new List<string>() {"Restart", "Feedback", "Contact","Continue"});
            
            PromptDialog.Choice(context,
                OptionChosen,
                promptOptions
            );
        }

        private async Task OptionChosen(IDialogContext context, IAwaitable<MenuOptionChosen> optionChosen)
        {
            var chosenOption = await optionChosen;

            if (chosenOption == MenuOptionChosen.Contact)
            {
                await context.SayAsync("This is how to contact DVA:....");
            }

            else if (chosenOption == MenuOptionChosen.Restart)
            {
                //context.Reset();
                var root = new RootDialog(context.GetChatContextOrDefault());
                context.Call(root,null);
            }

            else if (chosenOption == MenuOptionChosen.Continue)
            {

            }
        }
        
    }
}