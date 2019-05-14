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

  private InRoom inRm;

  public bool moving { get => true; }
  public float gridMult { get => inRm.gridMult; }
  public Vector2 roomPos { get => inRm.roomPos; set => inRm.roomPos = value; }
  public Vector2 roomNum { get => inRm.roomNum; set => inRm.roomNum = value; }

  public int GetFacing() => facing;
  public float GetSpeed() => speed;
  public Vector2 GetRoomPosOnGrid(float mult = -1) => inRm.GetRoomPosOnGrid(mult);

  protected override void Awake()
  {
    base.Awake();
    inRm = GetComponent<InRoom>();
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
