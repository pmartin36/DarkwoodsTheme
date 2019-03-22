using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent (typeof(InputManager))]
[RequireComponent(typeof(TileGenerator))]
public class InGameManager : ContextManager
{
	private Player player;

	private List<Light> lights = new List<Light>();
	public IEnumerable<Light> ActiveLights { get => lights.Where(l => l.enabled); }

	[HideInInspector]
	public TileGenerator TileGenerator;

	public override void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
		TileGenerator = GetComponent<TileGenerator>();
    }

	public override void HandleInput(InputPackage p) {
		player.HandleInput(p);
	}

	public void AddLight(Light light) {
		lights.Add(light);
	}

	public void RemoveLight(Light l) {
		lights.Remove(l);
	}

	public void GenerateTilesForPosition(TilePosition tp) {
		TileGenerator.GenerateForPosition(tp);
	}
}
