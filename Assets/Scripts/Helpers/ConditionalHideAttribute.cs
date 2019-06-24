
using UnityEngine;
using System;

//Original version of the ConditionalHideAttribute created by Brecht Lecluyse (www.brechtos.com)
//Modified by: -

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Struct, Inherited = true)]
public class ConditionalHideAttribute : PropertyAttribute
{
	public string ConditionalSourceField { get; private set; } = "";
	public bool Inverse { get; private set; } = false;


	public ConditionalHideAttribute(string conditionalSourceField)
	{
		ConditionalSourceField = conditionalSourceField;
		Inverse = false;
	}

	public ConditionalHideAttribute(string conditionalSourceField, bool inverse)
	{
		ConditionalSourceField = conditionalSourceField;
		Inverse = inverse;
	}
}
