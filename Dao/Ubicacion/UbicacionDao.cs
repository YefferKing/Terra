using Newtonsoft.Json;
using Terra.Commons;
using Terra.Models.Herramientas;
using Terra.Models;
using Terra.Models.Ubicacion;
using System.Data;

namespace Terra.Dao.Ubicacion
{
    public class UbicacionDao
    {
        private ITERRADB _dbConnection;

        public UbicacionDao(ITERRADB dbConnection)
        {
            _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
        }

        public async Task<List<UbicacionData>> GetAllUbicacion()
        {

            string query = $"CALL UBICACION_LIST()";

            JsonDataResult response = _dbConnection.TERRA_QTConsulta(query);

            if (!response.SUCCESS)
            {
                return null;
            }

            List<UbicacionData> ubicacion = JsonConvert.DeserializeObject<List<UbicacionData>>(response.CONTENIDO.ToString());

            if (ubicacion == null || ubicacion.Count == 0)
            {
                return null;
            }

            return ubicacion;
        }

        public async Task<UbicacionData> GetDataUbicacion(string id)
        {

            string query = $"CALL UBICACION_READ('{id}')";

            JsonDataResult json = _dbConnection.TERRA_QTConsulta(query);

            if (json.SUCCESS)
            {
                List<UbicacionData> data = (List<UbicacionData>)JsonConvert.DeserializeObject(json.CONTENIDO.ToString(), typeof(List<UbicacionData>));
                return data.First();
            }

            return null;
        }

        public async Task<bool> EliminarUbicacion(string id)
        {

            string query = "CALL UBICACION_DELETE(" + id + ")";

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


        public async Task<JsonDataResult> InsertarUbicacion(UbicacionData ubicacion)
        {


            string query = $"CALL UBICACION_CREATE('{ubicacion.CODIGO}','{ubicacion.DESCRIPCION}')";

            JsonDataResult json = _dbConnection.TERRA_QTConsulta(query);

            return json;
        }

        public async Task<JsonDataResult> ActualizarUbicacion(UbicacionData ubicacion)
        {


            string query = $"CALL UBICACION_UPDATE('{ubicacion.UBICACIONID}','{ubicacion.CODIGO}','{ubicacion.DESCRIPCION}')";

            JsonDataResult json = _dbConnection.TERRA_QTConsulta(query);

            return json;
        }
    }
}
