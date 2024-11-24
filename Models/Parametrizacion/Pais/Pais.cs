using Newtonsoft.Json;

namespace Terra.Models.Parametrizacion.Pais
{
    public class PaisData
    {

        [JsonProperty("PAISID")]
        public string PAISID { get; set; }

        [JsonProperty("PAIS")]
        public string PAIS { get; set; }
    }
}
