using Newtonsoft.Json;

namespace Terra.Models.Ubicacion
{
    public class UbicacionData
    {
        [JsonProperty("UBICACIONID")]
        public string UBICACIONID { get; set; }

        [JsonProperty("CODIGO")]
        public string CODIGO { get; set; }

        [JsonProperty("DESCRIPCION")]
        public string DESCRIPCION { get; set; }
    }
}
