using UnityEngine;

[CreateAssetMenu(fileName = "GachaPackage", menuName = "ScriptableObjects/GachaPackageModel", order = 1)]
public class GachaPackageModel : ScriptableObject, IGachaPackageModel
{
	[SerializeField] private string itemId;
	[SerializeField, Range(0, 10000)] private int amount = 1;
	[SerializeField] private ItemRarity rarity;

	public string ItemId => itemId;
	public int Amount => amount;
	public ItemRarity Rarity => rarity;
}