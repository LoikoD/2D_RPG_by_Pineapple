using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(DialogueWithTrigger))]

public class DialogueWithTriggerEditor : Editor {
	
	enum displayFieldType {DisplayAsAutomaticFields, DisplayAsCustomizableGUIFields}
	displayFieldType DisplayFieldType;
	
	DialogueWithTrigger t;
	SerializedObject GetTarget;
	SerializedProperty ThisList;
	int ListSize;


	private ReorderableList list;

	
	void OnEnable(){
		t = (DialogueWithTrigger)target;
		GetTarget = new SerializedObject(t);
		ThisList = GetTarget.FindProperty("message"); // Find the List in our script and create a refrence of it

	}
	
	public override void OnInspectorGUI(){
		base.OnInspectorGUI();
		
		GetTarget.Update();
		
		//Display our list to the inspector window
		for(int i = 0; i < ThisList.arraySize; i++){
			SerializedProperty MyListRef = ThisList.GetArrayElementAtIndex(i);
			SerializedProperty messageString = MyListRef.FindPropertyRelative("messageText");
			SerializedProperty choicesString = MyListRef.FindPropertyRelative("choices");
			SerializedProperty choicesInt = MyListRef.FindPropertyRelative("choiceDestiny");
			SerializedProperty triggerBool = MyListRef.FindPropertyRelative("hasTrigger");
			SerializedProperty triggerIndex = MyListRef.FindPropertyRelative("triggerID");



			GUILayout.Label("Dialogue index = " + i.ToString());
			EditorGUILayout.PropertyField(messageString);
				
			EditorGUILayout.Space ();
			EditorGUILayout.Space ();
			EditorGUILayout.LabelField("Player Dialog Choices");
				
			if(GUILayout.Button("Add New Choice",GUILayout.MaxWidth(130),GUILayout.MaxHeight(20))){
				choicesString.InsertArrayElementAtIndex(choicesString.arraySize);
				choicesString.GetArrayElementAtIndex(choicesString.arraySize -1).stringValue = "";

				choicesInt.InsertArrayElementAtIndex(choicesInt.arraySize);
				choicesInt.GetArrayElementAtIndex(choicesInt.arraySize -1).intValue = 0;
			}
				
			for(int a = 0; a < choicesString.arraySize; a++){
				EditorGUILayout.PropertyField(choicesString.GetArrayElementAtIndex(a));
				EditorGUILayout.PropertyField( choicesInt.GetArrayElementAtIndex(a));
				if(GUILayout.Button("Remove  (" + a.ToString() + ")",GUILayout.MaxWidth(100),GUILayout.MaxHeight(15))){
					choicesString.DeleteArrayElementAtIndex(a);
					choicesInt.DeleteArrayElementAtIndex(a);
				}
			}

			EditorGUILayout.PropertyField(triggerBool);
			EditorGUILayout.PropertyField(triggerIndex);
			
			EditorGUILayout.Space ();

			if(GUILayout.Button("Remove Dialogue")){
				ThisList.DeleteArrayElementAtIndex(i);
			}
			EditorGUILayout.Space ();
			EditorGUILayout.Space ();
			EditorGUILayout.Space ();
			EditorGUILayout.Space ();
		}

		if(GUILayout.Button("Add New Dialogue")){
			t.message.Add(new TextDialogue());
		}
		
		EditorGUILayout.Space ();
		EditorGUILayout.Space ();

		GetTarget.ApplyModifiedProperties();

		if (GUILayout.Button("Update Database")){
			GameObject.FindWithTag("Dialogue Database").GetComponent<DialogueDatabase>().LoadDialogueDatabase(); //For making sure the database is updated (load database before updating)
			
			Selection.activeGameObject.GetComponent<BaseDialogue>().UpdateDialogueInDatabase();
		}
		
		if (GUILayout.Button("Load from Database")){
			Selection.activeGameObject.GetComponent<BaseDialogue>().LoadDialogueFromDatabase();
		}
	}
}