using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor (typeof (DialogueDatabase))]
public class DialogueDatabaseEditor : Editor {

	public override void OnInspectorGUI(){
		base.OnInspectorGUI();

		if (GUILayout.Button("Save to File")){
			Selection.activeGameObject.GetComponent<DialogueDatabase>().SaveDialogueDatabase();                   //Selection.activeGameObject is active instance of DialogueDatabase
		}

		if (GUILayout.Button("Load from File")){
			Selection.activeGameObject.GetComponent<DialogueDatabase>().LoadDialogueDatabase();
		}

	}

}
