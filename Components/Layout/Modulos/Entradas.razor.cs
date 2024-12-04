using Microsoft.AspNetCore.Components;

namespace Terra.Components.Layout.Modulos
{
    public partial class Entradas
    {
        [Inject]
        private NavigationManager _navigationManager { get; set; }

        private async Task Onclick()
        {
            _navigationManager.NavigateTo("/Entradas");
        }
    }
}
