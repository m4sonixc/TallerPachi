namespace PachiTaller;

public partial class Turnos : ContentPage
{
    private readonly DatabaseService _databaseService;

    public Turnos()
    {
        InitializeComponent();
        _databaseService = new DatabaseService();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CargarVistaSegunRolAsync();
    }

    private async Task CargarVistaSegunRolAsync()
    {
        if (App.RolSesionActual == "Admin")
        {
            // Vista de Administrador: Ve todos los turnos activos en taller
            PanelAdmin.IsVisible = true;
            PanelUsuario.IsVisible = false;

            var todosLosTurnos = await _databaseService.ObtenerTodosLosTurnosAsync();
            var turnosActivos = todosLosTurnos
                .Where(t => t.Estado == "Pendiente" || t.Estado == "En Proceso")
                .ToList();

            ListaTurnosView.ItemsSource = turnosActivos;
        }
        else
        {
            // Vista de Cliente: Muestra el formulario de solicitud
            PanelAdmin.IsVisible = false;
            PanelUsuario.IsVisible = true;

            // Precompletar nombre y teléfono desde la sesión actual de forma obligatoria
            if (App.UsuarioSesionActual != null)
            {
                TxtNombreCliente.Text = App.UsuarioSesionActual.Nombre;
                TxtTelefonoCliente.Text = App.UsuarioSesionActual.Telefono;
            }
        }
    }

    private async void OnEditarTurnoClicked(object sender, EventArgs e)
    {
        try
        {
            var boton = sender as Button;
            var turno = boton?.CommandParameter as TurnoModel;

            if (turno != null)
            {
                var navegacionParametros = new Dictionary<string, object>
                {
                    { "Turno", turno }
                };

                await Shell.Current.GoToAsync(nameof(EditarTurnoPage), navegacionParametros);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error de navegación: {ex.Message}");
            await DisplayAlertAsync("Atención", "No se pudo abrir la pantalla de edición.", "OK");
        }
    }

    private async void OnGuardarTurnoClicked(object sender, EventArgs e)
    {
        // Priorizar siempre la sesión actual del usuario
        string nombreCliente = App.UsuarioSesionActual?.Nombre ?? TxtNombreCliente.Text?.Trim() ?? "";
        string telefonoCliente = App.UsuarioSesionActual?.Telefono ?? TxtTelefonoCliente.Text?.Trim() ?? "";

        if (string.IsNullOrWhiteSpace(nombreCliente) ||
            string.IsNullOrWhiteSpace(TxtVehiculo.Text) ||
            string.IsNullOrWhiteSpace(TxtMotivo.Text))
        {
            await DisplayAlertAsync("Atención", "Por favor, complete todos los campos obligatorios del vehículo y motivo.", "OK");
            return;
        }

        var nuevoTurno = new TurnoModel
        {
            NombreCliente = nombreCliente,
            Telefono = telefonoCliente,
            Vehiculo = TxtVehiculo.Text.Trim(),
            Motivo = TxtMotivo.Text.Trim(),
            FechaTurno = DateTime.Now.ToString("dd/MM/yyyy"),
            Estado = "Pendiente"
        };

        bool exito = await _databaseService.RegistrarTurnoAsync(nuevoTurno);

        if (exito)
        {
            await DisplayAlertAsync("Éxito", "Su turno ha sido registrado correctamente.", "OK");

            // Se limpian únicamente las entradas manuales de vehículo y motivo
            TxtVehiculo.Text = string.Empty;
            TxtMotivo.Text = string.Empty;

            await Shell.Current.GoToAsync("//InicioUsuario");
        }
        else
        {
            await DisplayAlertAsync("Error", "No se pudo registrar el turno. Intente nuevamente.", "OK");
        }
    }

    private async void OnActualizarEstadoClicked(object sender, EventArgs e)
    {
        var boton = sender as Button;
        var turno = boton?.CommandParameter as TurnoModel;

        if (turno != null)
        {
            bool exito = await _databaseService.ActualizarTurnoAdminAsync(
                turno.Id,
                turno.Estado,
                turno.FechaEstimadaTermino,
                turno.Diagnostico
            );

            if (exito)
            {
                await DisplayAlertAsync("Éxito", $"Datos del turno de '{turno.NombreCliente}' actualizados correctamente.", "OK");
                await CargarVistaSegunRolAsync();
            }
            else
            {
                await DisplayAlertAsync("Error", "No se pudieron actualizar los datos del turno.", "OK");
            }
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