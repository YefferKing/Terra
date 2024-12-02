using BlazorBootstrap;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Terra.Components.Layout.Components;
using Terra.Dao.Parametrizacion.Cargos;
using Terra.Dao.Ubicacion;
using Terra.Models.Parametrizacion.Cargos;
using Terra.Models.Ubicacion;

namespace Terra.Components.Pages.Cargos
{
    public partial class GridCargos
    {
        [Inject]
        private IToastService _toast { get; set; }

        [Inject]
        private NavigationManager _navigationManager { get; set; }

        [Inject]
        private CargoDao _cargoDao { get; set; }

        List<CargoData> data = new List<CargoData>();

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

            data = await _cargoDao.GetAllCargos();

            if (data?.Count() == 0)
                _toast.ShowWarning("No se encontraron registros");

            loadingModal.Hide();

            StateHasChanged();
        }

        private async Task FormCargo(string Id = "0")
        {
            _navigationManager.NavigateTo($"/Cargos/{Id}");
        }

        private async Task OnRowDoubleClick(GridRowEventArgs<CargoData> args) => await FormCargo(args.Item.CARGOID);

        private void ShowConfirmDelete(string id)
        {
            SelectId = id;
            confirmDelete.ShowAsync();
        }

        private async Task Eliminar()
        {
            loadingModal.Show();

            confirmDelete.HideAsync();

            bool result = await _cargoDao.EliminarCargo(SelectId);

            if (result)
            {
                data = await _cargoDao.GetAllCargos();
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
