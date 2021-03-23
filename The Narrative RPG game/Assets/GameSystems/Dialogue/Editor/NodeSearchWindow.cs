using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameSystems.Dialogue.Editor
{
    public class NodeSearchWindow : ScriptableObject, ISearchWindowProvider
    {
        private DialogueGraphView _graphView;
        private EditorWindow _window;
        private Texture2D _instantiationIcon;

        public void Init(EditorWindow window,DialogueGraphView graphView)
        {
            _graphView = graphView;
            _window = window;

            _instantiationIcon = new Texture2D(1, 1);
            _instantiationIcon.SetPixel(0,0,new Color(0,0,0,0));
            _instantiationIcon.Apply();


        }
        
        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            var tree = new List<SearchTreeEntry>
            {
                new SearchTreeGroupEntry(new GUIContent("Create Elements:"), 0),
                new SearchTreeGroupEntry(new GUIContent("Dialogue node"), 1),
                new SearchTreeEntry(new GUIContent("Dialogue node", _instantiationIcon))
                {
                    userData = new DialogueNode(),level = 2
                }
            };
            
            return tree;
        }

        public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {
            var worldMousePosition = _window.rootVisualElement.
                ChangeCoordinatesTo(_window.rootVisualElement.parent, context.screenMousePosition - _window.position.position);
            var localMousePosition = _graphView.contentContainer.WorldToLocal(worldMousePosition);
            switch (SearchTreeEntry.userData)
            {
                case DialogueNode dialogueNode:
                    _graphView.CreateNode("Dialogue node", localMousePosition);
                    return true;
                    break;
                default:
                    return false;
                    break;
            }
        }
    }
}