using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StationInfo : GalaxyObject {

	public enum StationType { Unknown, Agricultural, HeavyManufacturing, HighTech, ShipYard, Garrison, Leisure };
	public enum StationState { Unknown, Normal, UnderAttack, Captured, Destroyed };
	
	// station info
	public StationType type;
	public StationState state;
	
	// trading goods
	public float restockRate;
	public List<TradingGoods> tradingStock;
	
	public StationInfo()
	{
		type = StationType.Unknown;
		state = StationState.Normal;
		
		tradingStock = new List<TradingGoods>();
	}
		
}
