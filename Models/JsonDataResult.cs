using Newtonsoft.Json;

namespace Terra.Models
{
    public class JsonDataResult
    {
        [JsonProperty("OSUCCESS")]
        public bool SUCCESS { get; set; }
        [JsonProperty("RESPONSE")]
        public object CONTENIDO { get; set; }
    }

}
