using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Terra.Components.Layout.Components;
using Terra.Dao.Herramientas;
using Terra.Models.Herramientas;
using BlazorBootstrap;
using Terra.Dao.Parametrizacion.Personas;
using Terra.Models.Parametrizacion.Personas;
using Terra.Dao.Parametrizacion.Cargos;

namespace Terra.Components.Pages.Herramientas
{
    public partial class GridHerramientas
    {
        [Inject]
        private IToastService _toast { get; set; }

        [Inject]
        private NavigationManager _navigationManager { get; set; }

        [Inject]
        private HerramientaDao _herramientaDao { get; set; }

        List<HerramientaData> data = new List<HerramientaData>();

        List<HerramientaData> dataDB = new List<HerramientaData>();

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

            data = await _herramientaDao.GetAllHerramientas();

            if (data?.Count() == 0)
                _toast.ShowWarning("No se encontraron registros");

            loadingModal.Hide();

            StateHasChanged();
        }

        private async Task FormHerramienta(string Id = "0")
        {
            _navigationManager.NavigateTo($"/Herramientas/{Id}");
        }

        private async Task OnRowDoubleClick(GridRowEventArgs<HerramientaData> args) => await FormHerramienta(args.Item.HERRAMIENTAID);

        private void ShowConfirmDelete(string id)
        {
            SelectId = id;
            confirmDelete.ShowAsync();
        }

        private async Task Eliminar()
        {
            loadingModal.Show();

            confirmDelete.HideAsync();

            bool result = await _herramientaDao.EliminarHerramientas(SelectId);

            if (result)
            {
                data = await _herramientaDao.GetAllHerramientas();
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
            loadingModal.Hide();

            dataDB = await _herramientaDao.GetAllHerramientas(textFilter);

            if (dataDB != null && dataDB.Count > 0)
                data = dataDB.AsQueryable().ToList();
            else
                _toast.ShowWarning("No se encontraron registros");

            textFilter = "";

            loadingModal.Hide();

            StateHasChanged();
        }
    }
}
