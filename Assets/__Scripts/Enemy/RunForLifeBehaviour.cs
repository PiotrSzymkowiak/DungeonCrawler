using UnityEngine;

public class RunForLifeBehaviour : IBehaviour
{
  private Enemy enemy;
  private Dray dray;

  public RunForLifeBehaviour(Enemy enemy, Dray dray)
  {
    this.enemy = enemy;
    this.dray = dray;
  }

  public void Do()
  {
    if (enemy.knockback) return;

    Vector3 dirToDray = enemy.transform.position - dray.transform.position;
    enemy.DestinationPoint.position = enemy.transform.position + dirToDray.normalized * enemy.PathComputeDistance;

    if(dirToDray.magnitude > 10)
    {
      Debug.Log(dirToDray.magnitude);
      enemy.BehavController.ChangeBehaviour(BehavState.RndWalk);
      enemy.health = enemy.maxHealth / 2;
    }
  }
}