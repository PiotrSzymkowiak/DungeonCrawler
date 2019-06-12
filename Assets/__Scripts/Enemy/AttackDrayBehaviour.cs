using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDrayBehaviour : IBehaviour
{

  private Enemy enemy;
  private Dray dray;

  public AttackDrayBehaviour(Enemy enemy, Dray dray)
  {
    this.enemy = enemy;
    this.dray = dray;
  }

  public void Do()
  {
    if (enemy.knockback) return;

    enemy.DestinationPoint.position = dray.transform.position;
  }
}
