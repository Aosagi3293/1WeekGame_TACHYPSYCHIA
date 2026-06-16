using UnityEngine;
using System.Collections.Generic;

public class MapManager : MonoBehaviour
{
    [SerializeField] private List<Mesh> walls = new List<Mesh>();

    private void Start()
    {
        build();
    }

    private void build()
    {
        for(int i = 0; i < 6; i++)
        {
            // 生成したい「座標」と「角度」を決める
            Vector3 spawnPosition = new Vector3(0, 2, 5); // 好きな座標に変えられます
            Quaternion spawnRotation = Quaternion.Euler(0, i * 60, 0); // 回転（60ずつ）

            // 空っぽのゲームオブジェクトをその座標に作成する
            GameObject newWall = new GameObject("GeneratedWall");
            newWall.transform.position = spawnPosition;
            newWall.transform.rotation = spawnRotation;

            Mesh mesh = WallBuilder.Instance.BuildWall(newWall);

            walls.Add(mesh);
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
}
