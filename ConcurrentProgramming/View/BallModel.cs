using System.ComponentModel;
using System.Diagnostics;

public class BallModel : INotifyPropertyChanged
{
    private double x, y;

    public double X
    {
        get => x;
        set
        {
            if (x != value)
            {
                x = value;
                System.Diagnostics.Debug.WriteLine($"Zmieniono X na {x}"); // Debugowanie
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
                System.Diagnostics.Debug.WriteLine($"Zmieniono Y na {y}"); // Debugowanie
                OnPropertyChanged(nameof(Y));
            }
        }
    }

    public double Radius { get; set; } = 20;

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string name)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
