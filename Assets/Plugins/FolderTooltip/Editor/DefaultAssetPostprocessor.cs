using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Windows;

namespace FolderTooltip.Editor
{
    public class DefaultAssetPostprocessor : AssetPostprocessor
    {
        private static FolderTooltipAsset Asset => FolderTooltipAsset.Instance;
        
        private static void OnPostprocessAllAssets(
            string[] importedAssets,
            string[] deletedAssets,
            string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            RemoveFolderTooltips(deletedAssets);
            MoveFolderTooltips(movedAssets, movedFromAssetPaths);


            foreach (var importedAsset in importedAssets)
            {
                Debug.Log($"Imported {importedAsset}");
            }
            
            foreach (var deletedAsset in deletedAssets)
            {
                Debug.Log($"deleted {deletedAsset}");
            }
            
            foreach (var movedAsset in movedAssets)
            {
                Debug.Log($"Moved {movedAsset}");
            }
            
            foreach (var movedFromAssetPath in movedFromAssetPaths)
            {
                Debug.Log($"MovedFrom {movedFromAssetPath}");
            }
        }

        private static void RemoveFolderTooltips(string[] deletedAssets)
        {
            foreach (var deletedFolder in deletedAssets)
            {
                Asset.Remove(deletedFolder);
            }
        }
        
        private static void MoveFolderTooltips(string[] movedAssets, string[] movedFromAssetPaths)
        {
            for (int i = 0; i < movedAssets.Length; i++)
            {
                if (AssetDatabase.GetMainAssetTypeAtPath(movedAssets[i]) != typeof(DefaultAsset))
                {
                    continue;
                }

                Asset.Move(movedFromAssetPaths[i], movedAssets[i]);
            }
        }
    }
}