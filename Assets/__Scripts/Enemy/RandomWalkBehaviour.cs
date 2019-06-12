using UnityEngine;

public class RandomWalkBehaviour :  IBehaviour
{
  public Enemy enemy;

  public float timeThinkMin = 1f;
  public float timeThinkMax = 4f;

  public int facing = 0;
  public float timeNextDecision = 0;

  public RandomWalkBehaviour(Enemy enemy)
  {
    this.enemy = enemy;
  }

  public void Do()
  {
    if (enemy.knockback) return;

    if (Time.time >= timeNextDecision)
    {
      DecideDirection();
    }

    enemy.DestinationPoint.position = enemy.transform.position + Enemy.directions[facing] * enemy.PathComputeDistance;
  }

  void DecideDirection()
  {
    facing = Random.Range(0, 4);
    timeNextDecision = Time.time + Random.Range(timeThinkMin, timeThinkMax);
  }
}
