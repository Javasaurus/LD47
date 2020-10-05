using UnityEngine;

public class BasicEnemy : Loopable
{
    public enum EnemyState
    {
        IDLE, CHASE, FLEE, ROAM, HURT, DEAD
    }

    public EnemyState currentState = EnemyState.IDLE;

    public ZombieMoan moan;
    public GameObject Graphics;
    public GameObject DeathEffect;
    public ParticleSystem hitEffect;
    public Transform targetTransform;
    private Rigidbody2D rb;
    public int hp;
    public float speed;
    public int currHP;


    private float actionTimer;

    public bool facingRight;

    [HideInInspector]
    public Vector3 startPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currHP = hp;
    }


    void Update()
    {
        if (expired) return;
        UpdateGraphics();
        UpdateAI();
        HandleState();
    }

    private void UpdateGraphics()
    {
        transform.localScale = new Vector3(facingRight ? 1 : -1, 1, 1);
    }
    private void UpdateAI()
    {

    }

    protected virtual EnemyState EvaluateState()
    {
        if (currHP <= 0)
        {
            return EnemyState.DEAD;
        }

        if (ScoreManager.GAMEOVER)
        {
            if (currentState != EnemyState.FLEE)
            {
                actionTimer = Time.time + Random.Range(0f, 3f);
            }

            return EnemyState.FLEE;
        }

        return currentState;
    }
    protected void HandleState()
    {
        currentState = EvaluateState();
        switch (currentState)
        {
            case EnemyState.IDLE:
                HandleIdle();
                break;
            case EnemyState.CHASE:
                HandleChase();
                break;
            case EnemyState.FLEE:
                HandleFlee();
                break;
            case EnemyState.ROAM:
                HandleRoam();
                break;
            case EnemyState.HURT:
                HandleHurt();
                break;
            case EnemyState.DEAD:
                HandleDead();
                break;
        }
    }

    protected virtual void HandleIdle()
    {

    }
    protected virtual void HandleChase()
    {
        if (targetTransform == null) return;
        //   rb.MovePosition(Vector3.Lerp(transform.position, targetTransform.position, speed * Time.deltaTime));
        Vector3 dir = (targetTransform.position - transform.position).normalized;
        rb.velocity = dir * speed / 4;
        //  rb.MovePosition(Vector3.Lerp(transform.position, transform.position + dir, speed / 4 * Time.deltaTime));
        facingRight = targetTransform.position.x > transform.position.x;
    }
    protected virtual void HandleFlee()
    {
        if (Time.time > actionTimer)
        {
            Vector3 dir = (startPosition - transform.position).normalized;
            rb.velocity = dir * speed;
            //rb.MovePosition(Vector3.Lerp(transform.position, transform.position + dir, speed / 4 * Time.deltaTime));
            if (Vector3.Distance(transform.position, startPosition) > .1f)
            {
                facingRight = startPosition.x > transform.position.x;
            }
            else
            {
                hitEffect.Play();
                HandleDead();
            }
        }
    }
    protected virtual void HandleRoam()
    {

    }
    protected virtual void HandleHurt()
    {

    }
    protected virtual void HandleDead()
    {
        rb.velocity = Vector3.zero;
        hitEffect.transform.parent = null;
        DeathEffect.transform.parent = null;
        DeathEffect.SetActive(true);
        GameObject.Destroy(gameObject);
        GameObject.Destroy(hitEffect.gameObject, 5f);
        GameObject.Destroy(DeathEffect, 5f);
        expired = true;
    }
    protected override void OnLoopActivate()
    {

    }

    public override void OnHit( int dmg = 1 )
    {
        ScoreManager.instance.AddScore(dmg * 50, Color.white, Vector3.zero, transform);
        hitEffect.Play();
        currHP -= dmg * PowerUpManager.dmgMultiplier;
        ScreenShake.instance.AddShake();
        moan.PlayHurt();
    }

}
