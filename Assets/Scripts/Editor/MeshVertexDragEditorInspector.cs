#if UNITY_EDITOR
using static UnityEngine.GraphicsBuffer;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MeshVertexDragEditor))]
public class MeshVertexDragEditorInspector : Editor
{
    private MeshVertexDragEditor editor;
    private int hoveredVertexIndex = -1;
    private Camera sceneCamera;

    void OnEnable()
    {
        editor = (MeshVertexDragEditor)target;
        SceneView.duringSceneGui += OnSceneGUI;
    }

    void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    void OnSceneGUI(SceneView sceneView)
    {
        if (editor == null || !editor.enableEditing)
            return;

        sceneCamera = sceneView.camera;
        if (sceneCamera == null)
            return;

        Event e = Event.current;
        Vector2 mousePos = e.mousePosition;

        Ray ray = sceneCamera.ScreenPointToRay(
            new Vector3(mousePos.x, sceneView.position.height - mousePos.y, 0)
        );

        Vector3 rayPoint = ray.origin + ray.direction * 100f;
        hoveredVertexIndex = editor.GetVertexAtWorldPoint(rayPoint);

        HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

        if (e.type == EventType.MouseDown && e.button == 0 && hoveredVertexIndex >= 0)
        {
            editor.SetSelectedVertex(hoveredVertexIndex);
            e.Use();
        }

        if (e.type == EventType.MouseDrag && e.button == 0 && editor.SelectedVertexIndex >= 0)
        {
            Vector3 worldPos = ray.origin + ray.direction * 100f;
            editor.MoveSelectedVertex(worldPos);
            e.Use();
        }

        if (e.type == EventType.MouseUp)
        {
        }

        sceneView.Repaint();
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GUILayout.Space(10);
        GUILayout.Label("Vertex Editor", EditorStyles.boldLabel);

        if (editor.SelectedVertexIndex >= 0)
        {
            GUILayout.Label($"Selected Vertex: {editor.SelectedVertexIndex}",
                EditorStyles.helpBox);
            GUILayout.Label("드래그해서 이동하세요", EditorStyles.miniLabel);
        }
        else
        {
            GUILayout.Label("Scene에서 흰 구(버텍스)를 클릭하세요", EditorStyles.helpBox);
        }

        GUILayout.Space(10);
        if (GUILayout.Button("Reset Mesh", GUILayout.Height(30)))
        {
            editor.ResetMesh();
        }
    }
}
#endif