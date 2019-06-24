
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class GridGenerator
{
#region Variables (serialized)

	[SerializeField]
	private Tile m_tilePrefab = null;
	[SerializeField]
	private Transform m_gridContainer = null;
	[SerializeField]
	private Transform m_ground = null;

	#endregion

#region Variables (private)

	static private readonly string TILE_NAME_FORMAT = typeof(Tile).ToString() + "_{0:00}_{1:00}";
	private const float TILE_HALF_SIZE = 0.5f;

	#endregion


	public Tile[,] GenerateGrid(Tile[,] oldTiles = null)
	{
		Queue<Tile> tilesPool = new Queue<Tile>(oldTiles?.Length ?? 0);
		if (oldTiles != null)
			tilesPool.EnqueueRange(oldTiles);

		Vector2Int gridSize = new Vector2Int
		{
			x = (int)m_ground.localScale.x,
			y = (int)m_ground.localScale.z,
		};

		Tile[,] grid = new Tile[gridSize.x, gridSize.y];

		for (int x = 0; x < gridSize.x; ++x)
		{
			for (int y = 0; y < gridSize.y; ++y)
			{
				Tile tile = GetTile(tilesPool);
				InitializeTile(tile, x, y, gridSize);
				grid[x, y] = tile;
			}
		}

		ClearTilesPool(tilesPool);

		return grid;
	}

	private Tile GetTile(Queue<Tile> pool)
	{
		while (pool.Count > 0)
		{
			Tile tile = pool.Dequeue();

			if (tile != null)
				return tile;
		}

		return Object.Instantiate(m_tilePrefab, m_gridContainer);
	}

	private void InitializeTile(Tile tile, int x, int y, Vector2 gridSize)
	{
		tile.name = string.Format(TILE_NAME_FORMAT, x, y);
		tile.SetPosInGrid(x, y);
		tile.transform.localPosition = CalculateTilePosition(x, y, gridSize);
	}

	private Vector3 CalculateTilePosition(int x, int y, Vector2 gridSize)
	{
		Vector2 gridHalfSize = gridSize * 0.5f;

		return new Vector3(x + TILE_HALF_SIZE, 0.0f, y + TILE_HALF_SIZE) - gridHalfSize.x_y();
	}

	private void ClearTilesPool(Queue<Tile> pool)
	{
		while (pool.Count > 0)
			Toolkit.DestroyContextual(pool.Dequeue().gameObject);
	}
}
