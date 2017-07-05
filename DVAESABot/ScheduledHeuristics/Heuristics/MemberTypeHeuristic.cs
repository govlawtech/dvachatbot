using System;
using DVAESABot.Dialogs;
using DVAESABot.Domain;
using DVAESABot.Utilities;
using Microsoft.Bot.Builder.Dialogs;

namespace DVAESABot.ScheduledHeuristics.Heuristics
{
    class MemberTypeHeuristic : IScheduledHeuristic, IHaveDialog<UserType>
    {
        public string Description => "Member Type";

        public int Salience => 200;

        public Predicate<ChatContext> Condition => c => c.User.UserType.HasValue;

        public Action<ChatContext> Action => c =>
        {
            if (c.User.UserType != null)
            {
                switch (c.User.UserType)
                {
                    case UserType.Member:
                    {
                        c.FactsheetShortlist = c.FactsheetShortlist.RemoveCategories("GS", "HIP");
                        break;
                    }
                    case UserType.DependentOnDeceasedMember:
                    {
                        c.FactsheetShortlist = c.FactsheetShortlist.RemoveAllExceptWithKeyWords("Dependent", "Defacto", "Bereavement");
                        break;
                    }
                    case UserType.DependentOnMember:
                    {
                        c.FactsheetShortlist = c.FactsheetShortlist.RemoveAllExceptWithKeyWords("Dependent", "Children");
                        break;
                    }
                    case UserType.Organisation:
                    {
                        c.FactsheetShortlist = c.FactsheetShortlist.RemoveAllCategoriesOtherThan("HIP", "GS", "IP", "FIP", "DVA");
                        break;
                    }
                }
            }
        };

        public IDialog<UserType> GetDialog()
        {
            return new UserTypeDialog();
        }

        public void ApplyResult(UserType result, ChatContext chatContext)
        {
            chatContext.User.UserType = result;
        }
    }
}