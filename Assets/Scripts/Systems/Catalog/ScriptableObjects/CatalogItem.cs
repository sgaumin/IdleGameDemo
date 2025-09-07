using UnityEngine;

[CreateAssetMenu(fileName = "CatalogItem", menuName = "ScriptableObjects/CatalogItem", order = 1)]
public class CatalogItem : ScriptableObject, ICatalogItem
{
	[SerializeField] private string id;
	[SerializeField] private ItemType type;
	[SerializeField] private ItemRarity rarity;
	[SerializeField] private bool isStackable = true;

	public string Id => id;
	public ItemType Type => type;
	public ItemRarity Rarity => rarity;
	public bool IsStackable => isStackable; // Determine if new instance should be created if false

}