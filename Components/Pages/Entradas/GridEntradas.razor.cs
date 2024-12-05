using BlazorBootstrap;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Terra.Commons;
using Terra.Components.Layout.Components;
using Terra.Dao.Herramientas;
using Terra.Dao.Operacion;
using Terra.Models.Entradas;
using Terra.Models.Herramientas;

namespace Terra.Components.Pages.Entradas
{
    public partial class GridEntradas
    {
        [Inject]
        private IToastService _toast { get; set; }

        [Inject]
        private NavigationManager _navigationManager { get; set; }

        [Inject]
        private OperacionDao _operacionDao { get; set; }

        List<OperacionData> data = new List<OperacionData>();

        List<OperacionData> dataDB = new List<OperacionData>();

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

            data = await _operacionDao.GetAllOperacion();

            if (data?.Count() == 0)
                _toast.ShowWarning("No se encontraron registros");

            loadingModal.Hide();

            StateHasChanged();
        }

        private async Task FormOperacion(string Id = "0")
        {
            _navigationManager.NavigateTo($"/Entradas/{Id}");
        }

        public async void OnKeyUpSearch(string textFilter)
        {
            loadingModal.Hide();

            dataDB = await _operacionDao.GetAllOperacion();

            if (dataDB != null && dataDB.Count > 0)
                data = dataDB.AsQueryable().ToList();
            else
                _toast.ShowWarning("No se encontraron registros");

            textFilter = "";

            loadingModal.Hide();

            StateHasChanged();
        }

        private async Task OnRowDoubleClick(GridRowEventArgs<OperacionData> args) => await FormOperacion(args.Item.OPERACIONID);

        private void ShowConfirmDelete(string id)
        {
            SelectId = id;
            confirmDelete.ShowAsync();
        }

        private async Task Eliminar()
        {
            loadingModal.Show();

            confirmDelete.HideAsync();

            bool result = await _operacionDao.EliminarOperacion(SelectId);

            if (result)
            {
                data = await _operacionDao.GetAllOperacion();
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
