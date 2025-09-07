using System.Collections.Generic;

public interface ICatalogSystem : ISystem
{
	public IReadOnlyList<ICatalogItem> GetItems();
	public IReadOnlyList<ICatalogItem> GetItemsByType(ItemType type);
	public ICatalogItem GetItemById(string id);
}