public class InputBlocker
{
    private static InputBlocker _instance;

    public static InputBlocker Instance
    {
        get
        {
            if (_instance == null)
                _instance = new InputBlocker();
            return _instance;
        }
    }

    private InputBlocker() { }

    public bool blockScrollZoom = false;
}
