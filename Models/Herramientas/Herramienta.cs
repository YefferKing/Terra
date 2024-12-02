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

    public class PlacasData
    {
        [JsonProperty("PLACAID")]
        public string PLACAID { get; set; }

        [JsonProperty("HERRAMIENTAID")]
        public string HERRAMIENTAID { get; set; }

        [JsonProperty("CODIGO")]
        public string CODIGO { get; set; }

        [JsonProperty("SERIE")]
        public string SERIE { get; set; }

        [JsonProperty("VALORCOMPRA")]
        public string VALORCOMPRA { get; set; }

        [JsonProperty("FLETE")]
        public string FLETE { get; set; }

        [JsonProperty("ESTADO")]
        public string ESTADO { get; set; }
    }
}
