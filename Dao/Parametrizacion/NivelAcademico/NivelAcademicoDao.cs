using Newtonsoft.Json;
using Terra.Commons;
using Terra.Models.Parametrizacion.Pais;
using Terra.Models;
using Terra.Models.Parametrizacion.NivelAcademico;

namespace Terra.Dao.Parametrizacion.NivelAcademico
{
    public class NivelAcademicoDao
    {
        private ITERRADB _dbConnection;

        public NivelAcademicoDao(ITERRADB dbConnection)
        {
            _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
        }

        public async Task<List<NivelAcademicoData>> GetAllNivelAcademico()
        {

            string query = $"CALL NIVELACADEMICO_LIST()";

            JsonDataResult response = _dbConnection.TERRA_QTConsulta(query);

            if (!response.SUCCESS)
            {
                return null;
            }

            List<NivelAcademicoData> academico = JsonConvert.DeserializeObject<List<NivelAcademicoData>>(response.CONTENIDO.ToString());

            if (academico == null || academico.Count == 0)
            {
                return null;
            }

            return academico;
        }
    }
}
