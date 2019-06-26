
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GridManager))]
public class GridManagerEditor : Editor
{
#region Variables (private)

	static private readonly MethodInfo GENERATE_GRID_METHOD = typeof(GridManager).GetMethod("GenerateGrid", BindingFlags.NonPublic | BindingFlags.Instance);
	static private readonly FieldInfo GRID_FIELD = typeof(GridManager).GetField("m_grid", BindingFlags.NonPublic | BindingFlags.Static);


	static private readonly FieldInfo GRID_GENERATOR_FIELD = typeof(GridManager).GetField("m_gridGenerator", BindingFlags.NonPublic | BindingFlags.Instance);
	static private readonly FieldInfo GRID_CONTAINER_FIELD = typeof(GridGenerator).GetField("m_gridContainer", BindingFlags.NonPublic | BindingFlags.Instance);

	#endregion


	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		if (GUILayout.Button("Generate Grid"))
			GENERATE_GRID_METHOD.Invoke(target, null);
		if (GUILayout.Button("Clear Grid"))
			ClearGrid();
	}

	private void ClearGrid()
	{
		if (GRID_FIELD.GetValue(target) is Tile[,] grid)
		{
			foreach (Tile tile in grid)
			{
				if (tile.gameObject != null)
					Toolkit.DestroyContextual(tile.gameObject);
				else
					Toolkit.DestroyContextual(tile);
			}

			GRID_FIELD.SetValue(target, null);
		}

		TryClearUnlinkedGrid();
	}

	private void TryClearUnlinkedGrid()
	{
		GridGenerator gridGenerator = GRID_GENERATOR_FIELD.GetValue(target) as GridGenerator;
		Transform gridContainer = GRID_CONTAINER_FIELD.GetValue(gridGenerator) as Transform;

		Tile[] tiles = gridContainer.GetComponentsInChildren<Tile>();

		for (int i = tiles.Length - 1; i >= 0; --i)
			Toolkit.DestroyContextual(tiles[i].gameObject);
	}
}
