using System;
using System.Collections;
using System.Collections.Generic;
using R3;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// 敵オブジェクト
/// </summary>
public class Enemy : MonoBehaviour
{
    // 敵の弾
    [SerializeField]private EnemyBullet m_EnemyBullet;
    // 敵情報
    public EnemyData Data => m_Data;
    private EnemyData m_Data;
    
    // スポーン済みか
    private bool m_IsSpawn = false;
    // 生成後経過時間
    private float m_Time = 0f;
    // スコア通知
    private ISubject<int> m_ScoreSubject;
    // ダメージ通知
    private ISubject<int> m_DamageSubject;
    // ゲームオーバー通知
    private ISubject<Unit> m_GameoverSubject;
    // 弾発射までの猶予時間
    private float m_WaitShotTime = 0f;

    /// <summary>
    /// 敵情報設定
    /// </summary>
    /// <param name="data">データ</param>
    /// <param name="player">プレイヤー</param>
    /// <param name="scoreSubject">スコア通知</param>
    /// <param name="damageSubject">ダメージ通知</param>
    /// <param name="gameoverSubject">ゲームオーバー通知</param>
    public void Setup(EnemyData data, Player player, ISubject<int> scoreSubject, ISubject<Unit> damageSubject, ISubject<Unit> gameoverSubject)
    {
        // 位置設定
        m_Data = data;
        var pos = transform.position;
        pos.x = data.XPos;
        transform.position = pos;
        
        // 通知を保持
        m_ScoreSubject = scoreSubject;
        m_GameoverSubject = gameoverSubject;
        
        // 弾設定
        m_EnemyBullet.Set(player, m_Data, damageSubject,  data.Speed * 1.2f);
        gameObject.SetActive(false);
        
    }

    /// <summary>
    /// 生成(動作開始)
    /// </summary>
    public void Spawn()
    {
        // 位置を上に持ってくる
        var pos = transform.position;
        pos.y = 20f;
        transform.position = pos;
        // 発射までの時間をランダムで設定
        m_WaitShotTime = Random.Range(0.3f, 1.0f);
        gameObject.SetActive(true);
        m_IsSpawn = true;
    }
    
    /// <summary>
    /// 更新
    /// </summary>
    public void Update()
    {
        if (!m_IsSpawn)
        {
            return;
        }
        
        // 画面下に落下
        transform.position += Vector3.down * m_Data.Speed * Time.deltaTime;

        m_Time += Time.deltaTime;

        // 弾発射のタイミングなら弾動作開始
        if (m_WaitShotTime < m_Time && m_WaitShotTime > 0f)
        {
            m_EnemyBullet.Shot(transform.position);
            m_WaitShotTime = -1f;
        }

        // 画面下にきたら処理終了
        if (transform.position.y < -30f)
        {
            gameObject.SetActive(false);
        }
    }

   

    public void OnTriggerEnter(Collider other)
    {
        if (!m_IsSpawn)
        {
            return;
        }
        if (other.CompareTag("bullet"))
        {
            // 弾がヒットしたならスコア加算
            m_ScoreSubject.OnNext(1);
            gameObject.SetActive(false);
        }

        if (other.CompareTag("player"))
        {
            // プレイヤーがヒットしたなら終了
            m_GameoverSubject.OnNext(Unit.Default);
            gameObject.SetActive(false);
        }
        

    }
}
