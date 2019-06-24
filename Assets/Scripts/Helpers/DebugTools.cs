
using UnityEngine;


static public class DebugTools
{
	static public void Log(string message)
	{
		Debug.Log(message);
	}

	static public void Log(string message, params object[] args)
	{
		message = string.Format(message, args);
		Debug.Log(message);
	}

	static public void LogWarning(string message)
	{
		Debug.LogWarning(message);
	}

	static public void LogWarning(string message, params object[] args)
	{
		message = string.Format(message, args);
		Debug.LogWarning(message);
	}

	static public void LogError(string message)
	{
		Debug.LogError(message);
	}

	static public void LogError(string message, params object[] args)
	{
		message = string.Format(message, args);
		Debug.LogError(message);
	}
}
