using System;

public interface ISystem : IDisposable
{
	public void Initialize(ISystemManager systemManager);
}