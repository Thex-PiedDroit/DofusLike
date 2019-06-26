
using UnityEngine;


static public class GridUtils
{
	static public int GetDistanceBetweenTiles(Tile origin, Tile destination)
	{
		Vector2Int distanceVector = destination.GetPosInGrid() - origin.GetPosInGrid();
		distanceVector.x = Mathf.Abs(distanceVector.x);
		distanceVector.y = Mathf.Abs(distanceVector.y);

		return Mathf.Abs(distanceVector.x + distanceVector.y);
	}
}
