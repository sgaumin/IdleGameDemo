public interface ICatalogCharacterItem : ICatalogItem
{
	public int MaxLevel { get; }
	public int GetLevelUpCost(int level);
	public int GetRewardMultiplier(int level);
	public float GetAttackSpeed(int level);
}