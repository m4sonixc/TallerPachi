namespace PachiTaller;

public partial class EstadoTurnoPage : ContentPage
{
    private readonly DatabaseService _databaseService;

    public EstadoTurnoPage()
    {
        InitializeComponent();
        _databaseService = new DatabaseService();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CargarTurnosUsuarioAsync();
    }

    private async Task CargarTurnosUsuarioAsync()
    {
        try
        {
            // Leemos el nombre exacto del cliente y su rol desde la sesión global
            string nombreUsuario = App.UsuarioSesionActual?.Nombre ?? "";
            string rolUsuario = App.RolSesionActual ?? "Cliente";

            var turnosFiltrados = await _databaseService.ObtenerTurnosPorUsuarioAsync(nombreUsuario, rolUsuario);

            ListaTurnosUsuarioView.ItemsSource = null;
            ListaTurnosUsuarioView.ItemsSource = turnosFiltrados;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error al cargar turnos: {ex.Message}");
        }
    }

    private async void OnVolverInicioClicked(object sender, EventArgs e)
    {
        try
        {
            string rutaInicio = App.RolSesionActual == "Admin" ? "//InicioAdmin" : "//InicioUsuario";
            await Shell.Current.GoToAsync(rutaInicio);
        }
        catch
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}