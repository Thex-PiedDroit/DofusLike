
using UnityEngine;


static public class Toolkit
{
	static public void DestroyContextual(GameObject gameObject)
	{
		if (Application.isPlaying)
			Object.Destroy(gameObject);
		else
			Object.DestroyImmediate(gameObject);
	}
}
