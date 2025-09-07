using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "GachaModel", menuName = "ScriptableObjects/GachaModel", order = 1)]
public class GachaModel : ScriptableObject, IGachaModel
{
	[SerializeField, Range(0, 1000)] private int costPerPull;
	[SerializeField, Range(0, 10)] private int itemsPerPull;
	[SerializeField] private RarityWeight[] rarityWeights;
	[SerializeField] private List<GachaPackageModel> packages;

	private Dictionary<ItemRarity, int> weightPerRarity;

	public int CostPerPull => costPerPull;
	public int ItemsPerPull => itemsPerPull;
	public Dictionary<ItemRarity, int> WeightPerRarity
	{
		get
		{
			if (weightPerRarity == null)
				weightPerRarity = rarityWeights?.ToDictionary(rw => rw.rarity, rw => rw.weight) ?? new Dictionary<ItemRarity, int>();

			return weightPerRarity;
		}
	}
	public IList<IGachaPackageModel> Packages => packages?.Cast<IGachaPackageModel>().ToList() ?? new List<IGachaPackageModel>();
}