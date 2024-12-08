using Newtonsoft.Json;
using Terra.Commons;
using Terra.Models.Parametrizacion.Personas;
using Terra.Models;
using Terra.Models.Herramientas;
using System.Data;

namespace Terra.Dao.Herramientas
{
    public class HerramientaDao
    {
        private ITERRADB _dbConnection;

        public HerramientaDao(ITERRADB dbConnection)
        {
            _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
        }

        public async Task<List<HerramientaData>> GetAllHerramientas(string filtro = "")
        {

            string query = $"CALL HERRAMIENTAS_LIST('{filtro}')";

            JsonDataResult response = _dbConnection.TERRA_QTConsulta(query);

            if (!response.SUCCESS)
            {
                return null;
            }

            List<HerramientaData> herramientas = JsonConvert.DeserializeObject<List<HerramientaData>>(response.CONTENIDO.ToString());

            if (herramientas == null || herramientas.Count == 0)
            {
                return null;
            }

            return herramientas;
        }

        public async Task<bool> EliminarHerramientas(string id)
        {

            string query = "CALL HERRAMIENTAS_DELETE(" + id + ")";

            JsonDataResult response = _dbConnection.TERRA_QTConsulta(query);

            if (!response.SUCCESS)
            {
                return false;
            }

            DataTable tabla = (DataTable)JsonConvert.DeserializeObject(response.CONTENIDO.ToString(), typeof(DataTable));
            if (tabla != null && tabla.Rows.Count > 0)
            {

                if (Convert.ToInt32(tabla.Rows[0]["OSUCCESS"].ToString()) == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public async Task<HerramientaData> GetDataHerramienta(string id)
        {

            string query = $"CALL HERRAMIENTAS_READ('{id}')";

            JsonDataResult json = _dbConnection.TERRA_QTConsulta(query);

            if (json.SUCCESS)
            {
                List<HerramientaData> data = (List<HerramientaData>)JsonConvert.DeserializeObject(json.CONTENIDO.ToString(), typeof(List<HerramientaData>));
                return data.First();
            }

            return null;
        }

        public async Task<JsonDataResult> InsertarHerramienta(HerramientaData herramienta)
        {


            string query = $"CALL HERRAMIENTAS_CREATE('{herramienta.CODIGO}','{herramienta.NOMBRE}','{herramienta.DESCRIPCION}','{herramienta.INSUMOS}')";

            JsonDataResult json = _dbConnection.TERRA_QTConsulta(query);

            return json;
        }

        public async Task<JsonDataResult> ActualziarHerramienta(HerramientaData herramienta)
        {


            string query = $"CALL HERRAMIENTAS_UPDATE('{herramienta.HERRAMIENTAID}','{herramienta.CODIGO}','{herramienta.NOMBRE}','{herramienta.DESCRIPCION}','{herramienta.INSUMOS}')";

            JsonDataResult json = _dbConnection.TERRA_QTConsulta(query);

            return json;
        }

    }
}
