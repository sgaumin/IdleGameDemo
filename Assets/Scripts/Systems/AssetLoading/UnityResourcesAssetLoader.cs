using System;
using System.Collections.Generic;
using UnityEngine;

public class UnityResourcesAssetLoader : IAssetLoadSystem
{
	public void Initialize(ISystemManager systemManager) { }

	public T LoadAsset<T>(string id) where T : class
	{
		if (!typeof(UnityEngine.Object).IsAssignableFrom(typeof(T)))
		{
			throw new ArgumentException($"UnityResourcesAssetLoader requires types that inherit from UnityEngine.Object");
		}
		return (T)(object)Resources.Load(id, typeof(T));
	}

	public List<T> LoadAssets<T>() where T : class
	{
		if (typeof(T).IsInterface)
		{
			return LoadAssetsByInterface<T>();
		}
		else
		{
			if (!typeof(UnityEngine.Object).IsAssignableFrom(typeof(T)))
			{
				throw new ArgumentException($"UnityResourcesAssetLoader requires types that inherit from UnityEngine.Object");
			}

			UnityEngine.Object[] objects = Resources.LoadAll("", typeof(T));
			List<T> results = new List<T>();

			for (int i = 0; i < objects.Length; i++)
			{
				results.Add((T)(object)objects[i]);
			}

			return results;
		}
	}

	private List<T> LoadAssetsByInterface<T>() where T : class
	{
		UnityEngine.Object[] allObjects = Resources.LoadAll<UnityEngine.Object>("");
		List<T> results = new();

		foreach (UnityEngine.Object obj in allObjects)
		{
			if (obj is T interfaceImplementation)
			{
				results.Add(interfaceImplementation);
			}
		}

		return results;
	}

	public void UnloadUnusedAssets()
	{
		Resources.UnloadUnusedAssets();
	}

	public void Dispose() { }
}