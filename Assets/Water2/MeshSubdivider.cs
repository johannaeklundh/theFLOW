using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshSubdivider : MonoBehaviour
{
    void Start()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        Mesh mesh = meshFilter.mesh;
        Vector3[] oldVertices = mesh.vertices;
        int[] oldTriangles = mesh.triangles;

        // Lists to hold new vertices and triangles
        System.Collections.Generic.List<Vector3> newVertices = new System.Collections.Generic.List<Vector3>();
        System.Collections.Generic.List<int> newTriangles = new System.Collections.Generic.List<int>();

        // Process each triangle and create 4 new ones
        for (int i = 0; i < oldTriangles.Length; i += 3)
        {
            Vector3 v0 = oldVertices[oldTriangles[i]];
            Vector3 v1 = oldVertices[oldTriangles[i + 1]];
            Vector3 v2 = oldVertices[oldTriangles[i + 2]];

            Vector3 v01 = (v0 + v1) / 2;
            Vector3 v12 = (v1 + v2) / 2;
            Vector3 v20 = (v2 + v0) / 2;

            int i0 = newVertices.Count; newVertices.Add(v0);
            int i1 = newVertices.Count; newVertices.Add(v1);
            int i2 = newVertices.Count; newVertices.Add(v2);
            int i01 = newVertices.Count; newVertices.Add(v01);
            int i12 = newVertices.Count; newVertices.Add(v12);
            int i20 = newVertices.Count; newVertices.Add(v20);

            // Add new triangles: 4 triangles from what was previously 1
            newTriangles.AddRange(new int[] { i0, i01, i20 });
            newTriangles.AddRange(new int[] { i01, i1, i12 });
            newTriangles.AddRange(new int[] { i12, i2, i20 });
            newTriangles.AddRange(new int[] { i01, i12, i20 });
        }

        // Update mesh with new vertices and triangles
        mesh.Clear();
        mesh.vertices = newVertices.ToArray();
        mesh.triangles = newTriangles.ToArray();
        mesh.RecalculateNormals(); // To make sure lighting reacts well
    }
}
