
using UnityEngine;
using UnityEditor;

//Original version of the ConditionalHideAttribute created by Brecht Lecluyse (www.brechtos.com)
//Modified by: -

[CustomPropertyDrawer(typeof(ConditionalHideAttribute))]
public class ConditionalHidePropertyDrawer : PropertyDrawer
{
	private SerializedProperty currentProperty = null;


	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		currentProperty = property;

		GUI.enabled = ShouldDisplayProperty();

		++EditorGUI.indentLevel;

		if (GUI.enabled)
			EditorGUI.PropertyField(position, property, label, true);

		--EditorGUI.indentLevel;
	}

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		currentProperty = property;

		if (ShouldDisplayProperty())
			return EditorGUI.GetPropertyHeight(property, label);

		return -EditorGUIUtility.standardVerticalSpacing;
	}

	private bool ShouldDisplayProperty()
	{
		SerializedProperty conditionProperty = FindConditionProperty();

		bool shouldDisplay = conditionProperty != null ? IsConditionMet(conditionProperty) : true;

		ConditionalHideAttribute conditionalHideAttr = (ConditionalHideAttribute)attribute;
		return conditionalHideAttr.Inverse ? !shouldDisplay : shouldDisplay;
	}

	private SerializedProperty FindConditionProperty()
	{
		ConditionalHideAttribute conditionalHideAttr = (ConditionalHideAttribute)attribute;
		SerializedProperty conditionProperty = currentProperty.serializedObject.FindProperty(conditionalHideAttr.ConditionalSourceField);

		if (conditionProperty == null)
			conditionProperty = FindConditionPropertyFromValuePath();

		return conditionProperty;
	}

	private SerializedProperty FindConditionPropertyFromValuePath()
	{
		ConditionalHideAttribute conditionalHideAttr = (ConditionalHideAttribute)attribute;
		string conditionPath = currentProperty.propertyPath.Replace(currentProperty.name, conditionalHideAttr.ConditionalSourceField);

		return currentProperty.serializedObject.FindProperty(conditionPath);
	}

	private bool IsConditionMet(SerializedProperty property)
	{
		switch (property.propertyType)
		{
			case SerializedPropertyType.Boolean:
				return property.boolValue;

			case SerializedPropertyType.ObjectReference:
				return property.objectReferenceValue != null;

			default:
				DebugTools.LogError("Data type of the property used for conditional hiding [{0}] is currently not supported", property.propertyType);
				return true;
		}
	}
}
