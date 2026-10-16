namespace PachiTaller
{
    public partial class UsuarioMainPage : ContentPage
    {
        public UsuarioMainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Obtenemos el nombre del usuario autenticado en App.UsuarioSesionActual
            string nombreReal = App.UsuarioSesionActual?.Nombre ?? "Usuario";

            // Actualizamos la etiqueta con el nombre del usuario
            LblBienvenida.Text = $"Bienvenido, {nombreReal}";
        }

        private async void OnCerrarSesionClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//Login");
        }

        private async void OnRegistrarTurnoClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//Turnos");
        }

        private async void OnEstadoTurnoClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//EstadoTurno");
        }

        private async void OnHistorialTurnosClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//Historial");
        }
    }
}