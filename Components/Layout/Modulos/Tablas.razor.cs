using BlazorBootstrap;
using Microsoft.AspNetCore.Components;

namespace Terra.Components.Layout.Modulos
{
    public partial class Tablas
    {
        [Inject]
        private NavigationManager _navigationManager { get; set; }

        private Collapse ModuloTablaCollapse;
        private Collapse SubMenuPrincipalesCollapse;
        private Collapse SubMenuAuxiliaresCollapse;
        private Collapse SubMenuUbicacionCollapse;

        bool cargandoSubMenuPrincipales;


        private async Task ToggleModuloTablaCollapse() => await ModuloTablaCollapse.ToggleAsync();

        private async Task ToggleSubMenuPrincipalesCollapse() => await SubMenuPrincipalesCollapse.ToggleAsync();

        private async Task Onclick()
        {
            _navigationManager.NavigateTo("/Parametrizacion/Tablas/Personas");
        }

        private async Task ToggleSubMenuAuxiliaresCollapse()
        {
            await SubMenuAuxiliaresCollapse.ToggleAsync();

        }


        private async Task ToggleSubMenuUbicacionCollapse()
        {
            await SubMenuUbicacionCollapse.ToggleAsync();

        }
    }
}
