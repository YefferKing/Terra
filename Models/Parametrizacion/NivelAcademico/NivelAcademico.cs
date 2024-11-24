using Newtonsoft.Json;

namespace Terra.Models.Parametrizacion.NivelAcademico
{
    public class NivelAcademicoData
    {
        [JsonProperty("NIVELACADEMICOID")]
        public string NIVELACADEMICOID { get; set; }

        [JsonProperty("NIVELACADEMICO")]
        public string NIVELACADEMICO { get; set; }
    }
}
