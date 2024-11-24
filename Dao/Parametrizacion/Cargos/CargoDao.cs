using Newtonsoft.Json;
using Terra.Commons;
using Terra.Models.Parametrizacion.Pais;
using Terra.Models;
using Terra.Models.Parametrizacion.Cargos;

namespace Terra.Dao.Parametrizacion.Cargos
{
    public class CargoDao
    {
        private ITERRADB _dbConnection;

        public CargoDao(ITERRADB dbConnection)
        {
            _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
        }

        public async Task<List<CargoData>> GetAllCargos()
        {

            string query = $"CALL CARGOS_LIST()";

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
    }
}
