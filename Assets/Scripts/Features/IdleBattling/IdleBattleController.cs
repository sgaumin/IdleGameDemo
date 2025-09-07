using System;
using System.Collections.Generic;
using UnityEngine;

public class IdleBattleController : MonoBehaviour
{
	public Action<InventoryCharacterItem, CatalogCharacterItem> OnCharacterChanged;

	private const string SAVE_KEY = "IDLE_BATTLE_SAVE";
	private const string CLOSING_TIME = "ClosingTime";
	private const string DEFAULT_CHARACTER_ID = "Charles";

	[SerializeField] private IdleBattleView view;

	private ICatalogSystem catalog;
	private IInventorySystem inventory;
	private ISaveSystem saveSystem;
	private ILoggingSystem logger;
	private IdleBattleModel model;
	private InventoryCharacterItem character;
	private CatalogCharacterItem catalogCharacter;

	private float elapsedTime = 0f;

	private void Start()
	{
		catalog = Game.Systems.Get<ICatalogSystem>();
		inventory = Game.Systems.Get<IInventorySystem>();
		saveSystem = Game.Systems.Get<ISaveSystem>();
		logger = Game.Systems.Get<ILoggingSystem>();

		model = saveSystem.HasKey(SAVE_KEY) ? saveSystem.Load<IdleBattleModel>(SAVE_KEY) : new();

		view.Initialize(this, LevelUpCharacter, LoadPreviousCharacter, LoadNextCharacter);

		LoadCharacter();
		ComputeIdleAwayRewards();
	}

	private void Update()
	{
		if (elapsedTime >= GetAttackSpeed())
		{
			inventory.AddItem(CurrencyType.Silver.ToString(), GetRewardMultiplier());
			elapsedTime = 0f;
		}

		elapsedTime += Time.deltaTime;
	}

	private void LoadCharacter()
	{
		if (string.IsNullOrEmpty(model.CharacterInstanceId))
		{
			inventory.AddItem(DEFAULT_CHARACTER_ID);
			character = (InventoryCharacterItem)inventory.GetItem(DEFAULT_CHARACTER_ID);
			model.CharacterInstanceId = character.InstanceId;
		}
		else
		{
			character = (InventoryCharacterItem)inventory.GetItemInstance(model.CharacterInstanceId);
		}

		catalogCharacter = (CatalogCharacterItem)catalog.GetItemById(character.Id);
		OnCharacterChanged?.Invoke(character, catalogCharacter);
	}

	private void ComputeIdleAwayRewards()
	{
		if (!saveSystem.HasKey(CLOSING_TIME)) return;

		DateTime closingTime = saveSystem.Load<DateTime>(CLOSING_TIME);
		TimeSpan elapsed = DateTime.Now - closingTime;
		int rewardCycles = (int)(elapsed.TotalSeconds / GetAttackSpeed());
		if (rewardCycles > 0)
		{
			int totalReward = rewardCycles * GetRewardMultiplier();
			inventory.AddItem(CurrencyType.Silver.ToString(), totalReward);
			logger.Log($"{nameof(IdleBattleController)}: <color=green>Granted {totalReward} Silver for {rewardCycles} reward cycles during offline time of {elapsed.Hours}h {elapsed.Minutes}m {elapsed.Seconds}s.</color>");
		}
	}

	private int GetLevelUpCost()
	{
		if (character.Level >= catalogCharacter.MaxLevel) return -1;
		return catalogCharacter.GetLevelUpCost(character.Level + 1);
	}

	private float GetAttackSpeed() => catalogCharacter.GetAttackSpeed(character.Level);

	private int GetRewardMultiplier() => catalogCharacter.GetRewardMultiplier(character.Level);

	private void LevelUpCharacter()
	{
		int cost = GetLevelUpCost();
		if (cost < 0)
		{
			logger.Log($"{nameof(IdleBattleController)}: <color=yellow>Character is already at max level.</color>");
			return;
		}

		if (inventory.GetItemQuantity(CurrencyType.Silver.ToString()) >= cost)
		{
			inventory.RemoveItem(CurrencyType.Silver.ToString(), cost);
			character.Level++;
			saveSystem.Save(SAVE_KEY, model);
			OnCharacterChanged?.Invoke(character, catalogCharacter);

			logger.Log($"{nameof(IdleBattleController)}: <color=green>Character leveled up to {character.Level}!</color>");
		}
		else
		{
			logger.Log($"{nameof(IdleBattleController)}: <color=red>Not enough {CurrencyType.Silver} to level up. Required: {cost}, Available: {inventory.GetItemQuantity(CurrencyType.Silver.ToString())}</color>");
		}
	}

	private void LoadPreviousCharacter()
	{
		LoadCharacterByDirection(-1);
	}

	private void LoadNextCharacter()
	{
		LoadCharacterByDirection(1);
	}

	private void LoadCharacterByDirection(int direction)
	{
		List<InventoryItem> characters = inventory.GetItemsByType(ItemType.Character);
		int currentIndex = characters.FindIndex(c => c.InstanceId == character.InstanceId);
		int index = (currentIndex + direction + characters.Count) % characters.Count;
		character = (InventoryCharacterItem)characters[index];
		model.CharacterInstanceId = character.InstanceId;

		saveSystem.Save(SAVE_KEY, model);
		catalogCharacter = (CatalogCharacterItem)catalog.GetItemById(character.Id);

		elapsedTime = 0f;
		OnCharacterChanged?.Invoke(character, catalogCharacter);

		logger.Log($"{nameof(IdleBattleController)}: <color=yellow>Loaded character {character.Id} (Instance ID: {character.InstanceId})</color>");
	}

	private void OnDestroy()
	{
		saveSystem.Save(SAVE_KEY, model);
		saveSystem.Save(CLOSING_TIME, DateTime.Now);
	}
}