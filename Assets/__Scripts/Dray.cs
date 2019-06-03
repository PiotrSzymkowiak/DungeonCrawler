using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dray : MonoBehaviour, IFacingMover, IKeyMaster
{
  public enum eMode { idle, move, attack, transition, knockback }

  [Header("Defined in inspection panel")]
  public float speed = 5;
  public float attackDuration = 0.25f;
  public float attackDelay = 0.5f;
  public float transitionDelay = 0.5f;

  public int maxHealth = 10;
  public float knockbackSpeed = 10;
  public float knockbackDuration = 0.25f;
  public float invincibleDuration = 0.5f;

  [Header("Defined dynamically")]
  public int dirHeld = -1;
  public int facing = 1;
  public eMode mode = eMode.idle;
  public int numKeys = 0;
  public bool invincible = false;
  public bool hasGrappler = false;
  public Vector3 lastSafeLoc;
  public int lastSafeFacing;

  [SerializeField]
  private int _health;

  public int Health { get => _health; set => _health = value; }

  private float timeAtkDone = 0;
  private float timeAtkNext = 0;
  private float transitionDone = 0;
  private Vector2 transitionPos;
  private float knockbackDone = 0;
  private float invincibleDone = 0;
  private Vector3 knockbackVel;

  private SpriteRenderer sRend;
  private Rigidbody2D rigid;
  private Animator anim;

  private Vector3[] directions = new Vector3[]
  {
    Vector3.right,
    Vector3.up,
    Vector3.left,
    Vector3.down
  };

  private KeyCode[] keys = new KeyCode[]
  {
    KeyCode.RightArrow,
    KeyCode.UpArrow,
    KeyCode.LeftArrow,
    KeyCode.DownArrow
  };

  //IFacingMover impl
  public bool moving { get => mode == eMode.move; }

  public int GetFacing() => facing; //IFacingMover & IKeyMaster impl
  public float GetSpeed() => speed;

  //IKeyMaster impl
  public int keyCount { get => numKeys; set => numKeys = value; }

  void Awake()
  {
    sRend = GetComponent<SpriteRenderer>();
    rigid = GetComponent<Rigidbody2D>();
    anim = GetComponent<Animator>();   
    Health = maxHealth;
    lastSafeLoc = transform.position;
    lastSafeFacing = facing;
  }

  void Update()
  {
    if (invincible && Time.time > invincibleDone) invincible = false;
    sRend.color = invincible ? Color.red : Color.white;
    if(mode == eMode.knockback)
    {
      rigid.velocity = knockbackVel;
      if (Time.time < knockbackDone) return;
    }

    dirHeld = -1;
    for(int i = 0; i < 4; i++)
    {
      if (Input.GetKey(keys[i])) dirHeld = i;
    }

    //Attack key pressed
    if(Input.GetKeyDown(KeyCode.Z) && Time.time >= timeAtkNext)
    {
      mode = eMode.attack;
      timeAtkDone = Time.time + attackDuration;
      timeAtkNext = Time.time + attackDelay;
    }

    //Manage attack after its finish
    if(Time.time >= timeAtkDone)
    {
      mode = eMode.idle;
    }

    //Choose right state in case you dont attack
    if(mode != eMode.attack)
    {
      if(dirHeld == -1)
      {
        mode = eMode.idle;
      }
      else
      {
        facing = dirHeld;
        mode = eMode.move;
      }
    }

    Vector3 vel = Vector3.zero;
    switch (mode)
    {
      case eMode.attack:
        anim.CrossFade("Dray_Attack_" + facing, 0);
        anim.speed = 0;
        break;
      case eMode.idle:
        anim.CrossFade("Dray_Walk_" + facing, 0);
        anim.speed = 0;
        break;
      case eMode.move:
        vel = directions[dirHeld];
        anim.CrossFade("Dray_Walk_" + facing, 0);
        anim.speed = 1;
        break;
    }

    rigid.velocity = vel * speed;
  }

  void OnCollisionEnter2D(Collision2D coll)
  {
    if (invincible) return;
    DamageEffect dEf = coll.gameObject.GetComponent<DamageEffect>();
    if (dEf == null) return;

    Health -= dEf.damage;
    invincible = true;
    invincibleDone = Time.time + invincibleDuration;

    if(dEf.knockback)
    {
      Vector3 delta = transform.position - coll.transform.position;
      if(Mathf.Abs(delta.x) >= Mathf.Abs(delta.y))
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

      mode = eMode.knockback;
      knockbackDone = Time.time + knockbackDuration;
    }
  }

  void OnTriggerEnter2D(Collider2D colld)
  {
    PickUp pup = colld.GetComponent<PickUp>();
    if (pup == null) return;
    switch(pup.itemType)
    {
      case PickUp.eType.health:
        Health = Mathf.Min(Health + 2, maxHealth);
        break;
      case PickUp.eType.key:
        keyCount++;
        break;
      case PickUp.eType.grappler:
        hasGrappler = true;
        break;
    }

    Destroy(colld.gameObject);
  }

  public void ResetInRoom(int healthLoss = 0)
  {
    transform.position = lastSafeLoc;
    facing = lastSafeFacing;
    Health -= healthLoss;

    invincible = true;
    invincibleDone = Time.time + invincibleDuration;
  }
}
