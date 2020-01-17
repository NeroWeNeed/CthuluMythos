namespace CMythos
{
    using UnityEngine;
    using UnityEditor;
    using UnityEngine.UIElements;
    using UnityEditor.UIElements;

    [CustomEditor(typeof(GameBoardLayoutUtil))]
    public class GameBoardLayoutUtilEditor : Editor
    {
        private VisualElement rootElement;



        private Button refresh;
        private ObjectField gameBoardManagerField;
        private ObjectField cameraField;
        private ObjectField playMatRendererField;
        private void OnEnable()
        {
            rootElement = new VisualElement();
            refresh = new Button
            {
                text = "Refresh Layout"
            };
            refresh.clickable.clicked += () => UpdateLayout();

            gameBoardManagerField = new ObjectField("GameBoard Manager")
            {
                objectType = typeof(GameBoardManager)

            };
            gameBoardManagerField.RegisterValueChangedCallback(x => UpdateFields(false));
            cameraField = new ObjectField("Camera Field")
            {
                objectType = typeof(Camera)
            };
            cameraField.RegisterValueChangedCallback(x => UpdateFields(false));
            playMatRendererField = new ObjectField("Play Mat Renderer Field")
            {
                objectType = typeof(PlayMatRenderer)
            };
            playMatRendererField.RegisterValueChangedCallback(x => UpdateFields(false));

        }
        public override VisualElement CreateInspectorGUI()
        {
            rootElement.Clear();
            rootElement.Add(gameBoardManagerField);
            rootElement.Add(cameraField);
            rootElement.Add(playMatRendererField);
            rootElement.Add(refresh);
            UpdateFields(true);
            return rootElement;
        }
        private void UpdateLayout()
        {
            ((GameBoardLayoutUtil)target).Layout();
        }
        private void UpdateFields(bool fromTarget)
        {
            GameBoardLayoutUtil layoutUtil = (GameBoardLayoutUtil)target;
            if (fromTarget)
            {
                playMatRendererField.value = layoutUtil.PlayMatRenderer;
                gameBoardManagerField.value = layoutUtil.GameBoardManager;
                cameraField.value = layoutUtil.Camera;
            }
            else
            {
                layoutUtil.PlayMatRenderer = (PlayMatRenderer)playMatRendererField.value;
                layoutUtil.GameBoardManager = (GameBoardManager)gameBoardManagerField.value;
                layoutUtil.Camera = (Camera)cameraField.value;
                EditorUtility.SetDirty(target);
            }
            rootElement.MarkDirtyRepaint();

        }

    }
}