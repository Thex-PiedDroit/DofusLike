
using UnityEngine;


public class GridManager : MonoBehaviour
{
#region Variables (serialized)

	[SerializeField]
	private GridGenerator m_gridGenerator = null;

	#endregion

#region Variables (private)

	static private Tile[,] m_grid = null;

	#endregion


	private void Awake()
	{
		GenerateGrid();
	}

	private void GenerateGrid()
	{
		m_grid = m_gridGenerator.GenerateGrid(m_grid);
	}

#region Getters

	static public Tile[,] GetGrid()
	{
		return m_grid;
	}

	static public Tile GetTile(int x, int y)
	{
		return m_grid[x, y];
	}

	#endregion
}
