using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor (typeof (BaseDialogue))]
public class BaseDialogueEditor : Editor {

	public override void OnInspectorGUI(){
		base.OnInspectorGUI();
		
		if (GUILayout.Button("Update Database")){
			GameObject.FindWithTag("Dialogue Database").GetComponent<DialogueDatabase>().LoadDialogueDatabase(); //For making sure the database is updated (load database before updating)

			Selection.activeGameObject.GetComponent<BaseDialogue>().UpdateDialogueInDatabase();
		}
		
		if (GUILayout.Button("Load from Database")){
			Selection.activeGameObject.GetComponent<BaseDialogue>().LoadDialogueFromDatabase();
		}
	}
}
