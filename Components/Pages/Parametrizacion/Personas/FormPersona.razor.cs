using BlazorBootstrap;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Newtonsoft.Json;
using System.Data;
using Terra.Commons;
using Terra.Components.Layout.Components;
using Terra.Dao.Parametrizacion.Cargos;
using Terra.Dao.Parametrizacion.GrupoSanguineo;
using Terra.Dao.Parametrizacion.NivelAcademico;
using Terra.Dao.Parametrizacion.Pais;
using Terra.Dao.Parametrizacion.Personas;
using Terra.Models;
using Terra.Models.Parametrizacion.Cargos;
using Terra.Models.Parametrizacion.GrupoSanguineo;
using Terra.Models.Parametrizacion.NivelAcademico;
using Terra.Models.Parametrizacion.Pais;
using Terra.Models.Parametrizacion.Personas;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Terra.Components.Pages.Parametrizacion.Personas
{
    public partial class FormPersona
    {
        [Parameter]
        public string personaId { get; set; }

        [Inject]
        private IToastService _toast { get; set; }

        [Inject]
        private NavigationManager _navigationManager { get; set; }

        [Inject]
        private PersonaDao _personaDao { get; set; }

        [Inject]
        private PaisDao _paisDao { get; set; }

        [Inject]
        private GrupoSanguineoDao _grupo { get; set; }

        [Inject]
        private NivelAcademicoDao _academico { get; set; }

        [Inject]
        private CargoDao _cargo { get; set; }


        private List<ItemsData> dataItem = new List<ItemsData>();

        private LoadingModal loadingModal;

        private PersonaData dataPersonaInsert = new PersonaData();

        private PersonaData dataPersonaForm = new PersonaData();

        private EditContext editContextForm;

        private List<PaisData> dataSelectPais = new List<PaisData>();

        private List<GrupoSanguineoData> dataSelectGrupoSanguineo = new List<GrupoSanguineoData>();

        private List<NivelAcademicoData> dataSelectAcademico = new List<NivelAcademicoData>();

        private List<CargoData> dataSelectCargo = new List<CargoData>();

        private List<TipoItemsGrid> dataSelectTipoItems = new List<TipoItemsGrid>();

        private TipoItemsGrid dataTipoItemsForm = new();

        private bool isEditItems = false;

        private Modal modalItems;

        private Modal confirmDelete;

        private Grid<ItemsData> grid;

        private string SelectId;


        protected override async void OnInitialized()
        {
            editContextForm = new EditContext(dataPersonaForm);

            dataSelectPais = await _paisDao.GetAllPais();
            dataSelectGrupoSanguineo = await _grupo.GetAllGrupoSanguineo();
            dataSelectAcademico = await _academico.GetAllNivelAcademico();
            dataSelectCargo = await _cargo.GetAllCargos();

        }


        protected override async void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                loadingModal.Show();

                await GetData();

                StateHasChanged();

                loadingModal.Hide();

            }
        }

        private async Task GetData()
        {
            if (personaId != "0")
            {
                dataPersonaInsert = await _personaDao.GetDataPersona(personaId);

                dataItem = await _personaDao.GetAllItems(dataPersonaInsert.PERSONAID) ?? new List<ItemsData>();

                CalcularEdad(dataPersonaInsert.FECHANACIMIENTO);

                dataPersonaForm = dataPersonaInsert;
                editContextForm = new EditContext(dataPersonaForm);

                StateHasChanged();

            }
        }

        private async Task GuardarPersona()
        {
            loadingModal.Show();

            dataPersonaInsert = dataPersonaForm;

            if (personaId.Equals("0"))
            {

                JsonDataResult json = await _personaDao.InsertarPersona(dataPersonaInsert);

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

                int personaId = Convert.ToInt32(table.Rows[0]["VID"].ToString());

                foreach(var row in dataItem)
                {
                    ItemsData itemData = new ItemsData
                    {
                        PERSONAID = personaId.ToString(),
                        TIPOITEMID = row.TIPOITEMID,
                        CONTENIDO = row.CONTENIDO
                    };
                    JsonDataResult jsonItems = await _personaDao.InsertarItems(itemData);
                }

                _toast.ShowSuccess(response);
                loadingModal.Hide();
                _navigationManager.NavigateTo("/Parametrizacion/Tablas/Personas");
                return;
            }
            else
            {

                JsonDataResult json = await _personaDao.ActualizarPersona(dataPersonaInsert);

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


                foreach (var row in dataItem)
                {
                    ItemsData itemData = new ItemsData
                    {
                        PERSONAID = dataPersonaForm.PERSONAID,
                        TIPOITEMID = row.TIPOITEMID,
                        CONTENIDO = row.CONTENIDO
                    };
                    JsonDataResult jsonItems = await _personaDao.InsertarItems(itemData);
                }

                _toast.ShowSuccess(response);
                loadingModal.Hide();
                _navigationManager.NavigateTo("/Parametrizacion/Tablas/Personas");
                return;
            }
        }

        private void CalcularEdad(DateTime fechaNacimiento)
        {
            if (fechaNacimiento != DateTime.MinValue)
            {
                var hoy = DateTime.Today;
                var edad = hoy.Year - fechaNacimiento.Year;

                if (fechaNacimiento.Date > hoy.AddYears(-edad))
                {
                    edad--;
                }

                dataPersonaInsert.edad = edad;
                dataPersonaForm.edad = edad;
            }
        }

        private async void showModal()
        {
            isEditItems = false;

            dataSelectTipoItems = await _personaDao.GetAllTipoItems();

            dataTipoItemsForm = new();

            await modalItems.ShowAsync();

            StateHasChanged();
        }

        private void OnTipoItemChanged(string tipoItemId)
        {
            var selectedTipoItem = dataSelectTipoItems.FirstOrDefault(item => item.TIPOITEMID == tipoItemId);

            if (selectedTipoItem != null)
            {
                dataTipoItemsForm.DESCRIPITEM = selectedTipoItem.DESCRIPITEM;
            }
        }

        private async Task GuardarModalItems()
        {
            if (!isEditItems)
            {
                var nuevoItem = new ItemsData
                {
                    TIPOITEMID = dataTipoItemsForm.TIPOITEMID,
                    TIPO = dataTipoItemsForm.DESCRIPITEM,
                    CONTENIDO = dataTipoItemsForm.CONTENIDO
                };

                dataItem.Add(nuevoItem);
                grid.RefreshDataAsync();

            }
            else
            {
                var itemExistente = dataItem.FirstOrDefault(x => x.ITEMID == dataTipoItemsForm.TIPOITEMID);
                if (itemExistente != null)
                {
                    itemExistente.TIPO = dataTipoItemsForm.DESCRIPITEM;
                    itemExistente.CONTENIDO = dataTipoItemsForm.CONTENIDO;
                }
            }

            await modalItems.HideAsync();
            StateHasChanged();
            _toast.ShowSuccess(isEditItems ? "Ítem actualizado con éxito." : "Ítem agregado con éxito.");
        }

        private void ShowConfirmDelete(string id)
        {
            SelectId = id;
            confirmDelete.ShowAsync();
        }

        private async Task Eliminar()
        {
            loadingModal.Show();

            confirmDelete.HideAsync();

            bool result = await _personaDao.EliminarItem(SelectId);

            if (result)
            {
                dataItem = await _personaDao.GetAllItems(dataPersonaInsert.PERSONAID) ?? new List<ItemsData>();
                _toast.ShowSuccess("Registro eliminado con exito.");
            }
            else
            {
                _toast.ShowError("No se ha podido eliminar el registro.");
            }

            loadingModal.Hide();

            StateHasChanged();

        }


    }
}
