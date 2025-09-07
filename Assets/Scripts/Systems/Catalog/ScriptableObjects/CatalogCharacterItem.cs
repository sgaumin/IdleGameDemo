using UnityEngine;

[CreateAssetMenu(fileName = "CatalogCharacterItem", menuName = "ScriptableObjects/CatalogCharacterItem", order = 1)]
public class CatalogCharacterItem : ScriptableObject, ICatalogCharacterItem
{
	[SerializeField] private string id;
	[SerializeField] private ItemType type;
	[SerializeField] private ItemRarity rarity;
	[SerializeField] private bool isStackable;
	[SerializeField] private float variableName;
	[SerializeField] private AnimationCurve levelUpCost;
	[SerializeField] private AnimationCurve rewardMultiplier;
	[SerializeField] private AnimationCurve attackSpeed;

	public string Id => id;
	public ItemType Type => type;
	public ItemRarity Rarity => rarity;
	public bool IsStackable => isStackable;
	public int MaxLevel => Mathf.FloorToInt(levelUpCost.keys[^1].time);

	public int GetLevelUpCost(int level) => Mathf.FloorToInt(levelUpCost.Evaluate(level));

	public int GetRewardMultiplier(int level) => Mathf.FloorToInt(rewardMultiplier.Evaluate(level));

	public float GetAttackSpeed(int level) => attackSpeed.Evaluate(level);
}