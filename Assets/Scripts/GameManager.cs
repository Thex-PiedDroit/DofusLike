
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
#region Variables (singleton)

	static public GameManager Instance { get; private set; } = null;

	#endregion


	private void Awake()
	{
		if (Instance != null)
		{
			DebugTools.LogError("Two instances of GameManager found in current scene. Second one is getting disabled.");
			return;
		}

		Instance = this;
	}

	private void Start()
	{
		Tile origin = GridManager.GetTile(6, 4);
		Tile destination = GridManager.GetTile(14, 10);

		Queue<Tile> path = Pathfinder.GetPathToDestination(destination, origin, false);
		bool first = true;

		while (path.Count > 0)
		{
			Tile tile = path.Dequeue();

			Color lineColor = first ? Color.blue : path.Count == 0 ? Color.green : Color.white;
			Debug.DrawLine(tile.transform.position, tile.transform.position + Vector3.up, lineColor);

			first = false;
		}

		Debug.Break();
	}
}
