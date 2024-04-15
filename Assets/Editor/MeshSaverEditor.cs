using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(MeshFilter))]
public class MeshSaverEditor : MonoBehaviour
{
    [ContextMenu("Save Mesh")]
    public void SaveMesh()
    {
        MeshFilter mf = GetComponent<MeshFilter>();
        if (mf)
        {
            SaveAsset(mf.mesh);
        }
        else
        {
            Debug.LogError("No mesh filter found for the object " + gameObject.name);
        }
    }

    void SaveAsset(Mesh mesh)
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Mesh Asset", mesh.name, "asset", "Save mesh as asset");
        if (string.IsNullOrEmpty(path)) return;

        Mesh newMesh = new Mesh();
        newMesh.vertices = mesh.vertices;
        newMesh.triangles = mesh.triangles;
        newMesh.normals = mesh.normals;
        newMesh.uv = mesh.uv;

        AssetDatabase.CreateAsset(newMesh, path);
        AssetDatabase.SaveAssets();
    }
}
