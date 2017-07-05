using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DVAESABot.QnAMaker
{

    public class QnaMakerResult
    {
        [JsonProperty(PropertyName = "answers")]
        public List<AnswerObject> Answers { get; set; }
    }

    public class AnswerObject
    {
        [JsonProperty(PropertyName = "answer")]
        public string Answer { get; set; }

        [JsonProperty(PropertyName = "questions")]
        public List<string> Questions { get; set; }

        [JsonProperty(PropertyName = "score")]
        public double Score { get; set; }
    }
    
}
