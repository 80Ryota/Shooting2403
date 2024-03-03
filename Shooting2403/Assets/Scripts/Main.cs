using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// ゲーム内メインシーン
/// (シューティング)
/// </summary>
public class Main : MonoBehaviour
{
    // 弾薬最大数
    [SerializeField] private int m_MaxBulletNum = 5;
    // プレイヤープレハブ
    [SerializeField] private GameObject m_PlayerPrefab;
    // 弾薬プレハブ
    [SerializeField] private GameObject m_BulletPrefab;
    // 敵プレハブ
    [SerializeField] private GameObject m_EnemyPrefab;
    // 敵データ
    [SerializeField] public EnemyData[] m_EnemyData;
    // スコアテキスト
    [SerializeField] private TextMeshProUGUI m_ScoreText;
    // 体力テキスト
    [SerializeField] private TextMeshProUGUI m_LifeText;

    // プレイヤー
    private Player m_Player = null;
    // 弾薬リスト
    private List<Bullet> m_BulletList = new List<Bullet>();
    // 敵キュー
    private Queue<Enemy> m_EnemyQueue = new Queue<Enemy>();

    // ゲーム終了までの時間
    public static readonly int LimitTime = 30; 
    // 経過時間
    private float m_Time = 0f;
    // スコア
    private int m_Score = 0;
    // ライフ
    [SerializeField]private int m_Life = 3;
    // 初期化が完了したか
    private bool m_IsInitialized = false;
    // ゲーム終了したか
    private bool m_IsOver = false;
    // 通知:スコア
    private Subject<int> m_ScoreSubject;
    // 通知:ゲームオーバー
    private Subject<Unit> m_GameOverSubject;
    // 通知:ダメージ
    private Subject<Unit> m_DamageSubject;
    private void Start()
    {
        // 通知用意
        m_ScoreSubject  = new Subject<int>();
        m_ScoreSubject.Subscribe((x)=>AddScore(x));
        m_DamageSubject = new Subject<Unit>();
        m_DamageSubject.Subscribe((_) => Damage());
        m_GameOverSubject = new Subject<Unit>();
        m_GameOverSubject.Subscribe((_) => GameOver());
        
        // 初期化処理
        SetupObjectAsync().Forget();
    }

    /// <summary>
    /// ステージ上オブジェクトの初期設定
    /// </summary>
    private async UniTask SetupObjectAsync()
    {
        // プレイヤー
        var playerObj = GameObject.Instantiate(m_PlayerPrefab);
        m_Player = playerObj.GetComponent<Player>();
        
        // 弾
        for (var i = 0; i < m_MaxBulletNum; i++)
        {
            var obj = GameObject.Instantiate(m_BulletPrefab);
            var bullet = obj.GetComponent<Bullet>();
            m_BulletList.Add(bullet);
        }
        m_Player.SetBullet(m_BulletList);
        
        // 敵設定
        foreach (var enemyData in m_EnemyData)
        {
            var obj = GameObject.Instantiate(m_EnemyPrefab);
            var enemy = obj.GetComponent<Enemy>();
            enemy.Setup(enemyData, m_Player, m_ScoreSubject, m_DamageSubject, m_GameOverSubject);

            m_EnemyQueue.Enqueue(enemy);
        }
        
        // 初期化完了
        m_IsInitialized = true;
    }

    /// <summary>
    /// スコア追加
    /// </summary>
    /// <param name="add">加算値</param>
    private void AddScore(int add)
    {
        m_Score += add;
        m_ScoreText.text = $"スコア:{m_Score:d3}";
    }

    /// <summary>
    /// ゲームオーバー
    /// </summary>
    private void GameOver()
    {
        if (!m_IsOver)
        {
            PlayerPrefs.SetInt("Score", m_Score);
            SceneManager.LoadScene(2);
        }
    }

    /// <summary>
    /// ダメージ
    /// </summary>
    private void Damage()
    {
        m_Life -= 1;
        m_LifeText.text = new string('♡', m_Life);
        if (m_Life <= 0)
        {
            GameOver();
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (!m_IsInitialized)
        {
            return;
        }
        
        m_Time += Time.deltaTime;


        // 敵出現
        while (true)
        {
            if (m_EnemyQueue.Count == 0)
            {
                break;
            }

            var f = m_EnemyQueue.First();
            if (f.Data.SpawnTime > m_Time)
            {
                break;
            }

            f.Spawn();
            m_EnemyQueue.Dequeue();
        }

        if (m_Time > LimitTime)
        {
            GameOver();
        }
    }

    /// <summary>
    /// 終了
    /// </summary>
    private void OnDestroy()
    {
        m_ScoreSubject?.Dispose();
        m_DamageSubject?.Dispose();
        m_GameOverSubject?.Dispose();
        
    }
}