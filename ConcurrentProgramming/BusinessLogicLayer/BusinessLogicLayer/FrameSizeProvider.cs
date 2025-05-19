public class FrameSizeProvider
{
    private double _width;
    private double _height;
    private readonly object _lock = new();

    public double Width
    {
        get { lock (_lock) { return _width; } }
        set { lock (_lock) { _width = value; } }
    }

    public double Height
    {
        get { lock (_lock) { return _height; } }
        set { lock (_lock) { _height = value; } }
    }

    public FrameSizeProvider(double width, double height)
    {
        _width = width;
        _height = height;
    }
}
