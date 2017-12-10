import UnityEngine
import UnityEditor
[CustomEditor(PolyDrawer)]
class PolyDrawerEditor(Editor):
    drawer as PolyDrawer
    editing as bool
    def OnEnable():
        drawer = (target cast PolyDrawer)
        drawer.Start()
    public override def OnInspectorGUI():
        DrawDefaultInspector()
        if editing:
            drawer.MyUpdate()
        if GUILayout.Button('Save Mesh'):
            //AssetDatabase.AddObjectToAsset(drawer.msh.sharedMesh, "Assets/Sprites/CreatedMeshes/MeshTester.prefab")
            AssetDatabase.CreateAsset(drawer.msh.sharedMesh, "Assets/Sprites/CreatedMeshes/" + drawer.gameObject.name + ".asset")
            AssetDatabase.SaveAssets()
            drawer.msh.mesh = AssetDatabase.LoadAssetAtPath("Assets/Sprites/CreatedMeshes/" + drawer.gameObject.name + ".asset", Mesh)
            editing = false

        if not editing:
            if GUILayout.Button('Edit'):
                editing = true
        else:
            if GUILayout.Button('Stop Editing'):
                editing = false
