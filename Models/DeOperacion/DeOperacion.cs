using Newtonsoft.Json;

namespace Terra.Models.DeOperacion
{
    public class DeOperacionData
    {
        [JsonProperty("DEOPERACIONID")]
        public string DEOPERACIONID { get; set; }

        [JsonProperty("HERRAMIENTAID")]
        public string HERRAMIENTAID { get; set; }

        [JsonProperty("DESCRIPCION")]
        public string DESCRIPCION { get; set; }

        [JsonProperty("CODIGO")]
        public string CODIGO { get; set; }

        [JsonProperty("CANTIDAD")]
        public int CANTIDAD { get; set; }

        [JsonProperty("VALORCOMPRA")]
        public decimal VALORCOMPRA { get; set; }

    }
}
