using Newtonsoft.Json;
using Terra.Commons;
using Terra.Models.Parametrizacion.Pais;
using Terra.Models;
using Terra.Models.Parametrizacion.Cargos;
using Terra.Models.Ubicacion;
using System.Data;

namespace Terra.Dao.Parametrizacion.Cargos
{
    public class CargoDao
    {
        private ITERRADB _dbConnection;

        public CargoDao(ITERRADB dbConnection)
        {
            _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
        }

        public async Task<List<CargoData>> GetAllCargos(string filtro="")
        {

            string query = $"CALL CARGOS_LIST('{filtro}')";

            JsonDataResult response = _dbConnection.TERRA_QTConsulta(query);

            if (!response.SUCCESS)
            {
                return null;
            }

            List<CargoData> cargos = JsonConvert.DeserializeObject<List<CargoData>>(response.CONTENIDO.ToString());

            if (cargos == null || cargos.Count == 0)
            {
                return null;
            }

            return cargos;
        }

        public async Task<CargoData> GetDataCargo(string id)
        {

            string query = $"CALL CARGO_READ('{id}')";

            JsonDataResult json = _dbConnection.TERRA_QTConsulta(query);

            if (json.SUCCESS)
            {
                List<CargoData> data = (List<CargoData>)JsonConvert.DeserializeObject(json.CONTENIDO.ToString(), typeof(List<CargoData>));
                return data.First();
            }

            return null;
        }

        public async Task<bool> EliminarCargo(string id)
        {

            string query = "CALL CARGO_DELETE(" + id + ")";

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

        public async Task<JsonDataResult> InsertarCargo(CargoData cargo)
        {


            string query = $"CALL CARGO_CREATE('{cargo.CODIGO}','{cargo.DESCRIPCION}')";

            JsonDataResult json = _dbConnection.TERRA_QTConsulta(query);

            return json;
        }

        public async Task<JsonDataResult> ActualizarCargo(CargoData cargo)
        {


            string query = $"CALL CARGO_UPDATE('{cargo.CARGOID}','{cargo.CODIGO}','{cargo.DESCRIPCION}')";

            JsonDataResult json = _dbConnection.TERRA_QTConsulta(query);

            return json;
        }


    }
}
