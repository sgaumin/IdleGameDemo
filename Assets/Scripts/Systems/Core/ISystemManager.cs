using System;

public interface ISystemManager : IDisposable
{
	T Get<T>() where T : ISystem;
}