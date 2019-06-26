
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
#region Variables (singleton)

	static public GameManager Instance { get; private set; } = null;

	#endregion

#region Variables (serialized)

	[SerializeField]
	private CameraManager m_cameraManager = null;

	#endregion

	private Tile m_centerTile = null;
	private readonly MovementModule m_movementModule = new MovementModule();


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
		m_centerTile = GridManager.GetTile(5, 4);
	}

	private void Update()
	{
		m_movementModule.Update(m_centerTile, 3);
	}

	#region Getters

	public CameraManager GetCameraManager()
	{
		return m_cameraManager;
	}

	#endregion
}
