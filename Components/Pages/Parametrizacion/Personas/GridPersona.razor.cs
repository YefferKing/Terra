using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Terra.Models.Parametrizacion.Personas;
using Terra.Dao.Parametrizacion.Personas;
using Terra.Components.Layout.Components;
using BlazorBootstrap;
using Terra.Commons;
using Terra.Models.Herramientas;
using Terra.Dao.Herramientas;

namespace Terra.Components.Pages.Parametrizacion.Personas
{
    public partial class GridPersona
    {
        [Inject]
        private IToastService _toast { get; set; }

        [Inject]
        private NavigationManager _navigationManager { get; set; }

        [Inject]
        private PersonaDao _personaDao { get; set; }


        List<PersonaData> data = new List<PersonaData>();

        List<PersonaData> dataDB = new List<PersonaData>();

        private LoadingModal loadingModal;
        private Modal confirmDelete;
        private string SelectId;

        protected override async void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                loadingModal.Show();

                await GetData();
            }
        }

        private async Task GetData()
        {
            loadingModal.Show();

            data = await _personaDao.GetAllPersonas("");

            if (data?.Count() == 0)
                _toast.ShowWarning("No se encontraron registros");

            loadingModal.Hide();

            StateHasChanged();
        }

        private async Task FormPersona(string personaId = "0")
        {
            _navigationManager.NavigateTo($"/Parametrizacion/Tablas/Personas/{personaId}");
        }

        private async Task OnRowDoubleClick(GridRowEventArgs<PersonaData> args) => await FormPersona(args.Item.PERSONAID);

        private void ShowConfirmDelete(string id)
        {
            SelectId = id;
            confirmDelete.ShowAsync();
        }

        private async Task Eliminar()
        {
            loadingModal.Show();

            confirmDelete.HideAsync();

            bool result = await _personaDao.EliminarPersona(SelectId);

            if (result)
            {
                data = await _personaDao.GetAllPersonas();
                _toast.ShowSuccess("Registro eliminado con exito.");
            }
            else
            {
                _toast.ShowError("No se ha podido eliminar el registro.");
            }

            loadingModal.Hide();

            StateHasChanged();

        }

        public async void OnKeyUpSearch(string textFilter)
        {

            dataDB = await _personaDao.GetAllPersonas(textFilter);

            if (dataDB != null && dataDB.Count > 0)
                data = dataDB.AsQueryable().ToList();
            else
                _toast.ShowWarning("No se encontraron registros");

            textFilter = "";

            StateHasChanged();
        }
    }
}
