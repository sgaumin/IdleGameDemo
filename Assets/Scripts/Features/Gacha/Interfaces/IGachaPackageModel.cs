public interface IGachaPackageModel
{
	public string ItemId { get; }
	public int Amount { get; }
	public ItemRarity Rarity { get; }
}