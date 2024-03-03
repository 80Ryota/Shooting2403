using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditor.SceneManagement;

/// <summary>
/// CSVから敵データを読み込んで設定する
/// </summary>
public class LoadEnemyDataFromCsv : EditorWindow {
    
    [MenuItem("Tools/LoadEnemyDataFromCsv")]
    static void Init() {
        GetWindow<LoadEnemyDataFromCsv>();
    }

    private void OnGUI()
    {
        GUILayout.Label("Mainシーンを開いた状態で行なってください");
        if (GUILayout.Button("csvファイル選択"))
        {
            //パスの取得
            var path = EditorUtility.OpenFilePanel("Open csv", "", "CSV");
            if (string.IsNullOrEmpty(path))
                return;
            Debug.Log(path);

            //読み込み
            var reader = new StreamReader(path);


            List<EnemyData> enemyDataList = new List<EnemyData>();
            EnemyData enemyData = new EnemyData();

            while (reader.Peek() != -1)
            {
                // 1行ずつ処理
                var dataIndex = 0;
                var line = reader.ReadLine();
                var split = line.Split(",");
                for (int i = 0; i < split.Length; i++)
                {

                    switch (dataIndex)
                    {
                        case 0:
                        {
                            // タイプ
                            enemyData.Type = (EnemyType)int.Parse(split[i]);
                            break;
                        }
                        case 1:
                        {
                            // 出現時間
                            enemyData.SpawnTime = float.Parse(split[i]);
                            break;
                        }
                        case 2:
                        {
                            // 速度
                            enemyData.Speed = float.Parse(split[i]);
                            break;
                        }
                        case 3:
                        {
                            // X座標
                            enemyData.XPos = float.Parse(split[i]);
                            enemyDataList.Add(enemyData);
                            enemyData = new EnemyData();
                            break;
                        }


                    }

                    dataIndex = (dataIndex + 1) % EnemyData.Count;
                }
            }


            // Mainシーン内の敵データに保存
            var mainObj = GameObject.Find("Main");
            var main = mainObj.GetComponent<Main>();
            main.m_EnemyData = enemyDataList.ToArray();

            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            EditorSceneManager.SaveOpenScenes();
        }
    }


}