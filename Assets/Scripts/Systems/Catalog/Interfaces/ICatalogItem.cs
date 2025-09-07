public interface ICatalogItem
{
	public string Id { get; }
	public ItemType Type { get; }
	public ItemRarity Rarity { get; }

	/// <summary>
	/// Determine if new instance should be created per each addition to inventory, or if existing instance should be updated (stacked).
	/// </summary>
	public bool IsStackable { get; }
}