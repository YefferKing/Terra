using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Terra.Components.Layout
{
    public partial class NavMenu
    {
        [Inject]
        private NavigationManager _navigationManager { get; set; }
        [Inject]
        private IJSRuntime JSRuntime { get; set; }

        private bool showModalProfile = false;

        private void ToggleMenu()
        {
            showModalProfile = !showModalProfile;
        }
        private async Task CerrarSesion()
        {

            // Clear session storage
            await JSRuntime.InvokeVoidAsync("sessionStorage.removeItem", "user_session_valid");
            await JSRuntime.InvokeVoidAsync("sessionStorage.removeItem", "DASHBOARDMENSAJE");
            // Redirect to login page
            _navigationManager.NavigateTo("/Login", forceLoad: true);
        }
    }
}
