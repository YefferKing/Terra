using BlazorBootstrap;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Newtonsoft.Json;
using Terra.Commons;
using Terra.Components.Layout.Components;
using Terra.Dao.Parametrizacion.Cargos;
using Terra.Dao.Parametrizacion.GrupoSanguineo;
using Terra.Dao.Parametrizacion.NivelAcademico;
using Terra.Dao.Parametrizacion.Pais;
using Terra.Dao.Parametrizacion.Personas;
using Terra.Models.Parametrizacion.Cargos;
using Terra.Models.Parametrizacion.GrupoSanguineo;
using Terra.Models.Parametrizacion.NivelAcademico;
using Terra.Models.Parametrizacion.Pais;
using Terra.Models.Parametrizacion.Personas;

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


        List<ItemsData> dataItem = new List<ItemsData>();

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

                bool insertoPersona = await _personaDao.InsertarPersona(dataPersonaInsert);

                if (!insertoPersona)
                {
                    _toast.ShowWarning("No se inserto el registro.");
                    loadingModal.Hide();
                    return;
                }

                _toast.ShowSuccess("Se inserto el registro.");
                loadingModal.Hide();
                _navigationManager.NavigateTo("/Parametrizacion/Tablas/Personas");
                return;
            }
            else
            {

                bool actualizoPersona = await _personaDao.ActualizarPersona(dataPersonaInsert);

                if (!actualizoPersona)
                {
                    _toast.ShowWarning("No se actualizo el registro.");
                    loadingModal.Hide();
                    return;
                }

                _toast.ShowSuccess("registro actualizado con éxito");
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

        private async Task GuardarModalItems()
        {
        }

    }
}
