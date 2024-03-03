using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemyData
{
   public static readonly int Count = 4;
   
   public EnemyType Type;
   public float SpawnTime;
   public float XPos;
   public float Speed;

}
