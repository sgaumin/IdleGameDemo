using UnityEngine;
using UnityEngine.UI;

public class DebugController : MonoBehaviour
{
	// Note: I prefer assigning callbacks via code to avoid accidental unassignment in the inspector
	[SerializeField] private Button plusTenButton;
	[SerializeField] private Button plusHundredButton;
	[SerializeField] private Button plusThousandButton;

	private IInventorySystem inventory;

	private void Start()
	{
		inventory = Game.Systems.Get<IInventorySystem>();

		plusTenButton.onClick.AddListener(Add10);
		plusHundredButton.onClick.AddListener(Add100);
		plusThousandButton.onClick.AddListener(Add1000);
	}

	private void OnDestroy()
	{
		plusTenButton.onClick.RemoveAllListeners();
		plusHundredButton.onClick.RemoveAllListeners();
		plusThousandButton.onClick.RemoveAllListeners();
	}

	private void Add10() => inventory.AddItem(CurrencyType.Silver.ToString(), 10);

	private void Add100() => inventory.AddItem(CurrencyType.Silver.ToString(), 100);

	private void Add1000() => inventory.AddItem(CurrencyType.Silver.ToString(), 1000);
}
