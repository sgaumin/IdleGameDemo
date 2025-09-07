using System.Collections.Generic;

public interface IGachaModel
{
	public int CostPerPull { get; }
	public int ItemsPerPull { get; }
	public Dictionary<ItemRarity, int> WeightPerRarity { get; }
	public IList<IGachaPackageModel> Packages { get; }
}