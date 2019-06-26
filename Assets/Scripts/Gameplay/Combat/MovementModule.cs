
using System.Collections.Generic;
using UnityEngine;


public class MovementModule
{
#region Variables (private)

	private Tile previouslyPointedTile = null;
	private readonly Queue<Tile> currentPath = new Queue<Tile>();

	#endregion


	public void Update(Tile currentTile, int movementPoints)
	{
		if (PointedTileChanged(currentTile, movementPoints, out Tile pointedTile))
		{
			previouslyPointedTile = pointedTile;
			UpdatePath(currentTile, pointedTile);
		}
	}

	private bool PointedTileChanged(Tile currentTile, int movementPoints, out Tile pointedTile)
	{
		Camera activeCamera = GameManager.Instance.GetCameraManager().GetActiveCamera();

		Ray mouseRay = activeCamera.ScreenPointToRay(Input.mousePosition);

		if (Physics.Raycast(mouseRay, out RaycastHit hit, 1000.0f, LayerMask.GetMask("Ground"), QueryTriggerInteraction.Collide))
		{
			pointedTile = hit.collider.GetComponent<Tile>();

			if (GridUtils.GetDistanceBetweenTiles(currentTile, pointedTile) > movementPoints)
				pointedTile = null;
		}
		else
		{
			pointedTile = null;
		}

		return pointedTile != previouslyPointedTile;
	}

	private void UpdatePath(Tile currentTile, Tile destinationTile)
	{
		ClearPreviousPath();

		if (destinationTile != null)
		{
			Pathfinder.GetPathToDestination(destinationTile, currentTile, false, currentPath);
			ShowPath();
		}
	}

	private void ClearPreviousPath()
	{
		while (currentPath.Count > 0)
		{
			Tile tile = currentPath.Dequeue();
			tile.DisableOverlay();
		}
	}

	private void ShowPath()
	{
		List<Tile> path = new List<Tile>(currentPath);

		for (int i = 0, n = currentPath.Count; i < n ; ++i)
			path[i].ActivateOverlay(CombatEnum.Movement);
	}
}
