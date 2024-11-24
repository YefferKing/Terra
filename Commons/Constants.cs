using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Terra.Commons
{
    public class Constants
    {
       

        public static Dictionary<string, string> Dias = new Dictionary<string, string>
        {
            {"0","LUNES"},
            {"1","MARTES"},
            {"2","MIÉRCOLES"},
            {"3","JUEVES"},
            {"4","VIERNES"},
            {"5","SÁBADO"},
            {"6","DOMINGO"}
        };

        public static Dictionary<string, string> Frecuencia = new Dictionary<string, string>
        {
            {"1","1 VEZ AL MES"},
            {"2","2 VECES AL MES"},
            {"3","3 VECES AL MES"},
            {"4","SEMANAL"}
        };

        public static List<Dia> DiasList = new List<Dia>
        {
            new Dia("LUNES"),
            new Dia("MARTES"),
            new Dia("MIERCOLES"),
            new Dia("JUEVES"),
            new Dia("VIERNES"),
            new Dia("SABADO"),
            new Dia("DOMINGO")
        };

        public static Dictionary<string, string> Meses = new Dictionary<string, string>
        {
             {"1", "ENERO"},
             {"2", "FEBRERO"},
             {"3", "MARZO"},
             {"4", "ABRIL"},
             {"5", "MAYO"},
             {"6", "JUNIO"},
             {"7", "JULIO"},
             {"8", "AGOSTO"},
             {"9", "SEPTIEMBRE"},
             {"10","OCTUBRE"},
             {"11","NOVIEMBRE"},
             {"12","DICIEMBRE"}
        };



        public const int ElementosPorPagina = 15;
        public const int ElementosPorPaginaGrid= 5;
    }

    public class Dia
    {
        public string Nombre { get; set; }

        public Dia(string nombre)
        {
            Nombre = nombre;
        }
    }

    public class SessionValues
    {
        #region GlobalVariables        
        public const string DASHBOARDMENSAJE = "DashboardMensaje";

        #endregion




        #region Filtro aplicados

        #region Reportes

        public const string FILTROREPORTE = "";
        #endregion

        

        #endregion
      
    }
}

