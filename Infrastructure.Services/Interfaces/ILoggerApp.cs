namespace Infrastructure.Services.Interfaces
{
    public interface ILoggerApp
    {
        void Trace(string message);

        void Debug(string message);

        void Info(string message);

        void Fatal(string message);
    }
}