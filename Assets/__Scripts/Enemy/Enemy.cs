using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
  public static Vector3[] directions = new Vector3[]
  {
      Vector3.right,
      Vector3.up,
      Vector3.left,
      Vector3.down
  };

  [Header("Defined in inspection panel: Enemy")]
  public float maxHealth = 1;
  public float knockbackSpeed = 10;
  public float knockbackDuration = 0.25f;
  public float invincibleDuration = 0.5f;
  public float speed = 1.5f;
  public GameObject[] randomItemDrops;
  public GameObject guaranteedItemDrop = null;

  [Header("Defined dynamically: Enemy")]
  public float health;
  public bool invincible = false;
  public bool knockback = false;

  private float knockbackDone = 0;
  private float invincibleDone = 0;
  private Vector3 knockbackVel;

  protected BehaviourController behavController;
  protected Dray dray;

  protected Animator anim;
  protected Rigidbody2D rigid;
  protected SpriteRenderer sRend;


  public Animator Anim { get => anim; }
  public Rigidbody2D Rigid { get => rigid; }
  public SpriteRenderer SRend { get => sRend; }
  public BehaviourController BehavController { get => behavController; }

  protected virtual void Awake()
  {
    health = maxHealth;
    anim = GetComponent<Animator>();
    rigid = GetComponent<Rigidbody2D>();
    sRend = GetComponent<SpriteRenderer>();
    dray = FindObjectOfType<Dray>();
    behavController = new BehaviourController(this, dray);
  }

  protected virtual void Update()
  {
    invinicbleHandle();
    if(knockbackVelHandle()) return;
    behaviourHandle();
  }

  private bool knockbackVelHandle()
  {
    if (knockback)
    {
      rigid.velocity = knockbackVel;
      if (Time.time < knockbackDone) return true;
    }

    anim.speed = 1;
    knockback = false;
    return false;
  }

  private void invinicbleHandle()
  {
    if (invincible && Time.time > invincibleDone) invincible = false;
    sRend.color = invincible ? Color.red : Color.white;
  }

  private void behaviourHandle()
  {
    behavController.PerformBehaviour();
  }

  void OnTriggerEnter2D(Collider2D colld)
  {
    if (!colld.CompareTag("Sword")) return;
    if (invincible) return;
    DamageEffect dEf = colld.gameObject.GetComponent<DamageEffect>();
    if (dEf == null) return;

    health -= dEf.damage;
    if (health <= 0) Die();

    invincible = true;
    invincibleDone = Time.time + invincibleDuration;

    if (dEf.knockback)
    {
      Vector3 delta = transform.position - colld.transform.position;
      if (Mathf.Abs(delta.x) >= Mathf.Abs(delta.y))
      {
        //knockback horizontally
        delta.x = (delta.x > 0) ? 1 : -1;
        delta.y = 0;
      }
      else
      {
        //knockback vertically
        delta.x = 0;
        delta.y = (delta.y > 0) ? 1 : -1;
      }

      knockbackVel = delta * knockbackSpeed;
      rigid.velocity = knockbackVel;

      knockback = true;
      knockbackDone = Time.time + knockbackDuration;
      anim.speed = 0;
    }
  }

  void Die()
  {
    GameObject go;
    if(guaranteedItemDrop != null)
    {
      go = Instantiate<GameObject>(guaranteedItemDrop);
      go.transform.position = transform.position;
    }
    else if (randomItemDrops.Length > 0)
    {
      int n = Random.Range(0, randomItemDrops.Length);
      GameObject prefab = randomItemDrops[n];
      if(prefab != null)
      {
        go = Instantiate<GameObject>(prefab);
        go.transform.position = transform.position;
      }
    }
    Destroy(gameObject);
  }
}
