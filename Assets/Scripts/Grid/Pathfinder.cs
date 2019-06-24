
using UnityEngine;
using System.Collections.Generic;


static public class Pathfinder
{
#region Variables (private)

	private class Node
	{
		public Node m_parentNode = null;
		public Tile m_tile = null;

		public float m_distanceFromStart = 0.0f;
		public int m_distanceFromDestination = 0;


		public Node(Tile tile)
		{
			m_tile = tile;
		}

		public void SetValues(Node parent, float distanceFromstart, int distanceFromDestination)
		{
			m_parentNode = parent;
			m_distanceFromStart = distanceFromstart;
			m_distanceFromDestination = distanceFromDestination;
		}

		public float GetCost()
		{
			return m_distanceFromStart + m_distanceFromDestination;
		}
	}

	#endregion


	static public Queue<Tile> GetPathToDestination(Tile destination, Tile origin, bool canUseDiagonals, Queue<Tile> pathQueue = null)
	{
		if (pathQueue == null)
			pathQueue = new Queue<Tile>();
		else
			pathQueue.Clear();

		int minDistance = GetDistanceBetweenNodes(origin, destination);

		List<Node> tilesToCheck = new List<Node>(minDistance);
		List<Node> checkedTiles = new List<Node>(minDistance);

		tilesToCheck.Add(new Node(origin));

		bool foundPath = false;

		do
		{
			int cheapestOpenNodeIndex = GetCheapestNodeIndexInList(tilesToCheck);
			Node currentNode = tilesToCheck[cheapestOpenNodeIndex];

			tilesToCheck.RemoveAt(cheapestOpenNodeIndex);
			checkedTiles.Add(currentNode);

			if (currentNode.m_tile == destination)
			{
				foundPath = true;
				break;
			}

			PopulateOpenListWithNeighbours(currentNode, destination, ref tilesToCheck, checkedTiles, canUseDiagonals);

		} while (tilesToCheck.Count > 0);

		if (foundPath)
			pathQueue = RetracePathFromEnd(checkedTiles, origin, pathQueue);
		else
			DebugTools.LogError("Couldn't find a path from {0} to {1}", origin, destination);

		return pathQueue;
	}

	static private void PopulateOpenListWithNeighbours(Node centerNode, Tile destination, ref List<Node> tilesToCheck, List<Node> checkedTiles, bool canUseDiagonals)
	{
		Tile[,] grid = GridManager.GetGrid();

		Vector2Int centerPos = centerNode.m_tile.GetPosInGrid();

		for (int x = -1; x <= 1; ++x)
		{
			for (int y = -1; y <= 1; ++y)
			{
				int neighbourPosX = centerPos.x + x;
				int neighbourPosY = centerPos.y + y;

				if (IsCenter(x, y) || (!canUseDiagonals && IsDiagonal(x, y)) || IsOutOfMap(neighbourPosX, neighbourPosY))
					continue;

				Tile currentNeighbour = grid[neighbourPosX, neighbourPosY];

				if (!currentNeighbour.IsOccupied() && !ListContainsTile(checkedTiles, currentNeighbour))
					AddNeighbourToList(currentNeighbour, tilesToCheck, centerNode, destination);
			}
		}
	}

	static private bool IsCenter(int x, int y)
	{
		return x == 0 && y == 0;
	}

	static private bool IsDiagonal(int x, int y)
	{
		return x != 0 && y != 0;
	}

	static private bool IsOutOfMap(int x, int y)
	{
		Tile[,] grid = GridManager.GetGrid();
		return x < 0 || y < 0 || x >= grid.GetLength(0) || y >= grid.GetLength(1);
	}

	static private void AddNeighbourToList(Tile neighbour, List<Node> list, Node centerNode, Tile destination)
	{
		float distanceFromStartToHere = centerNode.m_distanceFromStart + 1;
		int distanceFromDestination = GetDistanceBetweenNodes(neighbour, destination);

		int indexInOpenList = FindNodeInList(neighbour, list);

		Node node;
		bool shouldRefreshNodeValues;

		if (indexInOpenList == -1)
		{
			node = new Node(neighbour);
			list.Add(node);

			shouldRefreshNodeValues = true;
		}
		else
		{
			node = list[indexInOpenList];
			shouldRefreshNodeValues = distanceFromStartToHere + distanceFromDestination < node.GetCost();
		}

		if (shouldRefreshNodeValues)
			node.SetValues(centerNode, distanceFromStartToHere, distanceFromDestination);
	}

	static private int GetCheapestNodeIndexInList(List<Node> nodes)
	{
		int cheapestNodeIndex = 0;

		for (int i = 1, n = nodes.Count; i < n; ++i)
		{
			if (IsNodeCheaper(nodes[i], nodes[cheapestNodeIndex]))
				cheapestNodeIndex = i;
		}

		return cheapestNodeIndex;
	}

	static private bool IsNodeCheaper(Node currentNode, Node cheapestNode)
	{
		float currentNodeCost = currentNode.GetCost();
		float cheapestCost = cheapestNode.GetCost();

		return currentNodeCost < cheapestCost || (currentNodeCost == cheapestCost && currentNode.m_distanceFromDestination < cheapestNode.m_distanceFromDestination);
	}

	static private bool ListContainsTile(List<Node> list, Tile tile)
	{
		return FindNodeInList(tile, list) != -1;
	}

	static private int FindNodeInList(Tile tile, List<Node> list)
	{
		for (int i = 0, n = list.Count; i < n; ++i)
		{
			if (list[i].m_tile == tile)
				return i;
		}

		return -1;
	}

	static private int GetDistanceBetweenNodes(Tile origin, Tile destination)
	{
		Vector2Int distanceVector = destination.GetPosInGrid() - origin.GetPosInGrid();
		distanceVector.x = Mathf.Abs(distanceVector.x);
		distanceVector.y = Mathf.Abs(distanceVector.y);

		return Mathf.Abs(distanceVector.x + distanceVector.y);
	}

	static private Queue<Tile> RetracePathFromEnd(List<Node> closedList, Tile start, Queue<Tile> pathQueue)
	{
		List<Tile> pathReversed = new List<Tile>(closedList.Count);

		Node currentNode = closedList.Last();
		pathReversed.Add(currentNode.m_tile);

		while (currentNode.m_tile != start)
		{
			currentNode = currentNode.m_parentNode;
			pathReversed.Add(currentNode.m_tile);
		}

		for (int i = pathReversed.Count - 1; i >= 0; --i)
			pathQueue.Enqueue(pathReversed[i]);

		return pathQueue;
	}
}
