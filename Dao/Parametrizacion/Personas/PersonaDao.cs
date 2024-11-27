using Newtonsoft.Json;
using System.Data;
using Terra.Commons;
using Terra.Models;
using Terra.Models.Parametrizacion.Personas;

namespace Terra.Dao.Parametrizacion.Personas
{
    public class PersonaDao
    {
        private ITERRADB _dbConnection;

        public PersonaDao(ITERRADB dbConnection)
        {
            _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
        }

        public async Task<List<PersonaData>> GetAllPersonas()
        {

            string query = $"CALL PERSONA_LIST()";

            JsonDataResult response =  _dbConnection.TERRA_QTConsulta(query);

            if (!response.SUCCESS)
            {
                return null;
            }

            List<PersonaData> personas = JsonConvert.DeserializeObject<List<PersonaData>>(response.CONTENIDO.ToString());

            if (personas == null || personas.Count == 0)
            {
                return null;
            }

            return personas;
        }

        public async Task<PersonaData> GetDataPersona(string id)
        {

            string query = $"CALL PERSONA_READ('{id}')";

            JsonDataResult json = _dbConnection.TERRA_QTConsulta(query);

            if (json.SUCCESS)
            {
                List<PersonaData> data = (List<PersonaData>)JsonConvert.DeserializeObject(json.CONTENIDO.ToString(), typeof(List<PersonaData>));
                return data.First();
            }

            return null;
        }

        public async Task<JsonDataResult> InsertarPersona(PersonaData persona)
        {

            
            string query = $"CALL PERSONA_CREATE('{persona.DOCUMENTO}','{persona.NOMBRES}','{persona.APELLIDOS}','{persona.PAISID}'," +
                $"'{persona.RESIDENCIA}','{Helpers.formatFecha(persona.FECHANACIMIENTO,"yyyy/MM/dd")}','{persona.TIPOSANGRE}','{persona.NIVELACADEMICOID}','{persona.CODCARGO}','{Helpers.formatFecha(persona.FECHAINNGRESO,"yyyy/MM/dd")}')";

            JsonDataResult json =  _dbConnection.TERRA_QTConsulta(query);

            return json;
        }

        public async Task<JsonDataResult> ActualizarPersona(PersonaData persona)
        {

            string query = $"CALL PERSONA_UPDATE('{persona.PERSONAID}','{persona.DOCUMENTO}','{persona.NOMBRES}','{persona.APELLIDOS}','{persona.PAISID}'," +
                $"'{persona.RESIDENCIA}','{Helpers.formatFecha(persona.FECHANACIMIENTO, "yyyy/MM/dd")}','{persona.TIPOSANGRE}','{persona.NIVELACADEMICOID}','{persona.CODCARGO}','{Helpers.formatFecha(persona.FECHAINNGRESO, "yyyy/MM/dd")}')";
            
            JsonDataResult json = _dbConnection.TERRA_QTConsulta(query);
            return json;
        }

        public async Task<bool> EliminarPersona(string id)
        {

            string query = "CALL PERSONA_DELETE(" + id + ")";

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

        public async Task<bool> EliminarItem(string id)
        {

            string query = "CALL ITEMS_DELETE(" + id + ")";

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

        public async Task<List<ItemsData>> GetAllItems(string Id)
        {

            string query = $"CALL ITEMS_LIST('{Id}')";

            JsonDataResult response = _dbConnection.TERRA_QTConsulta(query);

            if (!response.SUCCESS)
            {
                return null;
            }

            List<ItemsData> items = JsonConvert.DeserializeObject<List<ItemsData>>(response.CONTENIDO.ToString());

            if (items == null || items.Count == 0)
            {
                return null;
            }

            return items;
        }

        public async Task<List<TipoItemsGrid>> GetAllTipoItems()
        {

            string query = $"CALL TIPOITEMS_LIST()";

            JsonDataResult response = _dbConnection.TERRA_QTConsulta(query);

            if (!response.SUCCESS)
            {
                return null;
            }

            List<TipoItemsGrid> tipoItems = JsonConvert.DeserializeObject<List<TipoItemsGrid>>(response.CONTENIDO.ToString());

            if (tipoItems == null || tipoItems.Count == 0)
            {
                return null;
            }

            return tipoItems;
        }

        public async Task<JsonDataResult> InsertarItems(ItemsData items)
        {


            string query = $"CALL ITEMS_CREATE('{items.PERSONAID}','{items.TIPOITEMID}','{items.CONTENIDO}')";
                

            JsonDataResult json = _dbConnection.TERRA_QTConsulta(query);

            return json;
        }
    }
}
