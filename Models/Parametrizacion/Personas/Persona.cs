using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Terra.Models.Parametrizacion.Personas
{
    public class PersonaData
    {
        [JsonProperty("PERSONAID")]
        public string PERSONAID { get; set; }

        [JsonProperty("DOCUMENTO")]
        [Required(ErrorMessage = "Este campo es obligatorio.")]
        public string DOCUMENTO { get; set; }

        [JsonProperty("NOMBRES")]
        public string NOMBRES { get; set; }

        [JsonProperty("APELLIDOS")]
        public string APELLIDOS { get; set; }

        [JsonProperty("FECHANACIMIENTO")]
        public DateTime FECHANACIMIENTO { get; set; } = DateTime.Now;

        [JsonProperty("PAISID")]
        public string PAISID { get; set; }


        [JsonProperty("RESIDENCIA")]
        public string RESIDENCIA { get; set; }

        [JsonProperty("PAIS")]
        public string PAIS { get; set; }

        [JsonProperty("TIPOSANGRE")]
        public string TIPOSANGRE { get; set; }

        [JsonProperty("NIVELACADEMICOID")]
        public string NIVELACADEMICOID { get; set; }

        [JsonProperty("NIVELACADEMICO")]
        public string NIVELACADEMICO { get; set; }

        [JsonProperty("CODCARGO")]
        public string CODCARGO { get; set; }

        [JsonProperty("DESCRIPCARGO")]
        public string DESCRIPCARGO { get; set; }

        [JsonProperty("FECHAINNGRESO")]
        public DateTime FECHAINNGRESO { get; set; } = DateTime.Now;

        public int? edad { get; set; }


    }

    public class ItemsData
    {
        [JsonProperty("ITEMID")]
        public string ITEMID { get; set; }

        [JsonProperty("TIPOITEMID")]
        public string TIPOITEMID { get; set; }

        [JsonProperty("TIPO")]
        public string TIPO { get; set; }

        [JsonProperty("CONTENIDO")]
        public string CONTENIDO { get; set; }

        public string PERSONAID { get; set; }

    }

    public class TipoItemsGrid
    {
        [JsonProperty("TIPOITEMID")]
        public string TIPOITEMID { get; set; }

        [JsonProperty("DESCRIPCION")]
        public string DESCRIPITEM { get; set; }

        [JsonProperty("CONTENIDO")]
        public string CONTENIDO { get; set; }

    }
}
