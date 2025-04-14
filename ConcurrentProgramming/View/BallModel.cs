using System.ComponentModel;

public class BallModel : INotifyPropertyChanged
{
    private double x;
    private double y;
    private double radius = 20;

    public double X
    {
        get => x;
        set { x = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(X))); }
    }

    public double Y
    {
        get => y;
        set { y = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Y))); }
    }

    public double Radius
    {
        get => radius;
        set { radius = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Radius))); }
    }

    public event PropertyChangedEventHandler PropertyChanged;
}
