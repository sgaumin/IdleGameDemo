using System;
using System.Collections.Generic;

public interface IInventorySystem : ISystem
{
	public Action OnInventoryChanged { get; set; }
	public void AddItem(string id, int quantity = 1);
	public bool RemoveItem(string id, int quantity = 1);
	public int GetItemQuantity(string id);
	public InventoryItem GetItem(string id);
	public InventoryItem GetItemInstance(string instanceId);
	public List<InventoryItem> GetItemsByType(ItemType type);
	public int GetItemCountOfType(ItemType type);
}