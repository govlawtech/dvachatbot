using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using DVAESABot.Domain;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using User = Microsoft.VisualBasic.ApplicationServices.User;

namespace DVAESABot.Dialogs
{
    public class UserTypeDialog : IDialog<UserType>
    {
        private static readonly string MEMBER = "Member";
        private static readonly string DEPENDENT = "Dependent on a member";
        private static readonly string DEPENDENT_ON_DECEASED = "Dependent on a deceased member";
        private static readonly string ORGANISATION = "Organisation";
        private static readonly string OTHER = "Other";

        public async Task StartAsync(IDialogContext context)
        {
            if (context.GetChatContextOrDefault().User.UserType.HasValue)
            {
                context.Done(context.GetChatContextOrDefault().User.UserType);
            }
            else
            {
                PromptDialog.Choice(

                    context: context,
                    resume: ResponseReceived,
                    choices: new Dictionary<string, IEnumerable<string>>()
                    {
                        {
                            MEMBER,
                            new[]
                            {
                                "former member", "cadet", "reservist", "peacekeeper", "veteran", "cadet", "mariner",
                                "eligible civilian", "permanent force", "adf"
                            }
                        },
                        {DEPENDENT, new List<string>()},
                        {DEPENDENT_ON_DECEASED, new List<string>()},
                        {ORGANISATION, new List<string>()},
                        {OTHER, new List<string>()}
                    },
                    prompt: "Which best describes your circumstances?",
                    retry: "Try again:",
                    attempts: 1
                );
            }
        }

        private async Task ResponseReceived(IDialogContext context, IAwaitable<string> activity)
        {
            string userTypeString = null;
            try
            {
                userTypeString = await activity;
            }
            catch (TooManyAttemptsException e)
            {
                await context.SayAsync("Pick one.");
                await StartAsync(context);
            }

            UserType userType = userTypeString == MEMBER
                ? UserType.Member
                : userTypeString == DEPENDENT
                    ? UserType.DependentOnMember
                    : userTypeString == DEPENDENT_ON_DECEASED
                        ? UserType.DependentOnDeceasedMember
                        : userTypeString == ORGANISATION
                            ? UserType.Organisation
                            : UserType.Other;
            
            var cc = context.GetChatContextOrDefault();
            cc.User.UserType = userType;
            context.SetChatContext(cc);
            context.Done(userType);
        }
    }
}