using System.ComponentModel;

public class BallModel : INotifyPropertyChanged
{
    private double x, y;

    public double X
    {
        get => x;
        set
        {
            if (Math.Abs(x - value) > 0.01) // Użyj tolerancji dla double
            {
                x = value;
                //System.Diagnostics.Debug.WriteLine($"X changed to {value}");
                OnPropertyChanged(nameof(X));
            }
        }
    }

    public double Y
    {
        get => y;
        set
        {
            if (y != value)
            {
                y = value;
                OnPropertyChanged(nameof(Y));  // Powiadomienie UI o zmianie
            }
        }
    }

    public double Radius { get; set; } = 10;

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string name)
    {
        // Wydrukuj, aby upewnić się, że zmiana jest powiadamiana
        Console.WriteLine($"Właściwość {name} została zmieniona");
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
