using Newtonsoft.Json;

namespace Terra.Models.Entradas
{
    public class OperacionData
    {
        [JsonProperty("OPERACIONID")]
        public string OPERACIONID { get; set; }

        [JsonProperty("TIPOOPERACIONID")]
        public string TIPOOPERACIONID { get; set; }

        [JsonProperty("TIPO")]
        public string TIPO { get; set; }

        [JsonProperty("UBICACIONID")]
        public string UBICACIONID { get; set; }

        [JsonProperty("UBICACION")]
        public string UBICACION { get; set; }

        [JsonProperty("FECHA")]
        public DateTime FECHA { get; set; } = DateTime.Now;

        [JsonProperty("NUMERO")]
        public int NUMERO { get; set; }

        [JsonProperty("PERSONAID")]
        public string PERSONAID { get; set; }

        [JsonProperty("OBSERVACION")]
        public string OBSERVACION { get; set; }
        
        [JsonProperty("FLETE")]
        public decimal FLETE { get; set; }

        [JsonProperty("TOTAL")]
        public string TOTAL { get; set; }
    }
}
