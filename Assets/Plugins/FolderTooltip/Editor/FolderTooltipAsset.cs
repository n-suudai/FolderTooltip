using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace FolderTooltip.Editor
{
    [Serializable]
    internal class FolderTooltipAsset : ScriptableObject
    {
        [SerializeField]
        private List<TooltipData> Tooltips = new ();
        
        [Serializable]
        internal class TooltipData
        {
            public string Path;
            public string Text;
        }

        internal bool Contains(string path)
        {
            return Tooltips.Exists(x => x.Path == path);
        }

        internal bool TryGetValue(string path, out TooltipData tooltip)
        {
            if (Contains(path))
            {
                tooltip = Tooltips.First(x => x.Path == path);
                return true;
            }

            tooltip = null;
            return false;
        }
        
        public void Remove(string folder)
        {
            if (TryGetValue(folder, out var tooltip))
            {
                Tooltips.Remove(tooltip);
                
                EditorUtility.SetDirty(this);
                AssetDatabase.SaveAssetIfDirty(this);
            }
        }

        public void Add(string folder, string tooltipText)
        {
            if (TryGetValue(folder, out var tooltip))
            {
                tooltip.Text = tooltipText;
            }
            else
            {
                tooltip = new TooltipData
                {
                    Path = folder,
                    Text = tooltipText
                };
                Tooltips.Add(tooltip);
            }
            
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssetIfDirty(this);
        }

        public string GetTooltipText(string folder)
        {
            if (TryGetValue(folder, out var tooltip))
            {
                return tooltip.Text;
            }
            return "";
        }

        public void Move(string from, string to)
        {
            string tooltip = GetTooltipText(from);
            Remove(from);
            Add(to, tooltip);
        }

        /// <summary>
        /// データの一貫性を保つために呼び出します。
        /// 見つからないフォルダの余分なデータを削除します。
        /// </summary>
        public void MakeDataConsistent()
        {
            foreach (var tooltip in Tooltips)
            {
                if (!Directory.Exists(tooltip.Path))
                {
                    Remove(tooltip.Path);
                }
            }
        }
        
        #region Static Assets
        
        private const string FolderTooltipAssetPath = "Assets/Editor Default Resources/FolderTooltip";
        private const string FolderTooltipAssetFileName = "FolderTooltipAsset.asset";
        private static readonly string FolderTooltipAssetFilePath = $"{FolderTooltipAssetPath}/{FolderTooltipAssetFileName}";

        private static FolderTooltipAsset _instance;
        public static FolderTooltipAsset Instance
        {
            get
            {
                if (_instance != null)
                {
                    return _instance;
                }

                _instance = EditorGUIUtility.Load(FolderTooltipAssetFilePath) as FolderTooltipAsset;
                if (_instance != null)
                {
                    return _instance;
                }
                
                _instance = CreateInstance<FolderTooltipAsset>();
                if (!Directory.Exists(FolderTooltipAssetPath))
                {
                    Directory.CreateDirectory(FolderTooltipAssetPath);
                }
                
                AssetDatabase.CreateAsset(_instance, FolderTooltipAssetFilePath);
                return _instance;
            }
        }

        #endregion
    }
}
