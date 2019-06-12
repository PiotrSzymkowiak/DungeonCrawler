using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : Enemy
{
  public float attackRange;
  private float lastAttackTime;
  public float attackDelay;

  public GameObject projectile;
  public float ballForce;

  protected override void Update()
  {
    base.Update();

    float distanceToPlayer = Vector3.Distance(transform.position, dray.transform.position);
    Vector3 direction = dray.transform.position - transform.position;
    if (distanceToPlayer < attackRange)
    {
      if (Time.time > lastAttackTime + attackDelay)
      {
        GameObject newBall = GameObject.Instantiate(projectile, transform.position, transform.rotation) as GameObject;
        newBall.GetComponent<Rigidbody2D>().AddRelativeForce(direction.normalized * ballForce);
        lastAttackTime = Time.time;
      }
    }
  }
}
