
using System;
using UnityEngine;


[Serializable]
public class CameraManager
{
#region Variables (serialized)

	[SerializeField]
	private Camera m_activeCamera = null;

	#endregion

	#region Variables (private)



	#endregion


	public Camera GetActiveCamera()
	{
		return m_activeCamera;
	}
}
