using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor (typeof (ADRCone))]
public class ADRConeEditor : Editor {
	
	SerializedProperty _baseSides;
	
	[MenuItem ("GameObject/Create Other/Cone")]
	static void Create(){
		ADRCone.Create ();
	}

	void OnEnable() {
		(target as ADRCone).hideFlags = HideFlags.HideInInspector;
	}
}