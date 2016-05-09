using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor (typeof (ADRPyramid))] 
[CanEditMultipleObjects]
public class ADRPyramidEditor : Editor {

	SerializedProperty _baseSides;

	[MenuItem ("GameObject/Create Other/Pyramid")]
	static void Create(){
		ADRPyramid.Create ();
	}

	void OnEnable() {
		_baseSides = serializedObject.FindProperty ("baseSides");
	}

	public override void OnInspectorGUI ()
	{
		serializedObject.Update();
		int baseSides = _baseSides.intValue;
		EditorGUILayout.PropertyField (_baseSides, true);
		if (_baseSides.intValue > 2) {
			if (_baseSides.intValue != baseSides) {
				serializedObject.ApplyModifiedProperties ();
				(target as ADRPyramid).Rebuild();
			}
		}
	}
}