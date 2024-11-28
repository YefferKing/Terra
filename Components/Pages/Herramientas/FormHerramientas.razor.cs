using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Newtonsoft.Json;
using System.Data;
using Terra.Components.Layout.Components;
using Terra.Dao.Herramientas;
using Terra.Dao.Parametrizacion.Pais;
using Terra.Dao.Parametrizacion.Personas;
using Terra.Models;
using Terra.Models.Herramientas;
using Terra.Models.Parametrizacion.Personas;

namespace Terra.Components.Pages.Herramientas
{
    public partial class FormHerramientas
    {
        [Parameter]
        public string herramientaId { get; set; }

        [Inject]
        private IToastService _toast { get; set; }

        [Inject]
        private NavigationManager _navigationManager { get; set; }

        [Inject]
        private HerramientaDao _herramientaDao { get; set; }

        private LoadingModal loadingModal;

        private HerramientaData dataHerramientaInsert = new HerramientaData();

        private HerramientaData dataHerramientaForm = new HerramientaData();

        private EditContext editContextForm;


        protected override async void OnInitialized()
        {
            editContextForm = new EditContext(dataHerramientaForm);

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
            if (herramientaId != "0")
            {
                dataHerramientaInsert = await _herramientaDao.GetDataHerramienta(herramientaId);


                dataHerramientaForm = dataHerramientaInsert;
                editContextForm = new EditContext(dataHerramientaForm);

                StateHasChanged();

            }
        }

        private async Task Guardar()
        {
            loadingModal.Show();

            dataHerramientaInsert = dataHerramientaForm;

            if (herramientaId.Equals("0"))
            {

                JsonDataResult json = await _herramientaDao.InsertarHerramienta(dataHerramientaInsert);

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
                _navigationManager.NavigateTo("/Herramientas");
                return;
            }
            else
            {

                JsonDataResult json = await _herramientaDao.ActualziarHerramienta(dataHerramientaInsert);

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
                _navigationManager.NavigateTo("/Herramientas");
                return;
            }
        }

    }
}
