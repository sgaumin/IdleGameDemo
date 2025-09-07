using TMPro;
using UnityEngine;

public class SilverCountDisplayer : MonoBehaviour
{
	private IInventorySystem inventorySystem;
	private TMP_Text text;

	private void Start()
	{
		inventorySystem = Game.Systems.Get<IInventorySystem>();
		text = GetComponent<TMP_Text>();

		// Call once to initialize display
		OnInventoryChanged();

		// Event driven update
		inventorySystem.OnInventoryChanged += OnInventoryChanged;
	}

	private void OnInventoryChanged()
	{
		int silverCount = inventorySystem.GetItemQuantity("Silver");
		text.SetText($"<color=grey>Silver</color> {silverCount}");
	}

	private void OnDestroy()
	{
		inventorySystem.OnInventoryChanged -= OnInventoryChanged;
	}
}
