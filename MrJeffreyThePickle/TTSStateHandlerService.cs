namespace MrJeffreyThePickle;

public static class TTSStateHandlerService
{
    private static bool _isResponsesTTS = false;

    public static bool IsResponsesTts
    {
        get => _isResponsesTTS;
        set => _isResponsesTTS = value;
    }
}