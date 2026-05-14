using Universal_Control_Full.Services;

namespace Universal_Control_Full;

public partial class MainPage : ContentPage
{
    private readonly IIRService _irService;
    private int _currentTemp = 24;

    // Nuevo Perfil: TGM Inverter (Protocolo basado en Gree - 38kHz)
    private readonly int[] _tgmInverterPattern = {
        // Cabecera inicial
        9000, 4500, 
        // Bloque 1 de datos
        600, 540, 600, 1600, 600, 540, 600, 540, 600, 1600, 600, 540, 600, 540, 600, 540,
        600, 540, 600, 540, 600, 540, 600, 1600, 600, 540, 600, 540, 600, 540, 600, 540,
        600, 540, 600, 540, 600, 540, 600, 540, 600, 540, 600, 1600, 600, 540, 600, 540,
        600, 540, 600, 1600, 600, 540, 600, 1600, 600, 540, 600, 540, 600, 540, 600, 540,
        600, 1600, 600, 1600, 600, 540, 600, 1600,
        // Espacio separador obligatorio en protocolo Gree
        600, 20000, 
        // Cabecera secundaria
        9000, 4500, 
        // Bloque 2 de datos
        600, 540, 600, 540, 600, 540, 600, 540, 600, 1600, 600, 540, 600, 540, 600, 540,
        600, 540, 600, 540, 600, 540, 600, 540, 600, 540, 600, 540, 600, 540, 600, 540,
        600, 540, 600, 540, 600, 540, 600, 540, 600, 540, 600, 540, 600, 540, 600, 540,
        600, 540, 600, 540, 600, 540, 600, 540, 600, 540, 600, 1600, 600, 540, 600, 1600,
        // Cierre
        600, 20000
    };

    private readonly int[] _samsungPowerPattern = {
        4500, 4500,
        560, 1690, 560, 1690, 560, 1690, 560, 560, 560, 560, 560, 560, 560, 560, 560, 560,
        560, 1690, 560, 1690, 560, 1690, 560, 560, 560, 560, 560, 560, 560, 560, 560, 560,
        560, 560, 560, 1690, 560, 560, 560, 560, 560, 560, 560, 560, 560, 560, 560, 560,
        560, 1690, 560, 560, 560, 1690, 560, 1690, 560, 1690, 560, 1690, 560, 1690, 560, 1690,
        560 // Bit de parada
    };

    public MainPage(IIRService irService)
    {
        InitializeComponent();
        _irService = irService;

        if (!_irService.HasIrEmitter())
        {
            StatusLabel.Text = "Hardware IR no detectado en este dispositivo";
            StatusLabel.TextColor = Colors.Red;
            PowerBtn.IsEnabled = false;
        }
    }

    private void OnPowerClicked(object sender, EventArgs e)
    {
        try
        {
            // Apuntando directo al sensor del aire acondicionado
            _irService.Transmit(38000, _tgmInverterPattern);
            StatusLabel.Text = "Comando TGM Inverter (Gree) enviado";
            StatusLabel.TextColor = Colors.LightGreen;
        }
        catch (Exception ex)
        {
            StatusLabel.Text = "Error: " + ex.Message;
        }
    }

    private void OnSamsungClicked(object sender, EventArgs e)
    {
        try
        {
            // Transmisión a 38kHz
            _irService.Transmit(38000, _samsungPowerPattern);
            StatusLabel.Text = "Comando Samsung enviado";
            StatusLabel.TextColor = Colors.LightBlue;
        }
        catch (Exception ex)
        {
            StatusLabel.Text = "Error: " + ex.Message;
            StatusLabel.TextColor = Colors.Red;
        }
    }

    private void OnTempUpClicked(object sender, EventArgs e)
    {
        if (_currentTemp < 30)
        {
            _currentTemp++;
            TempLabel.Text = $"{_currentTemp}°C";
            StatusLabel.Text = $"Ajustado a {_currentTemp}°C";
        }
    }

    private void OnTempDownClicked(object sender, EventArgs e)
    {
        if (_currentTemp > 16)
        {
            _currentTemp--;
            TempLabel.Text = $"{_currentTemp}°C";
            StatusLabel.Text = $"Ajustado a {_currentTemp}°C";
        }
    }
}