using Newtonsoft.Json;
using UnityEngine;

public class UnityLocalSaver : ISaveSystem
{
	private readonly JsonSerializerSettings settings = new JsonSerializerSettings
	{
		TypeNameHandling = TypeNameHandling.Auto,
		TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple
	};

	public void Initialize(ISystemManager systemManager) { }

	public void Save(string id, object data)
	{
		string json = JsonConvert.SerializeObject(data, settings);
		PlayerPrefs.SetString(id, json);
		PlayerPrefs.Save();
	}

	public T Load<T>(string id)
	{
		if (!PlayerPrefs.HasKey(id)) return default;

		string json = PlayerPrefs.GetString(id);
		return JsonConvert.DeserializeObject<T>(json, settings);
	}

	public bool HasKey(string id)
	{
		return PlayerPrefs.HasKey(id);
	}

	public void Dispose()
	{
		PlayerPrefs.Save();
	}
}