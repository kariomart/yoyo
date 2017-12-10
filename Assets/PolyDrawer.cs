using UnityEngine;

//[ExecuteInEditMode]
public class PolyDrawer : MonoBehaviour {
    public PolygonCollider2D poly;
    public MeshFilter msh;
    public Vector2 pos;

    public void Start() {
        poly = GetComponent<PolygonCollider2D>();
        msh = GetComponent<MeshFilter>();
    }
    public void MyUpdate() {
        Vector2[] pts = poly.points;
        int[] tris = new Triangulator(pts).Triangulate();
        Vector2[] uv = new Vector2[pts.Length];
        for (int i = 0; i < pts.Length; i++) {
            uv[i] = Camera.main.WorldToScreenPoint(pts[i]);
            Mesh mesh = new Mesh();
            msh.mesh = mesh;
            Vector3[] points = new Vector3[pts.Length];
            for (int j = 0; j < pts.Length; j++) {
                points[j] = pts[j];
            }
            mesh.vertices = points;
            mesh.triangles = tris;
            mesh.uv = uv;
        }
    }
}
