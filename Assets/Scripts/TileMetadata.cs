using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMetadata
{
	public bool IsBonfire { get; set; }
	public TilePosition Position { get; set; }

	public TileMetadata(bool isBonfire, int x, int y): this(isBonfire, new TilePosition(x,y)) { }

	public TileMetadata(bool isBonfire, TilePosition position) {
		IsBonfire = isBonfire;
		Position = position;
	}
}
