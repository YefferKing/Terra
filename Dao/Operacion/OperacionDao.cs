using Newtonsoft.Json;
using Terra.Commons;
using Terra.Models.Ubicacion;
using Terra.Models;
using Terra.Models.Entradas;
using System.Data;
using Terra.Models.DeOperacion;

namespace Terra.Dao.Operacion
{
    public class OperacionDao
    {
        private ITERRADB _dbConnection;

        public OperacionDao(ITERRADB dbConnection)
        {
            _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
        }

        public async Task<List<OperacionData>> GetAllOperacion()
        {

            string query = $"CALL OPERACION_LIST()";

            JsonDataResult response = _dbConnection.TERRA_QTConsulta(query);

            if (!response.SUCCESS)
            {
                return null;
            }

            List<OperacionData> operacion = JsonConvert.DeserializeObject<List<OperacionData>>(response.CONTENIDO.ToString());

            if (operacion == null || operacion.Count == 0)
            {
                return null;
            }

            return operacion;
        }

        public async Task<OperacionData> GetDataOperacion(string id)
        {

            string query = $"CALL OPERACION_READ('{id}')";

            JsonDataResult json = _dbConnection.TERRA_QTConsulta(query);

            if (json.SUCCESS)
            {
                List<OperacionData> data = (List<OperacionData>)JsonConvert.DeserializeObject(json.CONTENIDO.ToString(), typeof(List<OperacionData>));
                return data.First();
            }

            return null;
        }

        public async Task<bool> EliminarOperacion(string id)
        {

            string query = "CALL OPERACION_DELETE(" + id + ")";

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

        public async Task<JsonDataResult> InsertarOperacion(OperacionData operacion)
        {


            string query = $"CALL OPERACION_CREATE('{operacion.TIPOOPERACIONID}','{Helpers.formatFecha(operacion.FECHA, "yyyy/MM/dd")}','{operacion.NUMERO}','{operacion.UBICACIONID}','{operacion.PERSONAID}','{operacion.OBSERVACION}','{operacion.FLETE}')";

            JsonDataResult json = _dbConnection.TERRA_QTConsulta(query);

            return json;
        }

        public async Task<JsonDataResult> ActualizarOperacion(OperacionData operacion)
        {


            string query = $"CALL OPERACION_UPDATE('{operacion.OPERACIONID}','{operacion.TIPOOPERACIONID}','{operacion.FECHA}','{operacion.NUMERO}','{operacion.UBICACIONID}','{operacion.PERSONAID}','{operacion.OBSERVACION}','{operacion.FLETE}')";

            JsonDataResult json = _dbConnection.TERRA_QTConsulta(query);

            return json;
        }

        public async Task<List<DeOperacionData>> GetAllDeOperacion(string id)
        {

            string query = $"CALL DEOPERACION_LIST('{id}')";

            JsonDataResult response = _dbConnection.TERRA_QTConsulta(query);

            if (!response.SUCCESS)
            {
                return null;
            }

            List<DeOperacionData> Doperacion = JsonConvert.DeserializeObject<List<DeOperacionData>>(response.CONTENIDO.ToString());

            if (Doperacion == null || Doperacion.Count == 0)
            {
                return null;
            }

            return Doperacion;
        }
        public async Task<JsonDataResult> InsertarDeOperacion(DeOperacionData DeOperacion)
        {


            string query = $"CALL DEOPERACION_CREATE('{DeOperacion.OPERACIONID}','{DeOperacion.HERRAMIENTAID}','{DeOperacion.CANTIDAD}','{DeOperacion.VALORCOMPRA}')";

            JsonDataResult json = _dbConnection.TERRA_QTConsulta(query);

            return json;
        }

        public async Task<JsonDataResult> ActualizarDeOperacion(DeOperacionData DeOperacion)
        {


            string query = $"CALL DEOPERACION_UPDATE('{DeOperacion.DEOPERACIONID}','{DeOperacion.HERRAMIENTAID}','{DeOperacion.CANTIDAD}','{DeOperacion.VALORCOMPRA}')";

            JsonDataResult json = _dbConnection.TERRA_QTConsulta(query);

            return json;
        }

        public async Task<bool> EliminarDeOperacion(string id)
        {

            string query = "CALL DEOPERACION_DELETE(" + id + ")";

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

    }
}
