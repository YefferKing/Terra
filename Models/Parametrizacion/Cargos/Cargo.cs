using Newtonsoft.Json;

namespace Terra.Models.Parametrizacion.Cargos
{
    public class CargoData
    {
        [JsonProperty("CARGOID")]
        public string CARGOID { get; set; }

        [JsonProperty("CODIGO")]
        public string CODIGO { get; set; }

        [JsonProperty("DESCRIPCION")]
        public string DESCRIPCION { get; set; }
    }
}
