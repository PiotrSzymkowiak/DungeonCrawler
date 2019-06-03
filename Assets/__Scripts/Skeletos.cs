using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeletos : Enemy, IFacingMover
{
  [Header("Defined in inspection panel: Skeletos")]
  public int speed = 2;
  public float timeThinkMin = 1f;
  public float timeThinkMax = 4f;

  [Header("Defined dynamically: Skeletos")]
  public int facing = 0;
  public float timeNextDecision = 0;

  public bool moving { get => true; }
  public int GetFacing() => facing;
  public float GetSpeed() => speed;

  protected override void Awake()
  {
    base.Awake();

  }

  protected override void Update()
  {
    base.Update();
    if (knockback) return;

    if(Time.time >= timeNextDecision)
    {
      DecideDirection();
    }

    rigid.velocity = directions[facing] * speed;
  }

  void DecideDirection()
  {
    facing = Random.Range(0, 4);
    timeNextDecision = Time.time + Random.Range(timeThinkMin, timeThinkMax);
  }
}
