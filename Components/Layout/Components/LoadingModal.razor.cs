namespace Terra.Components.Layout.Components
{
    public partial class LoadingModal
    {
        private string ViewMensaje = "Cargando ...";
        private bool ShowModal;
        private bool _isProgressBar;

        public string progreso = "0";

        public bool loading { get { return ShowModal; } }

        public void Show(string mensaje = "Cargando", bool isProgressBar = false)
        {
            _isProgressBar = isProgressBar;

            if (string.IsNullOrEmpty(mensaje))
                mensaje = "Cargando";

            if (ViewMensaje != mensaje)
                ViewMensaje = mensaje;

            ShowModal = true;
            StateHasChanged();
        }

        public void Hide()
        {
            ShowModal = false;
            StateHasChanged();
            progreso = "0";
            _isProgressBar = false;
        }

        public void CambiarProgreso(string progreso)
        {
            this.progreso = progreso;
            StateHasChanged();
        }

    }
}
