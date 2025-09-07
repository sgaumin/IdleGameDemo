using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IdleBattleView : MonoBehaviour
{
	[Header("Buttons")]
	[SerializeField] private Button levelUpButton;
	[SerializeField] private Button loadNextButton;
	[SerializeField] private Button loadPreviousButton;

	[Header("Stats")]
	[SerializeField] private TMP_Text nameText;
	[SerializeField] private TMP_Text idText;
	[SerializeField] private TMP_Text levelText;
	[SerializeField] private TMP_Text rewardText;
	[SerializeField] private TMP_Text speedText;
	[SerializeField] private TMP_Text levelUpCostText;
	[SerializeField] private TMP_Text characterCountText;

	private IdleBattleController controller;
	private IInventorySystem inventory;
	private int levelUpCost;
	private InventoryCharacterItem character;

	public void Initialize(IdleBattleController controller, Action OnLevelUpClicked, Action OnPreviousClicked, Action OnNextClicked)
	{
		this.controller = controller;

		inventory = Game.Systems.Get<IInventorySystem>();
		UpdateWithInventory();

		controller.OnCharacterChanged += OnCharacterChanged;
		inventory.OnInventoryChanged += UpdateWithInventory;
		levelUpButton.onClick.AddListener(() => OnLevelUpClicked?.Invoke());
		loadNextButton.onClick.AddListener(() => OnNextClicked?.Invoke());
		loadPreviousButton.onClick.AddListener(() => OnPreviousClicked?.Invoke());
	}

	private void OnDestroy()
	{
		loadPreviousButton.onClick.RemoveAllListeners();
		loadNextButton.onClick.RemoveAllListeners();
		levelUpButton.onClick.RemoveAllListeners();
		inventory.OnInventoryChanged -= UpdateWithInventory;
		controller.OnCharacterChanged -= OnCharacterChanged;
	}

	private void OnCharacterChanged(InventoryCharacterItem character, CatalogCharacterItem catalogCharacter)
	{
		this.character = character;

		nameText.SetText(character.Id);
		idText.SetText(character.InstanceId.Substring(0, 4));
		levelText.SetText(character.Level.ToString());
		rewardText.SetText(catalogCharacter.GetRewardMultiplier(character.Level).ToString());
		speedText.SetText(catalogCharacter.GetAttackSpeed(character.Level).ToString());

		levelUpCost = character.Level >= catalogCharacter.MaxLevel ? -1 : catalogCharacter.GetLevelUpCost(character.Level + 1);

		UpdateLevelUpCostDisplay();
		UpdateCharacterCount();
	}

	private void UpdateWithInventory()
	{
		UpdateCharacterLoadStatus();
		UpdateLevelUpCostDisplay();
		UpdateCharacterCount();
	}

	private void UpdateLevelUpCostDisplay()
	{
		levelUpButton.interactable = levelUpCost >= 0 && inventory.GetItemQuantity(CurrencyType.Silver.ToString()) >= levelUpCost;

		if (levelUpCost < 0)
		{
			levelUpCostText.SetText("<color=yellow>max</color>");
			return;
		}

		bool hasEnoughCurrency = inventory.GetItemQuantity(CurrencyType.Silver.ToString()) >= levelUpCost;
		string color = hasEnoughCurrency ? "green" : "red";
		levelUpCostText.SetText($"<color={color}>cost {levelUpCost}</color>");
	}

	private void UpdateCharacterLoadStatus()
	{
		int characterCount = inventory.GetItemCountOfType(ItemType.Character);
		loadNextButton.gameObject.SetActive(characterCount > 1);
		loadPreviousButton.gameObject.SetActive(characterCount > 1);
	}

	private void UpdateCharacterCount()
	{
		if (character == null)
		{
			// If no character is loaded, clear the text
			characterCountText.SetText("");
			return;
		}

		List<InventoryItem> characters = inventory.GetItemsByType(ItemType.Character);
		if (characters.Count < 1)
		{
			// If only one character exists, clear the text
			characterCountText.SetText("");
			return;
		}

		int currentIndex = characters.FindIndex(c => c.InstanceId == character.InstanceId);
		characterCountText.SetText($"{currentIndex + 1}/{characters.Count}");
	}
}