
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace FolderTooltip.Editor
{
    [CustomEditor(typeof(DefaultAsset))]
    public class CustomDefaultAssetEditor : UnityEditor.Editor
    {
        private FolderTooltipAsset Asset => FolderTooltipAsset.Instance;

        private readonly StyleColor _borderColor = new StyleColor
        {
            value = Color.gray
        };
        
        public override VisualElement CreateInspectorGUI()
        {
            var path = AssetDatabase.GetAssetPath(target);
            
            VisualElement root = new VisualElement();

            if (!path.StartsWith("Assets"))
            {
                return root;
            }
            
            var label = new Label
            {
                text = Asset.GetTooltipText(path),
                enableRichText = true,
                style =
                {
                    marginTop = 2,
                    marginBottom = 2,
                    marginLeft = 2,
                    marginRight = 2,

                    borderBottomWidth = 3,
                    borderTopWidth = 3,
                    borderLeftWidth = 3,
                    borderRightWidth = 3,
                    
                    paddingBottom = 2,
                    paddingTop = 2,
                    paddingLeft = 2,
                    paddingRight = 2,

                    borderBottomColor = _borderColor,
                    borderTopColor = _borderColor,
                    borderLeftColor = _borderColor,
                    borderRightColor = _borderColor,
                }
            };

            var textField = new TextField
            {
                multiline = true,
                style =
                {
                    display = DisplayStyle.None
                }
            };
            textField.RegisterValueChangedCallback(evt => 
            { 
                label.text = evt.newValue;
            });

            const string editText = "編集";
            const string saveText = "保存";
            var button = new Button
            {
                text = editText,
            };
            
            button.clicked += () =>
            {
                if (button.text == saveText)
                {
                    Asset.Add(path, textField.value);

                    textField.style.display = DisplayStyle.None;
                    button.text = editText;
                }
                else
                {
                    textField.value = label.text;
                    
                    textField.style.display = DisplayStyle.Flex;
                    button.text = saveText;
                }
            };

            root.Add(label);
            root.Add(textField);
            root.Add(button);
            return root;
        }
    }
}