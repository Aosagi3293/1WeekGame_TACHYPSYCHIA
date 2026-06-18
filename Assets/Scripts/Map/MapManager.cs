using UnityEngine;
using System.Collections.Generic;

public class MapManager : MonoBehaviour
{
    private float timer;

    private bool canBuild = true;
    
    [Space(10)]
    [SerializeField] private float magnification;

    [Space(10)]
    [SerializeField] private GameObject boundary;

    // 外壁の高さ
    private float height = 200f;

    [SerializeField] private List<GameObject> walls = new List<GameObject>();

    private void Start()
    {
        var baseSize = 1.5f * magnification;
        var radius = baseSize;

        BuildBoundary(radius);
    }

    private void Update()
    {
        if(GameManager.Instance == null) return;
        if(GameManager.Instance.state != GameState.Play) return;

        timer += Time.deltaTime;

        if(timer > 0 && canBuild == true)
        {
            build();
            canBuild = false;
        }
        else if(timer > 4f)
        {
            timer = 0;
            canBuild = true;
        }
    }

    private void build()
    {
        for(int i = 0; i < 6; i++)
        {
            // 生成したい「座標」と「角度」を決める
            Vector3 spawnPosition = new Vector3(0, -40, 0); // 好きな座標に変えられます
            Quaternion spawnRotation = Quaternion.Euler(0, 30+ i * 60, 0); // 回転（60ずつ）

            // 空っぽのゲームオブジェクトをその座標に作成する
            GameObject newWall = new GameObject("GeneratedWall");
            newWall.transform.position = spawnPosition;
            newWall.transform.rotation = spawnRotation;

            // スクリプトのアタッチ
            newWall.AddComponent<WallController>();

            WallBuilder.Instance.BuildWall(newWall);

            newWall.transform.localScale = Vector3.one * magnification;

            walls.Add(newWall);
        }

        // リストのシャッフル
        for(var i = walls.Count - 1; i > 0; i--)
        {
            var j = Random.Range(0, i + 1);

            // i番目とj番目の要素を交換する
            var tmp = walls[i];
            walls[i] = walls[j];
            walls[j] = tmp;
        }

        // 一か所穴をあける
        Destroy(walls[0]);
        
        // 次の生成に備えてリストの削除
        for(int i = walls.Count - 1; i >= 0; i--)
        {
            walls.Remove(walls[i]);
        }
    }

    // 外壁の生成
    public void BuildBoundary(float radius)
    {
        var playerPos = PlayerController.Instance.ReturnPosition();

        for(int i = 0; i < 6; i++)
        {
            float angle = i * Mathf.PI / 3f; // 60度

            Vector3 pos = new Vector3(
                Mathf.Cos(angle) * radius,
                playerPos,
                Mathf.Sin(angle) * radius
            );

            // 生成
            var wall = Instantiate(boundary, pos, Quaternion.identity);

            wall.transform.localScale = new Vector3(magnification, height, 1f);

            // 内側を向かせる
            wall.transform.rotation = Quaternion.LookRotation(-pos.normalized);

            // タグ付け
            wall.tag = "Boundary";
        }
    }
}
