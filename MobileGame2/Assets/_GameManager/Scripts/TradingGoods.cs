using UnityEngine;
using System.Collections;

public class TradingGoods {

	public enum GoodsType { 
		AgriProduct,
		MiningOre,
		Electronics,
		HeavyIndustry,
		ShipParts,
		Weaponry,
		Leisure
	};
	
	public GoodsType type;
	public string name;
	public string unit;
	public int numUnits;
	public float sellingPrice;
	public float buyingPrice;
	
	public TradingGoods()
	{

	}
}
