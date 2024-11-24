using Newtonsoft.Json;
using Terra.Commons;
using Terra.Models.Parametrizacion.Personas;
using Terra.Models;
using Terra.Models.Parametrizacion.Pais;

namespace Terra.Dao.Parametrizacion.Pais
{
    public class PaisDao
    {
        private ITERRADB _dbConnection;

        public PaisDao(ITERRADB dbConnection)
        {
            _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
        }

        public async Task<List<PaisData>> GetAllPais()
        {

            string query = $"CALL PAISES_LIST()";

            JsonDataResult response = _dbConnection.TERRA_QTConsulta(query);

            if (!response.SUCCESS)
            {
                return null;
            }

            List<PaisData> pais = JsonConvert.DeserializeObject<List<PaisData>>(response.CONTENIDO.ToString());

            if (pais == null || pais.Count == 0)
            {
                return null;
            }

            return pais;
        }
    }
}
