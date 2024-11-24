using Newtonsoft.Json;

namespace Terra.Models.Parametrizacion.GrupoSanguineo
{
    public class GrupoSanguineoData
    {

        [JsonProperty("GRUPOSANGUINEOID")]
        public string GRUPOSANGUINEOID { get; set; }

        [JsonProperty("TIPOSANGRE")]
        public string TIPOSANGRE { get; set; }
    }
}
