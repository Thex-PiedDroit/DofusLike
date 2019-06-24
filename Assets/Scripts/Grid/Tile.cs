
using UnityEngine;


public class Tile : MonoBehaviour
{
#region Variables (serialized)

	[SerializeField]
	private SpriteRenderer m_spriteRenderer = null;

	#endregion

#region Variables (private)

	private Vector2Int m_posInGrid = Vector2Int.zero;

	private bool occupied = false;

	#endregion


#region Getters

	public Vector2Int GetPosInGrid()
	{
		return m_posInGrid;
	}

	public bool IsOccupied()
	{
		return occupied;
	}

	#endregion

#region Setters

	public void SetPosInGrid(int x, int y)
	{
		m_posInGrid.x = x;
		m_posInGrid.y = y;
	}

	#endregion
}
