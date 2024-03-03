using System;
using System.Collections;
using System.Collections.Generic;
using R3;
using UnityEngine;

/// <summary>
/// 敵の弾
/// </summary>
public class EnemyBullet : Bullet
{
    // 敵情報
    private EnemyData m_Data;
    // ダメージ通知
    private ISubject<Unit> m_DamageSubject;
    // プレイヤー
    private Player m_Player;
    // Start is called before the first frame update
    void Start()
    {
        m_Direction = Vector3.down;
    }

    public override void OnUpdate()
    {
        switch (m_Data.Type)
        {
            case EnemyType.Standard:
            {
                // 直線落下
                OnUpdateStandard();
                break;
            }
            case EnemyType.Circle:
            {
                // 円形落下
                OnUpdateCircle();
                break;
            }
            case EnemyType.DirectAttack:
            {
                // 自機に接近
                OnUpdateDirectAttack();
                break;
            }
        }

        // 画面下に来たら止める
        if (transform.position.y < m_Player.transform.position.y - 10)
        {
            Stop();
        }
    }
    
    /// <summary>
    /// 直線落下
    /// </summary>
    public void OnUpdateStandard()
    {
        transform.position += Vector3.down * m_Data.Speed * Time.deltaTime;
    }

    /// <summary>
    /// 円形落下
    /// </summary>
    public void OnUpdateCircle()
    {
        var r = m_Time * Mathf.PI;
        var pos = transform.position;
        pos.x += (Mathf.Cos(r) * m_Data.Speed) * Time.deltaTime;   
        pos.y += -(m_Data.Speed * 0.5f + Mathf.Sin(r) * m_Data.Speed * 0.5f) * Time.deltaTime;   
        transform.position = pos;
    }

    /// <summary>
    /// 自機に接近
    /// </summary>
    public void OnUpdateDirectAttack()
    {
        var pos = transform.position;
        var diff = pos - m_Player.transform.position;
        pos.x += -diff.x * Time.deltaTime;
        pos.y += -(m_Data.Speed * 0.5f + diff.y / m_Data.Speed) * Time.deltaTime;
        transform.position = pos;
    }

    /// <summary>
    /// 弾情報設定
    /// </summary>
    /// <param name="player">プレイヤー</param>
    /// <param name="data">敵情報</param>
    /// <param name="damageSubject"><ダメージ通知/param>
    /// <param name="speed">速度</param>
    /// <param name="lifeTime"></param>
    public void Set(Player player, EnemyData data, ISubject<Unit> damageSubject, float speed)
    {
        m_Player = player;
        m_Data = data;
        m_DamageSubject = damageSubject;
        m_BaseSpeed = speed;
        m_AdditiveSpeed = 0f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("player"))
        {
            // 自機ならダメージ通知
            m_DamageSubject.OnNext(Unit.Default);
            gameObject.SetActive(false);
        }

        if (other.CompareTag("bullet"))
        {
            // 弾と当たったら消える
            gameObject.SetActive(false);
        }
    }
}
