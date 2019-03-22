using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TileGenerator : MonoBehaviour
{
	[SerializeField]
	private Tile tilePrefab;

	private Dictionary<TilePosition, TileMetadata> tileData;
	private Dictionary<TilePosition, Tile> instantiatedTiles;

	public void Start() {
		var startingPosition = new TilePosition(0,0);
		var startingPositionMetadata = new TileMetadata(true, startingPosition);
		tileData = new Dictionary<TilePosition, TileMetadata>();
		tileData.Add(startingPosition, startingPositionMetadata);

		Tile startingTile = GameObject.FindGameObjectWithTag("Ground").GetComponent<Tile>();
		startingTile.Init(startingPositionMetadata);
		instantiatedTiles = new Dictionary<TilePosition, Tile>();
		instantiatedTiles.Add(startingPosition, startingTile);

		GenerateForPosition(startingPosition);
	}

	public void GenerateForPosition(TilePosition tp) {
		List<TilePosition> tilesToGenerate = new List<TilePosition>() {		
			new TilePosition(tp.X - 1,	tp.Y + 1),
			new TilePosition(tp.X,		tp.Y + 1),
			new TilePosition(tp.X + 1,	tp.Y + 1),
			new TilePosition(tp.X - 1,	tp.Y),
			new TilePosition(tp.X,      tp.Y),
			new TilePosition(tp.X + 1,	tp.Y),
			new TilePosition(tp.X - 1,	tp.Y - 1),
			new TilePosition(tp.X,		tp.Y - 1),
			new TilePosition(tp.X + 1,	tp.Y - 1)
		};

		// remove tiles that are not part of the 9 tiles to generate
		List<KeyValuePair<TilePosition, Tile>> tilesToRemove = instantiatedTiles.Where(kvp => !tilesToGenerate.Contains(kvp.Key)).ToList();
		for(int i = tilesToRemove.Count - 1; i >= 0; i--) {
			KeyValuePair<TilePosition, Tile> kvp = tilesToRemove[i];
			instantiatedTiles.Remove(kvp.Key);
			Destroy(kvp.Value.gameObject);
		}

		// generate the new tiles
		foreach (TilePosition pos in tilesToGenerate) {
			if(!instantiatedTiles.ContainsKey(pos)) {
				TileMetadata data = tileData.ContainsKey(pos) ? tileData[pos] : new TileMetadata(false, pos);

				Tile t = Instantiate(tilePrefab, new Vector3(pos.X*40, 0, pos.Y*40), Quaternion.identity);
				t.Init(data);

				instantiatedTiles.Add(pos, t);
			}
		}
	}

	public void AddBonfireTile(TilePosition tp) {
		tileData.Add(tp, new TileMetadata(true, tp));
	}
}
