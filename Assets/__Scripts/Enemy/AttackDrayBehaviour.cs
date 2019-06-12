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
    Vector3 dirToDray = dray.transform.position - enemy.transform.position;
    enemy.Rigid.velocity = dirToDray.normalized * enemy.speed;
  }
}
