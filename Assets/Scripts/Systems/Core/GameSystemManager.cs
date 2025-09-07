using System;
using System.Collections.Generic;

public class GameSystemManager : ISystemManager
{
	private Dictionary<Type, ISystem> systems;

	public GameSystemManager()
	{
		// Note: This is where we could choose which interface implementation to use, based on platform target, scripting defines, custom config, etc.
		systems = new()
		{
			{ typeof(ILoggingSystem), new UnityDebugLogger() },
			{ typeof(ISaveSystem), new UnityLocalSaver() },
			{ typeof(IAssetLoadSystem), new UnityResourcesAssetLoader() },
			{ typeof(ICatalogSystem), new UnityLocalCatalog() },
			{ typeof(IInventorySystem), new UnityLocalInventory() }, // Note: We could have setup & choose another implementations (PlayFabInventory(), AWSInventory(), etc..) for online inventories.
		};

		foreach (ISystem system in systems.Values)
		{
			system.Initialize(this);
		}
	}

	public void Dispose()
	{
		Get<IInventorySystem>().Dispose();
		Get<ICatalogSystem>().Dispose();
		Get<IAssetLoadSystem>().Dispose();
		Get<ISaveSystem>().Dispose();
		Get<ILoggingSystem>().Dispose();
	}

	public T Get<T>() where T : ISystem
	{
		if (systems.TryGetValue(typeof(T), out ISystem system))
		{
			return (T)system;
		}

		throw new InvalidOperationException($"System of type {typeof(T).Name} is not registered.");
	}
}