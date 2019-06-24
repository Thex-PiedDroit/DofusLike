using UnityEngine;
using UnityEditor;

public class MaterialReferencesCleaner : EditorWindow
{
	private Material m_selectedMaterial;
	private SerializedObject m_serializedObject;

	[MenuItem("Tools/MaterialReferencesCleaner")]
	private static void Init()
	{
		GetWindow<MaterialReferencesCleaner>("Ref. Cleaner");
	}

	protected virtual void OnEnable()
	{
		GetSelectedMaterial();
	}

	protected virtual void OnSelectionChange()
	{
		GetSelectedMaterial();
	}

	protected virtual void OnProjectChange()
	{
		GetSelectedMaterial();
	}

	protected virtual void OnGUI()
	{
		EditorGUIUtility.labelWidth = 200f;

		if (m_selectedMaterial == null)
		{
			EditorGUILayout.LabelField("No material selected");
		}
		else
		{
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Selected material:", m_selectedMaterial.name);
			EditorGUILayout.LabelField("Shader:", m_selectedMaterial.shader.name);

			m_serializedObject.Update();

			ProcessProperties("m_SavedProperties.m_TexEnvs");
			ProcessProperties("m_SavedProperties.m_Floats");
			ProcessProperties("m_SavedProperties.m_Colors");
		}

		EditorGUIUtility.labelWidth = 0;
	}

	private void ProcessProperties(string path)
	{
		var properties = m_serializedObject.FindProperty(path);
		if (properties != null && properties.isArray)
		{
			for (int i = properties.arraySize - 1; i >= 0; --i)
			{
				string propName = properties.GetArrayElementAtIndex(i).displayName;
				bool exist = m_selectedMaterial.HasProperty(propName);

				if (!exist)
				{
					properties.DeleteArrayElementAtIndex(i);
					m_serializedObject.ApplyModifiedProperties();
				}
			}
		}
	}

	private void GetSelectedMaterial()
	{
		m_selectedMaterial = Selection.activeObject as Material;
		if (m_selectedMaterial != null)
		{
			m_serializedObject = new SerializedObject(m_selectedMaterial);
		}

		Repaint();
	}
}
