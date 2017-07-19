using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DVAESABot.Utilities;
using Microsoft.Azure.Search.Models;
using NodaTime;

namespace DVAESABot.Domain
{
    [Serializable]
    [SerializePropertyNamesAsCamelCase]
    public class User
    {
        public User()
        {
            
        }

        public UserType? UserType { get; set; }
        public int? Age { get; set; }
        public LocalDate? EnlistmentDate { get; set; }
        public bool? Transitioning { get; set; }
        public bool? SeekingTreatmentOrRehab { get; set; }
    }

    [Serializable]
    public enum UserType
    {
       Member,
       DependentOnMember,
       DependentOnDeceasedMember,
       Organisation,
       Other
    }
}