using BlazorBootstrap;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Newtonsoft.Json;
using System.Data;
using Terra.Components.Layout.Components;
using Terra.Dao.Herramientas;
using Terra.Dao.Operacion;
using Terra.Dao.Parametrizacion.Personas;
using Terra.Dao.Ubicacion;
using Terra.Models;
using Terra.Models.DeOperacion;
using Terra.Models.Entradas;
using Terra.Models.Herramientas;
using Terra.Models.Parametrizacion.GrupoSanguineo;
using Terra.Models.Parametrizacion.Personas;
using Terra.Models.Ubicacion;

namespace Terra.Components.Pages.Reintegros
{
    public partial class FormReintegros
    {
        [Inject]
        private HerramientaDao _herramientaDao { get; set; }

        [Inject]
        private IToastService _toast { get; set; }

        [Parameter]
        public string reintegroId { get; set; }

        [Inject]
        private NavigationManager _navigationManager { get; set; }

        [Inject]
        private UbicacionDao _ubicacionDao { get; set; }

        [Inject]
        private PersonaDao _personaDao { get; set; }

        [Inject]
        private OperacionDao _operacionDao { get; set; }

        List<HerramientaData> data = new List<HerramientaData>();

        List<HerramientaData> dataDB = new List<HerramientaData>();


        private OperacionData dataOperacionInsert = new OperacionData();

        private OperacionData dataOperacionForm = new OperacionData();

        private EditContext editContextForm;

        private List<UbicacionData> dataSelectUbicacion = new List<UbicacionData>();

        private List<PersonaData> dataSelectPersona = new List<PersonaData>();

        private List<DeOperacionData> selectedTools = new();

        private LoadingModal loadingModal;

        private bool isDisabled = true;


        protected override async void OnInitialized()
        {
            editContextForm = new EditContext(dataOperacionForm);
            dataSelectUbicacion = await _ubicacionDao.GetAllUbicacion();
            dataSelectPersona = await _personaDao.GetAllPersonas();
        }

        protected override async void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {

                await GetData();
            }
        }

        private async Task GetData()
        {

            data = await _herramientaDao.GetAllHerramientas();

            if (data?.Count() == 0)
                _toast.ShowWarning("No se encontraron registros");

            if (reintegroId != "0")
            {
                dataOperacionInsert = await _operacionDao.GetDataOperacion(reintegroId);
                selectedTools = await _operacionDao.GetAllDeOperacion(reintegroId) ?? new List<DeOperacionData>();
                dataOperacionForm = dataOperacionInsert;
                editContextForm = new EditContext(dataOperacionForm);

                StateHasChanged();

            }

            StateHasChanged();
        }

        private void OnToolSelect(GridRowEventArgs<HerramientaData> args)

        {

            var tool = args.Item;

            if (!selectedTools.Any(t => t.CODIGO == tool.CODIGO))

            {

                var newTool = new DeOperacionData

                {

                    HERRAMIENTAID = tool.HERRAMIENTAID,
                    DESCRIPCION = tool.NOMBRE,
                    CODIGO = tool.CODIGO,
                    CANTIDAD = 0,
                    VALORCOMPRA = 0
                };

                selectedTools.Add(newTool);

                StateHasChanged();

            }

        }


        public bool ToolExists(string codigo)
        {
            return selectedTools.Any(t => t.CODIGO == codigo);
        }

        public void UpdateToolQuantity(string codigo, int cantidad)
        {
            var tool = selectedTools.FirstOrDefault(t => t.CODIGO == codigo);
            if (tool != null)
            {
                tool.CANTIDAD = cantidad;
            }
        }

        public void UpdateToolValue(string codigo, decimal valor)
        {
            var tool = selectedTools.FirstOrDefault(t => t.CODIGO == codigo);
            if (tool != null)
            {
                tool.VALORCOMPRA = valor;
            }
        }

        private async Task RemoveTool(DeOperacionData tool)
        {
            if (reintegroId != "0")
            {
                await _operacionDao.EliminarDeOperacion(tool.DEOPERACIONID);
                selectedTools = await _operacionDao.GetAllDeOperacion(reintegroId) ?? new List<DeOperacionData>();
            }
            else
            {
                if (selectedTools == null)
                {
                    selectedTools = new List<DeOperacionData>();
                }

                selectedTools.RemoveAll(t => t.CODIGO == tool.CODIGO);
            }

            StateHasChanged();
        }

        public async void OnKeyUpSearch(string textFilter)
        {

            dataDB = await _herramientaDao.GetAllHerramientas(textFilter);

            if (dataDB != null && dataDB.Count > 0)
                data = dataDB.AsQueryable().ToList();
            else
                _toast.ShowWarning("No se encontraron registros");

            textFilter = "";

            StateHasChanged();
        }
        private async Task Guardar()
        {

            string tipoOperacion = "3";
            dataOperacionInsert = dataOperacionForm;
            dataOperacionInsert.TIPOOPERACIONID = tipoOperacion;

            if (reintegroId.Equals("0"))
            {
                JsonDataResult json = await _operacionDao.InsertarOperacion(dataOperacionInsert);

                DataTable table = (DataTable)JsonConvert.DeserializeObject(json.CONTENIDO.ToString(), typeof(DataTable));

                if (table == null || table.Rows.Count == 0)
                    return;

                int success = Convert.ToInt32(table.Rows[0]["OSUCCESS"].ToString());
                string response = (string)table.Rows[0]["RESPONSE"];

                if (success != 1)
                {
                    _toast.ShowWarning(response);
                    return;
                }

                int Id = Convert.ToInt32(table.Rows[0]["VID"].ToString());

                foreach (var row in selectedTools)
                {
                    DeOperacionData itemData = new DeOperacionData
                    {
                        OPERACIONID = Id.ToString(),
                        HERRAMIENTAID = row.HERRAMIENTAID,
                        CANTIDAD = row.CANTIDAD,
                        VALORCOMPRA = row.VALORCOMPRA
                    };
                    JsonDataResult jsonItems = await _operacionDao.InsertarDeOperacion(itemData);
                }

                _toast.ShowSuccess(response);
                _navigationManager.NavigateTo("/Reintegros");
                return;
            }
            else
            {

                JsonDataResult json = await _operacionDao.ActualizarOperacion(dataOperacionInsert);

                DataTable table = (DataTable)JsonConvert.DeserializeObject(json.CONTENIDO.ToString(), typeof(DataTable));

                if (table == null || table.Rows.Count == 0)
                    return;

                int success = Convert.ToInt32(table.Rows[0]["OSUCCESS"].ToString());
                string response = (string)table.Rows[0]["RESPONSE"];

                if (success != 1)
                {
                    _toast.ShowWarning(response);
                    loadingModal.Hide();
                    return;
                }


                foreach (var row in selectedTools)
                {
                    DeOperacionData itemData = new DeOperacionData
                    {
                        OPERACIONID = dataOperacionForm.OPERACIONID.ToString(),
                        HERRAMIENTAID = row.HERRAMIENTAID,
                        CANTIDAD = row.CANTIDAD,
                        VALORCOMPRA = row.VALORCOMPRA
                    };
                    JsonDataResult jsonItems = await _operacionDao.InsertarDeOperacion(itemData);
                }

                _toast.ShowSuccess(response);
                _navigationManager.NavigateTo("/Reintegros");
                return;
            }
        }
    }
}
