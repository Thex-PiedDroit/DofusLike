
using System.Collections.Generic;
using UnityEngine;


public class Tile : MonoBehaviour
{
#region Variables (serialized)

	[SerializeField]
	private SpriteRenderer m_overlaySpriteRenderer = null;

	#endregion

#region Variables (private)

	static private readonly Dictionary<CombatEnum, Color> OVERLAY_COLORS = new Dictionary<CombatEnum, Color>
	{
		[CombatEnum.Movement] = ColorReferences.MOVEMENT_GREEN,
		[CombatEnum.Targetting] = ColorReferences.TARGETTING_BLUE,
	};
	private const float OVERLAY_ALPHA = 0.78f;


	private Vector2Int m_posInGrid = Vector2Int.zero;

	private bool occupied = false;

	#endregion


	public void ActivateOverlay(CombatEnum purpose)
	{
		UpdateOverlayColor(purpose);
		m_overlaySpriteRenderer.gameObject.SetActive(true);
	}

	public void DisableOverlay()
	{
		m_overlaySpriteRenderer.gameObject.SetActive(false);
	}

	private void UpdateOverlayColor(CombatEnum purpose)
	{
		Color overlayColor = Color.white;
		if (OVERLAY_COLORS.ContainsKey(purpose))
			overlayColor = OVERLAY_COLORS[purpose];

		m_overlaySpriteRenderer.color = overlayColor.SetAlpha(OVERLAY_ALPHA);
	}

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
