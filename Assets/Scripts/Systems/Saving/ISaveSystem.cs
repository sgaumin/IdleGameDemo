public interface ISaveSystem : ISystem
{
	public void Save(string id, object data);
	public T Load<T>(string id);
	public bool HasKey(string id);
}