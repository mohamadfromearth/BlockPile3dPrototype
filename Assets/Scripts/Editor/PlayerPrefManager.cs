using UnityEditor;
using UnityEngine;
using Zenject;

namespace Editor
{
    public class PlayerPrefManager : EditorWindow
    {
        private string keyToDelete = "";


        [MenuItem("MenuItem/PlayerPrefManager")]
        private static void ShowWindow()
        {
            var window = GetWindow<PlayerPrefManager>();
            window.titleContent = new GUIContent("PlayerPrefManager");
            window.Show();
        }

        private void OnGUI()
        {
            keyToDelete = EditorGUILayout.TextField("Key to Delete", keyToDelete);
            
            if (GUILayout.Button("Delete PlayerPref"))
            {
                if (PlayerPrefs.HasKey(keyToDelete))
                {
                    PlayerPrefs.DeleteKey(keyToDelete);
                    PlayerPrefs.Save();
                    Debug.Log($"PlayerPref with key '{keyToDelete}' has been deleted.");
                }
                else
                {
                    Debug.LogWarning($"PlayerPref with key '{keyToDelete}' does not exist.");
                }
            }
        }
    }
}