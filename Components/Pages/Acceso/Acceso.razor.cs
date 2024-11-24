using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Terra.Components.Layout.Components;
using Terra.Dao.Usuario;
using Terra.Models.Usuario;
using Terra.Service;

namespace Terra.Components.Pages.Acceso
{
    public partial class Acceso
    {
        [Inject]
        private UsuarioDao _usuarioDao { get; set; }

        [Inject]
        private IToastService _toastService { get; set; }

        [Inject]
        private NavigationManager _navigationManager { get; set; }

        [Inject]
        private AuthService _authService { get; set; }

        [Inject]
        private IJSRuntime JSRuntime { get; set; }

        private LoadingModal loadingModal;


        private UsuarioData usuario = new UsuarioData();


        private async Task IniciarSesion()
        {
            loadingModal.Show();

            // Retardo de 500ms para asegurar que el modal se vea
            await Task.Delay(500);

            bool resultado = await _usuarioDao.Login(usuario.Usuario, usuario.Contraseña);

            if (!resultado)
            {
                _toastService.ShowError("Usuario o Contraseña Incorrecta.");
            }
            else
            {
                await JSRuntime.InvokeVoidAsync("sessionStorage.setItem", "user_session_valid", "true");
                await JSRuntime.InvokeVoidAsync("sessionStorage.setItem", "DASHBOARDMENSAJE", "Bienvenido al Dashboard!");
                _navigationManager.NavigateTo("/Dashboard");
            }

            loadingModal.Hide();
        }
    }
}
