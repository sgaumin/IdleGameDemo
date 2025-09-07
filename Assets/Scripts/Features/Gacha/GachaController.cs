using System.Collections.Generic;
using UnityEngine;

public class GachaController : MonoBehaviour
{
	private const string GACHA_MODEL_PATH = "Gacha/DefaultGacha";

	[SerializeField] private GachaView view;

	private IInventorySystem inventory;
	private ILoggingSystem logger;
	private GachaModel model;

	public void Start()
	{
		inventory = Game.Systems.Get<IInventorySystem>();
		logger = Game.Systems.Get<ILoggingSystem>();
		model = Game.Systems.Get<IAssetLoadSystem>().LoadAsset<GachaModel>(GACHA_MODEL_PATH);

		view.Initialize(model.CostPerPull, Spin);
	}

	private void Spin()
	{
		logger.Log($"{nameof(GachaController)}: <color=cyan>GACHA SPIN RESULT:</color>");
		logger.Log($"{nameof(GachaController)}: <color=cyan>----------------</color>");
		if (inventory.GetItemQuantity(CurrencyType.Silver.ToString()) >= model.CostPerPull)
		{
			inventory.RemoveItem(CurrencyType.Silver.ToString(), model.CostPerPull);

			List<GachaPackageModel> pool = new();
			foreach (IGachaPackageModel package in model.Packages)
			{
				int amount = model.WeightPerRarity[package.Rarity];
				for (int i = 0; i < amount; i++)
				{
					pool.Add((GachaPackageModel)package);
				}
			}

			List<GachaPackageModel> selection = new();
			for (int i = 0; i < model.ItemsPerPull; i++)
			{
				int randomIndex = Random.Range(0, pool.Count);
				selection.Add(pool[randomIndex]);
			}

			foreach (IGachaPackageModel package in selection)
			{
				inventory.AddItem(package.ItemId, package.Amount);
			}
		}
		else
		{
			logger.Log($"{nameof(GachaController)}: <color=red>Not enough {CurrencyType.Silver.ToString()} to perform gacha spin.</color>");
		}
		logger.Log($"{nameof(GachaController)}: <color=cyan>----------------</color>");
	}

	public void Dispose() { }
}