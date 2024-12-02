using Microsoft.AspNetCore.Components;

namespace Terra.Components.Layout.Modulos
{
    public partial class Cargos
    {
        [Inject]
        private NavigationManager _navigationManager { get; set; }

        private async Task Onclick()
        {
            _navigationManager.NavigateTo("/Cargos");
        }
    }
}
