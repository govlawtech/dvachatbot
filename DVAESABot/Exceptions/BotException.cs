using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace DVAESABot.Exceptions
{
    [Serializable]
    public class GeneralBotException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public GeneralBotException()
        {
        }

        public GeneralBotException(string message) : base(message)
        {
        }

        public GeneralBotException(string message, Exception inner) : base(message, inner)
        {
        }

        protected GeneralBotException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}