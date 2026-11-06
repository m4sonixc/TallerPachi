namespace PachiTaller;

public partial class Historial : ContentPage
{
    private readonly DatabaseService _databaseService;

    public Historial()
    {
        InitializeComponent();
        _databaseService = new DatabaseService();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CargarHistorialDesdeApi();
    }

    private async Task CargarHistorialDesdeApi()
    {
        try
        {
            // Mantenemos las variables de sesión (se usarán en futuros pasos para filtrar en la API)
            string nombreUsuario = App.UsuarioSesionActual?.Nombre ?? "";
            string rolUsuario = App.RolSesionActual ?? "Cliente";

            // 1. Instanciamos el nuevo servicio que conecta a la API
            var apiService = new TurnosApiService();

            // 2. Obtenemos los datos desde el servidor (por ahora traerá la lista de prueba)
            var historial = await apiService.ObtenerTurnosAsync();

            // 3. Actualizamos la vista visual de la pantalla
            ListaHistorialView.ItemsSource = null;
            ListaHistorialView.ItemsSource = historial;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error al cargar el historial: {ex.Message}");
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