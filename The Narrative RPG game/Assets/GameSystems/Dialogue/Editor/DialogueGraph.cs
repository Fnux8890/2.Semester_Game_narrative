using System.Linq;
using GameSystems.Dialogue.Runtime;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameSystems.Dialogue.Editor
{
    public class DialogueGraph : EditorWindow
    {
        private DialogueGraphView _graphView;
        private string _filename = "New Narrative";
        
        [MenuItem("Graph/Dialogue Graph")]
        public static void OpenDialogueGraphWindow()
        {
            var window = GetWindow<DialogueGraph>();
            window.titleContent = new GUIContent("Dialogue Graph");
        }

        

        private void OnEnable()
        {
            ConstructGraphView();
            GenerateToolbar();
            GenerateMiniMap();
            GenerateBlackboard();
        }

        private void GenerateBlackboard()
        {
            var blackboard = new Blackboard(_graphView);
            blackboard.Add(new BlackboardSection{title = "Exposed properties"});
            blackboard.addItemRequested = _blackboard => { _graphView.AddPropertyToBlackboard(new ExposedProperty());};
            blackboard.editTextRequested = (blackboard1, element, newValue) =>
            {
                var oldPropertyName = ((BlackboardField) element).text;
                if (_graphView.ExposedProperties.Any(x => x.PropertyName == newValue))
                {
                    EditorUtility.DisplayDialog("ERROR", "This property name already exists, please chose another one!",
                        "OK");
                    return;
                }

                var propertyIndex = _graphView.ExposedProperties.FindIndex(x => x.PropertyName == oldPropertyName);
                _graphView.ExposedProperties[propertyIndex].PropertyName = newValue;
                ((BlackboardField) element).text = newValue;

            };
            blackboard.SetPosition(new Rect(10,30,300,200));
            _graphView.Blackboard = blackboard;
            _graphView.Add(blackboard);

        }

        private void GenerateMiniMap()
        {
            var miniMap = new MiniMap {anchored = true};
            var cords = _graphView.contentViewContainer.WorldToLocal(new Vector2(this.maxSize.x - 10, 30));
            miniMap.SetPosition(new Rect(cords.x, cords.y, 200, 140));
            _graphView.Add(miniMap);
        }

        private void ConstructGraphView()
        {
            _graphView = new DialogueGraphView(this)
            {
                name = "Dialogue Graph"
            };
            
            _graphView.StretchToParentSize();
            rootVisualElement.Add(_graphView);
        }

        private void GenerateToolbar()
        {
            var toolbar = new Toolbar();

            var fileNameTextField = new TextField("File Name:");
            fileNameTextField.SetValueWithoutNotify(_filename);
            fileNameTextField.MarkDirtyRepaint();
            fileNameTextField.RegisterValueChangedCallback(evt => _filename = evt.newValue);
            toolbar.Add(fileNameTextField);
            toolbar.Add(new Button(()=>RequestDataOperation(true)) {text = "Save Data"});
            toolbar.Add(new Button(()=>RequestDataOperation(false)) {text = "Load Data"});

            rootVisualElement.Add(toolbar);

        }

        private void RequestDataOperation(bool save)
        {
            if (string.IsNullOrEmpty(_filename))
            {
                EditorUtility.DisplayDialog("Invalid file name!", "Please enter a valid file name", "OK");
                return;
            }
            var saveUtility = GraphSaveUtility.GetInstance(_graphView);
            if(save)
            {
                saveUtility.SaveGraph(_filename);
            }
            else
            {
                saveUtility.LoadGraph(_filename);
            }
        }


        private void OnDisable()
        {
            rootVisualElement.Remove(_graphView);
        }
    }
}

