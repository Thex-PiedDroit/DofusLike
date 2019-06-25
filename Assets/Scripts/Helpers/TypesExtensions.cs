
using UnityEngine;
using System;
using System.Collections.Generic;

static public class Vec2Extensions
{
#pragma warning disable IDE1006 // Naming Styles
	static public Vector3 xyz(this Vector2 vec)
	{
		return vec;	// Yep...
	}

	static public Vector3 x_y(this Vector2 vec)
	{
		return new Vector3(vec.x, 0.0f, vec.y);
	}

	static public Vector2 x_(this Vector2 vec)
	{
		return new Vector2(vec.x, 0.0f);
	}

	static public Vector2 _y(this Vector2 vec)
	{
		return new Vector2(0.0f, vec.y);
	}

	static public Vector2 ShiftX(this Vector2 vec, float offsetX)
	{
		vec.x += offsetX;
		return vec;
	}

	static public Vector2 ShiftY(this Vector2 vec, float offsetY)
	{
		vec.x += offsetY;
		return vec;
	}

	static public Vector2 ShiftXY(this Vector2 vec, Vector2 offset)
	{
		vec += offset;
		return vec;
	}

	static public Vector2 SetX(this Vector2 vec, float x)
	{
		vec.x = x;
		return vec;
	}

	static public Vector2 SetY(this Vector2 vec, float y)
	{
		vec.y = y;
		return vec;
	}

	static public Vector2 Clamp(this Vector2 vec, Vector2 min, Vector2 max)
	{
		vec.x = Mathf.Clamp(vec.x, min.x, max.x);
		vec.y = Mathf.Clamp(vec.y, min.y, max.y);

		return vec;
	}
}

static public class Vec3Extensions
{
	static public Vector2 xy(this Vector3 vec)
	{
		return vec;
	}

	static public Vector3 xzy(this Vector3 vec)
	{
		return new Vector3(vec.x, vec.z, vec.y);
	}

	static public Vector3 xyz(this Vector3 vec)
	{
		return new Vector3(vec.x, vec.y, vec.z);
	}

	static public Vector3 x_z(this Vector3 vec)
	{
		return new Vector3(vec.x, 0.0f, vec.z);
	}

	static public Vector3 xy_(this Vector3 vec)
	{
		return new Vector3(vec.x, vec.y);
	}

	static public Vector3 _yz(this Vector3 vec)
	{
		return new Vector3(0.0f, vec.y, vec.z);
	}
#pragma warning restore IDE1006 // Naming Styles

	static public Vector3 ShiftX(this Vector3 vec, float offsetX)
	{
		vec.x += offsetX;
		return vec;
	}

	static public Vector3 ShiftY(this Vector3 vec, float offsetY)
	{
		vec.x += offsetY;
		return vec;
	}

	static public Vector3 ShiftZ(this Vector3 vec, float offsetZ)
	{
		vec.x += offsetZ;
		return vec;
	}

	static public Vector3 ShiftXYZ(this Vector3 vec, Vector3 offset)
	{
		vec += offset;
		return vec;
	}

	static public Vector3 SetX(this Vector3 vec, float x)
	{
		vec.x = x;
		return vec;
	}

	static public Vector3 SetY(this Vector3 vec, float y)
	{
		vec.y = y;
		return vec;
	}

	static public Vector3 SetZ(this Vector3 vec, float z)
	{
		vec.z = z;
		return vec;
	}

	static public Vector3 Clamp(this Vector3 vec, Vector3 min, Vector3 max)
	{
		vec.x = Mathf.Clamp(vec.x, min.x, max.x);
		vec.y = Mathf.Clamp(vec.y, min.y, max.y);
		vec.z = Mathf.Clamp(vec.z, min.z, max.z);

		return vec;
	}
}

static public class ArrayExtensions
{
	static public T Last<T>(this T[] array)
	{
		return array[array.Length - 1];
	}

	static public bool Contains<T>(this T[] array, T element)
	{
		for (int i = 0, n = array.Length; i < n; ++i)
		{
			if (array[i].Equals(element))
				return true;
		}

		return false;
	}
}

static public class ListExtensions
{
	static public List<T> Clone<T>(this List<T> list)
	{
		List<T> newList = new List<T>(list.Count);

		for (int i = 0; i < list.Count; ++i)
		{
			newList.Add(list[i]);
		}

		return newList;
	}

	/// <summary>
	/// Returns -1 if not found.
	/// </summary>
	static public int Find<T>(this List<T> list, T element)
	{
		for (int i = 0, n = list.Count; i < n; ++i)
		{
			if (list[i].Equals(element))
				return i;
		}

		return -1;
	}

	static public T GetRandomElement<T>(this List<T> list)
	{
		return list[UnityEngine.Random.Range(0, list.Count)];
	}

	static public T First<T>(this List<T> list)
	{
		return list[0];
	}

	static public T Last<T>(this List<T> list)
	{
		return list[list.Count - 1];
	}

	static public void RemoveSwapLast<T>(this List<T> list, int index)
	{
		int lastItemIndex = list.Count - 1;

		list[index] = list[lastItemIndex];
		list.RemoveAt(lastItemIndex);
	}
}

static public class QueueExtensions
{
	static public Queue<T> EnqueueRange<T>(this Queue<T> queue, T[,] range)
	{
		foreach (T item in range)
			queue.Enqueue(item);

		return queue;
	}
}

static public class DictionaryExtensions
{
	static public Dictionary<T, U> Clone<T, U>(this Dictionary<T, U> dictionary)
	{
		Dictionary<T, U> newDictionary = new Dictionary<T, U>(dictionary.Count);

		foreach (KeyValuePair<T, U> pair in dictionary)
		{
			newDictionary.Add(pair.Key, pair.Value);
		}

		return newDictionary;
	}
}

static public class VariableExtensions
{
	static public float Sqrd(this float value)
	{
		return value * value;
	}
}

static public class StringExtensions
{
	static public T ToEnum<T>(this string str, T defaultValue) where T : Enum
	{
		if (string.IsNullOrEmpty(str))
			return defaultValue;

		Enum[] allValues = (Enum[])(Enum.GetValues(typeof(Enum)));

		for (int i = 0, n = allValues.Length; i < n; ++i)
		{
			Enum enumValue = allValues[i];

			if (FormatEnumToString(enumValue).Equals(GetLowerCase(str)))
				return (T)enumValue;
		}

		DebugTools.LogWarning("Could not find matching enum for \"{0}\" in enum \"{1}\"!", str, typeof(T).ToString());
		return defaultValue;
	}

	static private string FormatEnumToString(Enum enumName)
	{
		return enumName.ToString().ToLower().Replace("_", "");
	}

	static private string GetLowerCase(string str)
	{
		return str.Trim().ToLower();
	}

	static public string Replace(this string str, int index, char character)
	{
		return str.Remove(index, 1).Insert(index, character.ToString());
	}
}

static public class ColorExtensions
{
	static public Color SetAlpha(this Color color, float alpha)
	{
		color.a = alpha;
		return color;
	}
}
