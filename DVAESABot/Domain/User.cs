using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DVAESABot.Utilities;
using NodaTime;

namespace DVAESABot.Domain
{
    public class User
    {
        public User()
        {
            UserType = Option.None<UserType>();
            EnlistmentDate = Option.None<LocalDate>();
            Transitioning = Option.None<bool>();
            IsMRCAEnlistee = Option.None<bool>();
        }
        public Option<UserType> UserType { get; set; }
        public Option<LocalDate> EnlistmentDate { get; set; }
        public Option<bool> Transitioning { get; set; }
        public Option<bool> IsMRCAEnlistee { get; set; }
    }

    public enum UserType
    {
       Member,
       DependentOnMember,
       DependentOnDeceasedMember,
       Organisation
    }
}