using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GachaView : MonoBehaviour
{
	[SerializeField] private Button spinButton;
	[SerializeField] private TMP_Text costText;

	private int spinCost;
	private IInventorySystem inventory;

	public void Initialize(int spinCost, Action OnSpinClicked)
	{
		this.spinCost = spinCost;

		inventory = Game.Systems.Get<IInventorySystem>();
		inventory.OnInventoryChanged += OnInventoryChanged;

		spinButton.onClick.AddListener(() => OnSpinClicked?.Invoke());
	}

	private void OnDestroy()
	{
		spinButton.onClick.RemoveAllListeners();
		inventory.OnInventoryChanged -= OnInventoryChanged;
	}

	private void OnInventoryChanged()
	{
		bool hasEnoughCurrency = inventory.GetItemQuantity(CurrencyType.Silver.ToString()) >= spinCost;
		string color = hasEnoughCurrency ? "green" : "red";
		costText.SetText($"<color={color}>cost {spinCost}</color>");
	}
}