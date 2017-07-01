using System;
using DVAESABot.Domain;
using DVAESABot.Utilities;
using Microsoft.Bot.Builder.Dialogs;

namespace DVAESABot.ScheduledHeuristics.Heuristics
{
    class MemberTypeHeuristic : IScheduledHeuristic, IHaveDialog<UserType>
    {
        public string Description => "Member Type";

        public int Salience => 200;

        public Predicate<ChatContext> Condition => c => c.User.UserType.Tag == OptionType.Some;

        public Action<ChatContext> Action => c =>
        {
            if (c.User.UserType.MatchSome(out UserType userType))
            {
                switch (userType)
                {
                    case UserType.Member:
                    {
                        c.FactsheetShortlist.RemoveCategories("GS", "HIP");
                        break;
                    }
                    case UserType.DependentOnDeceasedMember:
                    {
                        c.FactsheetShortlist.RemoveAllExceptWithKeyWords("Dependent", "Defacto", "Bereavement");
                        break;
                    }
                    case UserType.DependentOnMember:
                    {
                        c.FactsheetShortlist.RemoveAllExceptWithKeyWords("Dependent", "Children");
                        break;
                    }
                    case UserType.Organisation:
                    {
                        c.FactsheetShortlist.RemoveAllCategoriesOtherThan("HIP", "GS", "IP", "FIP", "DVA");
                        break;
                    }
                }
            }
        };

        public IHaveDialog<UserType> GetDialog()
        {
            throw new NotImplementedException();
        }

        public void ApplyResult(UserType result, ChatContext chatContext)
        {
            chatContext.User.UserType = new Some<UserType>(result);
        }
    }
}