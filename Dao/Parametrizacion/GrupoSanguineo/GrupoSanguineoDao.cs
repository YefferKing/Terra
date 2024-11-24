using Newtonsoft.Json;
using Terra.Commons;
using Terra.Models.Parametrizacion.Pais;
using Terra.Models;
using Terra.Models.Parametrizacion.GrupoSanguineo;

namespace Terra.Dao.Parametrizacion.GrupoSanguineo
{
    public class GrupoSanguineoDao
    {
        private ITERRADB _dbConnection;

        public GrupoSanguineoDao(ITERRADB dbConnection)
        {
            _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
        }

        public async Task<List<GrupoSanguineoData>> GetAllGrupoSanguineo()
        {

            string query = $"CALL GRUPOSANGUINEO_LIST()";

            JsonDataResult response = _dbConnection.TERRA_QTConsulta(query);

            if (!response.SUCCESS)
            {
                return null;
            }

            List<GrupoSanguineoData> grupo = JsonConvert.DeserializeObject<List<GrupoSanguineoData>>(response.CONTENIDO.ToString());

            if (grupo == null || grupo.Count == 0)
            {
                return null;
            }

            return grupo;
        }
    }
}
