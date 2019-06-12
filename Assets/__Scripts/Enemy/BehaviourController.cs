using UnityEngine;

public enum BehavState { RndWalk, AttackDray, RunForLife }

public class BehaviourController
{
  private Enemy enemy;
  private Dray dray;
  private IBehaviour behaviour;

  public BehavState CurrentState { get; private set; } = BehavState.RndWalk;

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
        CurrentState = BehavState.RndWalk;
        break;

      case BehavState.AttackDray:
        behaviour = new AttackDrayBehaviour(enemy, dray);
        CurrentState = BehavState.AttackDray;
        break;

      case BehavState.RunForLife:
        behaviour = new RunForLifeBehaviour(enemy, dray);
        CurrentState = BehavState.RunForLife;
        break;
    }
  }

  public void PerformBehaviour()
  {
    behaviour.Do();
  }

}
