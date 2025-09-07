public interface ILoggingSystem : ISystem
{
	public void Log(string message);
	public void LogWarning(string message);
	public void LogError(string message);
}