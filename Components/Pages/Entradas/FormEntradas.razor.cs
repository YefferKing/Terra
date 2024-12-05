using BlazorBootstrap;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Terra.Components.Layout.Components;
using Terra.Dao.Herramientas;
using Terra.Dao.Parametrizacion.Personas;
using Terra.Dao.Ubicacion;
using Terra.Models.DeOperacion;
using Terra.Models.Entradas;
using Terra.Models.Herramientas;
using Terra.Models.Parametrizacion.GrupoSanguineo;
using Terra.Models.Parametrizacion.Personas;
using Terra.Models.Ubicacion;

namespace Terra.Components.Pages.Entradas
{
    public partial class FormEntradas
    {
        [Inject]
        private HerramientaDao _herramientaDao { get; set; }

        [Inject]
        private IToastService _toast { get; set; }

        [Parameter]
        public string entradaId { get; set; }

        [Inject]
        private NavigationManager _navigationManager { get; set; }

        [Inject]
        private UbicacionDao _ubicacionDao { get; set; }

        [Inject]
        private PersonaDao _personaDao { get; set; }

        List<HerramientaData> data = new List<HerramientaData>();


        private OperacionData dataOperacionInsert = new OperacionData();

        private OperacionData dataOperacionForm = new OperacionData();

        private EditContext editContextForm;

        private List<UbicacionData> dataSelectUbicacion = new List<UbicacionData>();

        private List<PersonaData> dataSelectPersona = new List<PersonaData>();

        private List<DeOperacionData> selectedTools = new();


        protected override async void OnInitialized()
        {
            editContextForm = new EditContext(dataOperacionForm);
            dataSelectUbicacion = await _ubicacionDao.GetAllUbicacion();
            dataSelectPersona = await _personaDao.GetAllPersonas();
        }

        protected override async void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {

                await GetData();
            }
        }

        private async Task GetData()
        {

            data = await _herramientaDao.GetAllHerramientas();

            if (data?.Count() == 0)
                _toast.ShowWarning("No se encontraron registros");

            StateHasChanged();
        }

        private void OnToolSelect(GridRowEventArgs<HerramientaData> args)

        {

            var tool = args.Item;

            if (!selectedTools.Any(t => t.CODIGO == tool.CODIGO))

            {

                // Create a new instance to avoid reference issues

                var newTool = new DeOperacionData

                {

                    HERRAMIENTAID = tool.HERRAMIENTAID,
                    DESCRIPCION = tool.DESCRIPCION,
                    CODIGO = tool.CODIGO,
                    CANTIDAD = 0,
                    VALORCOMPRA = 0
                };

                selectedTools.Add(newTool);

                StateHasChanged();

            }

        }



        private void RemoveTool(DeOperacionData tool)

        {

            selectedTools.RemoveAll(t => t.CODIGO == tool.CODIGO);

            StateHasChanged();

        }
    }
}
