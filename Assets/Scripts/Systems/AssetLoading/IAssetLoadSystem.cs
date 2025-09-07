using System.Collections.Generic;

public interface IAssetLoadSystem : ISystem
{
	public T LoadAsset<T>(string id) where T : class;

	public List<T> LoadAssets<T>() where T : class;

	public void UnloadUnusedAssets();
}