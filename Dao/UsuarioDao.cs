using Newtonsoft.Json;
using System.Data;
using Terra.Commons;
using Terra.Models;

namespace Terra.Dao.Usuario
{
    public class UsuarioDao
    {
        private ITERRADB _dbConnection;

        public UsuarioDao(ITERRADB dbConnection)
        {
            _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
        }


        public async Task<bool> Login(string username, string password)
        {
            string query = $"CALL LOGIN('{username}', '{password}')";

            JsonDataResult json = _dbConnection.TERRA_QTConsulta(query);

            DataTable table = (DataTable)JsonConvert.DeserializeObject(json.CONTENIDO.ToString(), typeof(DataTable));

            int success = Convert.ToInt32(table.Rows[0]["OSUCCESS"].ToString());

            if (success == 0)
                return false;
            return true;

        }

    }
}
