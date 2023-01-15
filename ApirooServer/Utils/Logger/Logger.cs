using System.Globalization;

namespace ApirooServer.Utils
{
    public class Logger<T> : Singleton<Logger<T>>
    {

        public List<string> Logs { get; private set; } = new List<string>();
        public Logger() => Log($"{typeof(T).Name} logger instancied", LogLevel.Info);
        public void Log(string logMessage, LogLevel level) => Logs.Add($"[{level}] [{DateTime.Now.ToString(new CultureInfo("fr-FR").DateTimeFormat)}] {logMessage}");
    }
    public enum LogLevel
    {
        Neutral = 0,
        Info = 1,
        Error = 2,
        Critical = 3
    }
}
