using UnityEngine;

/// <summary>
/// Wrapper around Unity's <see cref="Debug"/> class to implement <see cref="ILoggingSystem"/>. 
/// </summary>
/// <remarks>
/// Prevent logs on non-development builds for performance optimization.
/// </remarks>
public class UnityDebugLogger : ILoggingSystem
{
	public void Initialize(ISystemManager systemManager) { }

	public void Log(string message)
	{
#if UNITY_EDITOR || DEVELOPMENT_BUILD
		Debug.Log(message);
#endif
	}

	public void LogWarning(string message)
	{
#if UNITY_EDITOR || DEVELOPMENT_BUILD
		Debug.LogWarning(message);
#endif
	}

	public void LogError(string message)
	{
#if UNITY_EDITOR || DEVELOPMENT_BUILD
		Debug.LogError(message);
#endif
	}

	public void Dispose() { }
}