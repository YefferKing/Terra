using Newtonsoft.Json;

namespace Terra.Models.Herramientas
{
    public class HerramientaData
    {
        [JsonProperty("HERRAMIENTAID")]
        public string HERRAMIENTAID { get; set; }

        [JsonProperty("CODIGO")]
        public string CODIGO { get; set; }

        [JsonProperty("NOMBRE")]
        public string NOMBRE { get; set; }

        [JsonProperty("DESCRIPCION")]
        public string DESCRIPCION { get; set; }

        [JsonProperty("INSUMOS")]
        public string INSUMOS { get; set; }
    }
}
