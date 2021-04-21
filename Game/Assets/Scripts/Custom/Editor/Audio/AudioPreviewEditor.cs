using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AbstractSoundScriptableObject), true)]
public class AudioPreviewEditor : Editor
{
	[SerializeField] private AudioSource previewer;

	public void OnEnable()
	{
		previewer = EditorUtility.CreateGameObjectWithHideFlags(
			"Audio preview", HideFlags.HideAndDontSave, typeof(AudioSource)).GetComponent<AudioSource>();
	}

	public void OnDisable()
	{
		DestroyImmediate(previewer.gameObject);
	}

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		EditorGUI.BeginDisabledGroup(serializedObject.isEditingMultipleObjects);
		if (GUILayout.Button("Preview"))
		{
			((AbstractSoundScriptableObject)target).PlaySound(previewer);
		}
		EditorGUI.EndDisabledGroup();
	}
}