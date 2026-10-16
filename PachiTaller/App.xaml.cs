namespace PachiTaller;

public partial class App : Application
{
    public static string RolSesionActual { get; set; } = "Cliente";
    public static UsuarioModel? UsuarioSesionActual { get; set; }

    public App()
    {
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell());
    }
}