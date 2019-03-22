using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
	public static GameObject[] treePrefabs;
    public TileMetadata Metadata;

	public void Init(TileMetadata md) {
		Metadata = md;
		treePrefabs = treePrefabs ?? Resources.LoadAll<GameObject>("Prefabs/Trees");
	
		PoissonDiscSampler pds;
		float size = 5 * Mathf.Sqrt(2);

		List<GridPos> illegal = new List<GridPos>();
		for (int i = 0; i < 10; i++) {
			for (int j = 0; j < 10; j++) {
				//Each tile is made up of 10 squares
				//for bonfires, skip 3 through 6
				if (md.IsBonfire && i > 3 && i < 7 && j > 3 && j < 7) {
					illegal.Add(new GridPos(i, j));
				}
			}
		}
		pds = new PoissonDiscSampler(40f - 1, 40f - 1, size, illegal);

		foreach (var position in pds.Samples()) {
			var treePrefab = treePrefabs[0];
			GameObject g = Instantiate(
				treePrefab, 
				transform.position + new Vector3(position.x - 20f, treePrefab.transform.localScale.y / 2f, position.y - 20f) + Vector3.down * 0.5f, 
				Quaternion.Euler(0, Random.value * 360, 0), 
				this.transform
			);
			var treeSize = Random.Range(0.4f, 0.6f);
			g.transform.localScale = Vector3.one * treeSize; 
		}
	}
}
