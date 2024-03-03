using System;
using System.Collections;
using System.Collections.Generic;
using R3;
using UnityEngine;

/// <summary>
/// 弾
/// </summary>
public class Bullet : MonoBehaviour
{

    // 基本速度
    [SerializeField]protected float m_BaseSpeed;
    // 加速度
    [SerializeField]protected float m_AdditiveSpeed;
    // 寿命
    [SerializeField]protected float m_LifeTime = 5f;

    // 弾を飛ばす方向
    protected Vector3 m_Direction = Vector3.up;
    
    // 弾としてうごいている最中か
    public bool IsAlive => m_IsAlive;
    protected bool m_IsAlive = false;
    
    // 経過時間
    protected float m_Time = 0f;
    // 現在の速度
    protected float m_Speed = 0f;

    // Update is called once per frame
    void Update()
    {
        if (m_IsAlive)
        {
            OnUpdate();
            m_Time += Time.deltaTime;
            
        }
    }

    public virtual void OnUpdate()
    {
        
        m_Speed += m_AdditiveSpeed * Time.deltaTime;
        transform.position += m_Direction * m_Speed * Time.deltaTime;
        if (m_Time > m_LifeTime)
        {
            Stop();
        }
    }

    /// <summary>
    /// 停止
    /// </summary>
    public void Stop()
    {
            m_Speed = 0f;
            m_Time = 0f;
            m_IsAlive = false;
    }
    
    /// <summary>
    /// 発射
    /// </summary>
    /// <param name="startPos"></param>
    public void Shot(Vector3 startPos)
    {
        transform.SetParent(null);
        m_Speed = m_BaseSpeed;
        transform.position = startPos;
        m_IsAlive = true;
        gameObject.SetActive(true);
    }
}
