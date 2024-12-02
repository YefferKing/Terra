using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using System.Data;
using Terra.Components.Layout.Components;
using Terra.Dao.Ubicacion;
using Terra.Models.Ubicacion;
using Terra.Models;
using Terra.Dao.Parametrizacion.Cargos;
using Terra.Models.Parametrizacion.Cargos;

namespace Terra.Components.Pages.Cargos
{
    public partial class FormCargos
    {
        [Parameter]
        public string cargoId { get; set; }

        [Inject]
        private IToastService _toast { get; set; }

        [Inject]
        private NavigationManager _navigationManager { get; set; }

        [Inject]
        private CargoDao _cargoDao { get; set; }

        private LoadingModal loadingModal;

        private CargoData dataCargoInsert = new CargoData();

        private CargoData dataCargoForm = new CargoData();

        private EditContext editContextForm;


        protected override async void OnInitialized()
        {
            editContextForm = new EditContext(dataCargoForm);

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
            if (cargoId != "0")
            {
                dataCargoInsert = await _cargoDao.GetDataCargo(cargoId);


                dataCargoForm = dataCargoInsert;
                editContextForm = new EditContext(dataCargoForm);

                StateHasChanged();

            }
        }

        private async Task Guardar()
        {
            loadingModal.Show();

            dataCargoInsert = dataCargoForm;

            if (cargoId.Equals("0"))
            {

                JsonDataResult json = await _cargoDao.InsertarCargo(dataCargoInsert);

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

                _toast.ShowSuccess(response);
                loadingModal.Hide();
                _navigationManager.NavigateTo("/Cargos");
                return;
            }
            else
            {

                JsonDataResult json = await _cargoDao.ActualizarCargo(dataCargoInsert);

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

                _toast.ShowSuccess(response);
                loadingModal.Hide();
                _navigationManager.NavigateTo("/Cargos");
                return;
            }
        }
    }
}
