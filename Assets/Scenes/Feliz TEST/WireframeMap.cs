using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class WireframeMap : MonoBehaviour
{
    [SerializeField] private Vector3[] vertices = new Vector3[4];
    [SerializeField] private int[] indices = new int[4];
    void Update()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.SetIndices(indices, MeshTopology.Lines, 0);

        GetComponent<MeshFilter>().mesh = mesh;
        // Assicurati di avere un materiale semplice (Unlit/Color) per disegnare le linee
    }
}
