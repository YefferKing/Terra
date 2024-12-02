using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using System.Data;
using Terra.Components.Layout.Components;
using Terra.Dao.Herramientas;
using Terra.Models.Herramientas;
using Terra.Models;
using Terra.Dao.Ubicacion;
using Terra.Models.Ubicacion;

namespace Terra.Components.Pages.Ubicacion
{
    public partial class FormUbicacion
    {
        [Parameter]
        public string ubicacionId { get; set; }

        [Inject]
        private IToastService _toast { get; set; }

        [Inject]
        private NavigationManager _navigationManager { get; set; }

        [Inject]
        private UbicacionDao _ubicacionDao { get; set; }

        private LoadingModal loadingModal;

        private UbicacionData dataUbicacionInsert = new UbicacionData();

        private UbicacionData dataUbicacionForm = new UbicacionData();

        private EditContext editContextForm;


        protected override async void OnInitialized()
        {
            editContextForm = new EditContext(dataUbicacionForm);

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
            if (ubicacionId != "0")
            {
                dataUbicacionInsert = await _ubicacionDao.GetDataUbicacion(ubicacionId);


                dataUbicacionForm = dataUbicacionInsert;
                editContextForm = new EditContext(dataUbicacionForm);

                StateHasChanged();

            }
        }

        private async Task Guardar()
        {
            loadingModal.Show();

            dataUbicacionInsert = dataUbicacionForm;

            if (ubicacionId.Equals("0"))
            {

                JsonDataResult json = await _ubicacionDao.InsertarUbicacion(dataUbicacionInsert);

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
                _navigationManager.NavigateTo("/Ubicacion");
                return;
            }
            else
            {

                JsonDataResult json = await _ubicacionDao.ActualizarUbicacion(dataUbicacionInsert);

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
                _navigationManager.NavigateTo("/Ubicacion");
                return;
            }
        }
    }
}
