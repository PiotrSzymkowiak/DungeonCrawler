using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radar : MonoBehaviour
{
  private Enemy enemy;
  float exitTime;

  private void Awake()
  {
    enemy = GetComponentInParent<Enemy>();
  }

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if(collision.CompareTag("Player"))
    {
      enemy.BehavController.ChangeBehaviour(BehavState.AttackDray);
    }
  }

  private void OnTriggerStay2D(Collider2D collision)
  {
    if (collision.CompareTag("Player"))
    {
      enemy.BehavController.ChangeBehaviour(BehavState.AttackDray);
    }
  }

  private void OnTriggerExit2D(Collider2D collision)
  {
    if (collision.CompareTag("Player"))
    {
      Invoke("ChangeStateToRndWalk", 5);      
    }
  }

  private void ChangeStateToRndWalk()
  {
    enemy.BehavController.ChangeBehaviour(BehavState.RndWalk);

  }

}
