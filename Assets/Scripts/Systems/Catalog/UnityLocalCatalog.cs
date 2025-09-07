using System.Collections.Generic;
using System.Linq;

public class UnityLocalCatalog : ICatalogSystem
{
	private IReadOnlyList<ICatalogItem> items;

	public void Initialize(ISystemManager systemManager)
	{
		items = systemManager.Get<IAssetLoadSystem>().LoadAssets<ICatalogItem>();
	}

	public IReadOnlyList<ICatalogItem> GetItems() => items.ToList();

	public IReadOnlyList<ICatalogItem> GetItemsByType(ItemType type) => items.Where(x => x.Type == type).ToList();

	public ICatalogItem GetItemById(string id) => items.FirstOrDefault(x => x.Id == id);

	public void Dispose()
	{
		items = null;
	}
}