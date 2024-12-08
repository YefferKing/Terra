using BlazorBootstrap;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Terra.Components.Layout.Components;
using Terra.Dao.Herramientas;
using Terra.Dao.Ubicacion;
using Terra.Models.Herramientas;
using Terra.Models.Ubicacion;

namespace Terra.Components.Pages.Ubicacion
{
    public partial class GridUbicacion
    {
        [Inject]
        private IToastService _toast { get; set; }

        [Inject]
        private NavigationManager _navigationManager { get; set; }

        [Inject]
        private UbicacionDao _ubicacionDao { get; set; }

        List<UbicacionData> data = new List<UbicacionData>();

        List<UbicacionData> dataDB = new List<UbicacionData>();

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

            data = await _ubicacionDao.GetAllUbicacion("");

            if (data?.Count() == 0)
                _toast.ShowWarning("No se encontraron registros");

            loadingModal.Hide();

            StateHasChanged();
        }

        private async Task FormUbicacion(string Id = "0")
        {
            _navigationManager.NavigateTo($"/Ubicacion/{Id}");
        }

        private async Task OnRowDoubleClick(GridRowEventArgs<UbicacionData> args) => await FormUbicacion(args.Item.UBICACIONID);

        private void ShowConfirmDelete(string id)
        {
            SelectId = id;
            confirmDelete.ShowAsync();
        }

        private async Task Eliminar()
        {
            loadingModal.Show();

            confirmDelete.HideAsync();

            bool result = await _ubicacionDao.EliminarUbicacion(SelectId);

            if (result)
            {
                data = await _ubicacionDao.GetAllUbicacion();
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

            dataDB = await _ubicacionDao.GetAllUbicacion(textFilter);

            if (dataDB != null && dataDB.Count > 0)
                data = dataDB.AsQueryable().ToList();
            else
                _toast.ShowWarning("No se encontraron registros");

            textFilter = "";

            StateHasChanged();
        }
    }
}
