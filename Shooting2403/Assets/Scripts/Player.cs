using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 自機
/// </summary>
public class Player : MonoBehaviour
{
    // 移動速度
    [SerializeField] private float m_DefaultSpeed;
    // 移動方向
    private Vector3 m_Distance;
    // 発射要求
    private bool m_IsRequestShot;
    // 現在の速度
    private float m_Speed = 0f;
    // 発射クールタイム
    private float m_DefaultCoolTime = 0.1f;
    // 現在の発射クールタイム
    private float m_CoolTime = 0f;

    // 弾
    private IEnumerable<Bullet> m_Bullets;
    
    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        bool isAxisInput = false;
        bool isBrake = false;
        
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            // 左矢印が押された
            if (m_Distance.x <= 0f)
            {
                isAxisInput = true;
                m_Distance += Vector3.left;
            }
            else
            {
                isBrake = true;
            }

        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            // 右矢印が押された
            if (m_Distance.x >= 0f)
            {
                isAxisInput = true;
                m_Distance += Vector3.right;
            }
            else
            {
                isBrake = true;
            }
        }

        if (!isAxisInput)
        {
            // 減速
            float decSpeed = m_DefaultSpeed * Time.deltaTime;
            if (isBrake)
            {
                // 逆方向にキーが押された場合はさらに減速
                decSpeed *= 4f;
            }
           
            m_Speed =  Math.Clamp(m_Speed - decSpeed, 0f, m_DefaultSpeed);;
        }
        else
        {
            m_Speed = Math.Clamp(m_Speed + (m_DefaultSpeed * Time.deltaTime), 0f, m_DefaultSpeed);
        }
        
        // 移動
        m_Distance = m_Distance.normalized * m_Speed;
        var pos = transform.position;
        pos += m_Distance * Time.deltaTime;
        // 端にきたとき
        if (Mathf.Abs(pos.x) > 30f)
        {
            pos.x = Math.Clamp(pos.x, -30f, 30f);
            m_Speed = 0f;
        }
        
        // 発射
        if (m_CoolTime > 0f)
        {
            m_CoolTime -= Time.deltaTime;
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Shot();
                m_IsRequestShot = false;
            }
        }

        
        transform.position = pos;
    }

    /// <summary>
    /// 発射
    /// </summary>
    private void Shot()
    {
        foreach (var b in m_Bullets)
        {
            if (!b.IsAlive)
            {
                b.Shot(transform.position);
                m_CoolTime = m_DefaultCoolTime;
                return;
            }
        }

    }

    /// <summary>
    /// 弾
    /// </summary>
    /// <param name="bullets"></param>
    public void SetBullet(IEnumerable<Bullet> bullets)
    {
        m_Bullets = bullets;
    }
}
