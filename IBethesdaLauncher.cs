namespace Fallout76.Proxy
{
    public interface IBethesdaLauncher
    {
        bool IsInstalled { get; }
        bool IsActive { get; }

        void Start(BethesdaGameType GameIdx);
        void Stop();
    }
}
