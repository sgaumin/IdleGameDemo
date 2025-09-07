using System;
using System.Collections.Generic;
using System.Linq;

public class UnityLocalInventory : IInventorySystem
{
	private const string SAVE_KEY = "UNITY_LOCAL_INVENTORY_SAVE";

	public Action OnInventoryChanged { get; set; }

	private ICatalogSystem catalog;
	private ISaveSystem saveSystem;
	private ILoggingSystem logger;
	private List<InventoryItem> inventory;

	public void Initialize(ISystemManager systemManager)
	{
		catalog = systemManager.Get<ICatalogSystem>();
		saveSystem = systemManager.Get<ISaveSystem>();
		logger = systemManager.Get<ILoggingSystem>();

		inventory = saveSystem.HasKey(SAVE_KEY) ? saveSystem.Load<List<InventoryItem>>(SAVE_KEY) : new();
	}

	public void AddItem(string id, int quantity = 1)
	{
		ICatalogItem catalogItem = catalog.GetItemById(id);
		InventoryItem item = inventory.FirstOrDefault(x => x.Id == id);

		if (item != null && catalogItem.IsStackable)
		{
			item.Quantity += quantity;
		}
		else
		{
			switch (catalogItem.Type)
			{
				case ItemType.Currency:
				case ItemType.Consumable:
					item = new InventoryItem
					{
						Id = id,
						InstanceId = catalogItem.IsStackable ? null : Guid.NewGuid().ToString(),
						Quantity = quantity,
						Type = catalogItem.Type,
					};
					break;
				case ItemType.Character:
					item = new InventoryCharacterItem
					{
						Id = id,
						InstanceId = catalogItem.IsStackable ? null : Guid.NewGuid().ToString(),
						Quantity = quantity,
						Type = catalogItem.Type,
						Level = 1
					};
					break;
				default:
					break;
			}

			inventory.Add(item);
		}

		logger.Log($"{nameof(UnityLocalInventory)}: <color=green>Added</color> <color=yellow>{quantity}</color> of {item.Type} item {id} -> new quantity: <color=green>{item.Quantity}</color>");
		saveSystem.Save(SAVE_KEY, inventory);
		OnInventoryChanged?.Invoke();
	}

	public bool RemoveItem(string id, int quantity = 1)
	{
		InventoryItem item = inventory.FirstOrDefault(x => x.Id == id);
		if (item != null && item.Quantity >= quantity)
		{
			item.Quantity -= quantity;
			logger.Log($"{nameof(UnityLocalInventory)}: <color=red>Removed</color> <color=yellow>{quantity}</color> of {item.Type} item {id} -> new quantity: <color=red>{item.Quantity}</color>");
			if (item.Quantity == 0)
			{
				inventory.Remove(item);
				logger.Log($"{nameof(UnityLocalInventory)}: <color=red>Removed item {id} from inventory as quantity reached zero.</color>");

				saveSystem.Save(SAVE_KEY, inventory);
				OnInventoryChanged?.Invoke();
			}
			return true;
		}
		return false;
	}

	public int GetItemQuantity(string id)
	{
		InventoryItem item = inventory.FirstOrDefault(x => x.Id == id);
		return item != null ? item.Quantity : 0;
	}

	public InventoryItem GetItem(string id)
	{
		return inventory.FirstOrDefault(x => x.Id == id);
	}

	public InventoryItem GetItemInstance(string instanceId)
	{
		return inventory.FirstOrDefault(x => x.InstanceId == instanceId);
	}

	public List<InventoryItem> GetItemsByType(ItemType type)
	{
		return inventory.Where(x => x.Type == type).ToList();
	}

	public int GetItemCountOfType(ItemType type)
	{
		return inventory.Count(x => x.Type == type);
	}

	public void Dispose() { }
}