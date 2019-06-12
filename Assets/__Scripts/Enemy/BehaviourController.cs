using UnityEngine;

public enum BehavState { RndWalk, AttackDray }

public class BehaviourController
{
  private Enemy enemy;
  private Dray dray;
  private IBehaviour behaviour;

  public BehaviourController(Enemy enemy, Dray dray)
  {
    this.enemy = enemy;
    this.dray = dray;
    this.behaviour = new RandomWalkBehaviour(enemy);
  }

  public void ChangeBehaviour(BehavState state)
  {
    switch (state)
    {
      case BehavState.RndWalk: 
        behaviour = new RandomWalkBehaviour(enemy);
        break;

      case BehavState.AttackDray:
        behaviour = new AttackDrayBehaviour(enemy, dray);
        break;
    }
  }

  public void PerformBehaviour()
  {
    behaviour.Do();
  }

}
