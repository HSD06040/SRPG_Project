using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class MeshVertexDragEditor : MonoBehaviour
{
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private float vertexSelectRadius = 0.3f;
    [SerializeField] public bool enableEditing = true;

    private Mesh deformingMesh;
    private Vector3[] originalVertices;
    private Vector3[] currentVertices;
    private int selectedVertexIndex = -1;

    void OnEnable()
    {
        if (meshFilter == null)
            meshFilter = GetComponent<MeshFilter>();

        InitializeMesh();
    }

    void InitializeMesh()
    {
        if (meshFilter != null && meshFilter.sharedMesh != null)
        {
            deformingMesh = meshFilter.sharedMesh;
            originalVertices = deformingMesh.vertices;
            currentVertices = (Vector3[])originalVertices.Clone();
        }
    }

    public void MoveSelectedVertex(Vector3 worldPosition)
    {
        if (selectedVertexIndex < 0 || currentVertices == null)
            return;

        Vector3 localPos = transform.worldToLocalMatrix.MultiplyPoint(worldPosition);
        currentVertices[selectedVertexIndex] = localPos;
        ApplyMeshDeformation();
    }

    public int GetVertexAtWorldPoint(Vector3 worldPos)
    {
        if (currentVertices == null)
            return -1;

        for (int i = 0; i < currentVertices.Length; i++)
        {
            Vector3 vertexWorldPos = transform.TransformPoint(currentVertices[i]);
            float distance = Vector3.Distance(vertexWorldPos, worldPos);

            if (distance < vertexSelectRadius)
            {
                return i;
            }
        }

        return -1;
    }

    void ApplyMeshDeformation()
    {
        if (deformingMesh == null)
            return;

        deformingMesh.vertices = currentVertices;
        deformingMesh.RecalculateNormals();
        deformingMesh.RecalculateBounds();

        if (TryGetComponent<MeshCollider>(out var meshCollider))
        {
            meshCollider.convex = false;
            meshCollider.sharedMesh = null;
            meshCollider.sharedMesh = deformingMesh;
        }
    }

    void OnDrawGizmos()
    {
        if (!enableEditing || currentVertices == null)
            return;

        // 모든 버텍스 그리기
        Gizmos.color = Color.white;
        for (int i = 0; i < currentVertices.Length; i++)
        {
            Vector3 worldPos = transform.TransformPoint(currentVertices[i]);
            Gizmos.DrawSphere(worldPos, 0.08f);
        }

        // 선택된 버텍스 강조
        if (selectedVertexIndex >= 0 && selectedVertexIndex < currentVertices.Length)
        {
            Gizmos.color = Color.red;
            Vector3 selectedWorldPos = transform.TransformPoint(currentVertices[selectedVertexIndex]);
            Gizmos.DrawSphere(selectedWorldPos, 0.15f);
        }
    }

    public void ResetMesh()
    {
        currentVertices = (Vector3[])originalVertices.Clone();
        ApplyMeshDeformation();
        selectedVertexIndex = -1;
    }

    public int SelectedVertexIndex => selectedVertexIndex;
    public void SetSelectedVertex(int index) => selectedVertexIndex = index;
}