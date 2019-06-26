
using UnityEngine;


static public class Toolkit
{
	public static void DestroyContextual(Object item)
	{
		if (Application.isPlaying)
			Object.Destroy(item);
		else
			Object.DestroyImmediate(item);
	}
}
