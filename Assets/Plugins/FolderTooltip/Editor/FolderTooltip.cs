using UnityEditor;
using UnityEngine;

namespace FolderTooltip.Editor
{
    [InitializeOnLoad]
    internal static class FolderTooltip
    {
        private static string _guidHovered;
        
        static FolderTooltip()
        {
            EditorApplication.update += WaitForAssetDatabase;
        }

        private static void WaitForAssetDatabase()
        {
            if (EditorApplication.isUpdating)
            {
                return;
            }
            
            EditorApplication.update -= WaitForAssetDatabase;
            Initialize();
        }

        private static void Initialize()
        {
            FolderTooltipAsset.Instance.MakeDataConsistent();
            EditorApplication.projectWindowItemOnGUI += OnGUI;
        }

        private static void OnGUI(string guid, Rect selectionRect)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            if (path == "")
            {
                return;
            }

            if (FolderTooltipAsset.Instance.Contains(path))
            {
                GUI.Box(selectionRect, new GUIContent("", FolderTooltipAsset.Instance.GetTooltipText(path)), GUIStyle.none);
                GUI.Label(new Rect(Event.current.mousePosition, Vector2.one), GUI.tooltip);
            }
        }
    }
}