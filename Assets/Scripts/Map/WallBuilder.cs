using UnityEngine;

public class WallBuilder : MonoBehaviour
{
    public static WallBuilder Instance { get; private set; }

    [SerializeField] private Material material;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void BuildWall(GameObject targetObject)
    {
        MeshFilter filter = targetObject.GetComponent<MeshFilter>();
        if (filter == null) filter = targetObject.AddComponent<MeshFilter>();

        MeshRenderer renderer = targetObject.GetComponent<MeshRenderer>();
        if (renderer == null) renderer = targetObject.AddComponent<MeshRenderer>();

        MeshCollider collider = targetObject.GetComponent<MeshCollider>();
        if (collider == null) collider = targetObject.AddComponent<MeshCollider>();

        // メッシュの作成
        Mesh mesh = new Mesh();
        mesh.name = "TrianglePrism";

        float side = 2f;

        float height = side * Mathf.Sqrt(3f) / 2f;

        // 頂点の定義 (6つの頂点で三角柱を構成)
        // 0-2: 前面, 3-5: 背面
        Vector3[] vertices = new Vector3[]
        {
            new Vector3(0, 0, 0),    // 0: 前面 上
            new Vector3(-1, 0, -height),  // 1: 前面 左下
            new Vector3(1, 0, -height),   // 2: 前面 右下
            new Vector3(0, -0.25f, 0),    // 3: 背面 上
            new Vector3(-1, -0.25f, -height),  // 4: 背面 左下
            new Vector3(1, -0.25f, -height)    // 5: 背面 右下
        };

        // 三角形の構成 (右回り（時計回り）じゃないとバグるらしい)
        int[] triangles = new int[]
        {
            // 前面
            0, 2, 1,
            // 背面
            3, 4, 5,
            // 左側面
            0, 1, 4, 0, 4, 3,
            // 右側面
            0, 5, 2, 0, 3, 5,
            // 底面
            1, 2, 5, 1, 5, 4
        };

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        // 法線を自動計算
        mesh.RecalculateNormals();

        // アタッチ
        filter.mesh = mesh;
        renderer.material = material;
        collider.sharedMesh = mesh;

        // 当たり判定用のタグをアタッチ
        targetObject.tag = "Wall";
    }
}