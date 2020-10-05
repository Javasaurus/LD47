using UnityEngine;


public class Fence : MonoBehaviour
{
    public Color greenColor;
    public Color orangeColor;
    public Color redColor;

    private BoxCollider2D col;

    public float maxHealth;

    private float _currHealth;

    public float Health
    {
        get { return _currHealth; }
        set
        {
            _currHealth = value;
            float ratio = _currHealth / maxHealth;
        }
    }

    public float hpm = 10;
    private float regenTimer;
    private float regenDelay = 60;
    public float blinkSpeed = 5f;
    private SpriteRenderer sr;
    private float tick = 1f;
    private float tickTimer;
    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<BoxCollider2D>();
        sr = GetComponent<SpriteRenderer>();
        Health = maxHealth;
    }

    private void LateUpdate()
    {
        /*  if (Time.time > regenTimer)
          {
              Health += hpm;
              Health = Mathf.Clamp(Health, 0, maxHealth);
              regenTimer = Time.time + regenDelay;
          }*/
    }

    private void Update()
    {
        float ratio = _currHealth / maxHealth;
        if (ratio > .8f)
        {
            sr.color = greenColor;
        }
        else if (ratio > .4f)
        {
            sr.color = orangeColor;
        }
        else
        {
            sr.color = Color.Lerp(Color.white, redColor, Mathf.PingPong(Time.time * blinkSpeed, 1));
        }
    }

    public void OnCollisionStay2D( Collision2D collision )
    {
        if (Time.time > tickTimer)
        {

            Loopable loopable = collision.collider.GetComponent<Loopable>();
            if (loopable != null && !loopable.expired)
            {
                ScoreManager.instance.AddMessage("-1", Color.red, new Vector3(.5f, 0, 0), transform);
                _currHealth--;
                if (_currHealth <= 0)
                {
                    KillFence();
                }
            }
            tickTimer = Time.time + tick;
        }
    }

    private void KillFence()
    {
        ChadAudio.instance.PlayFenceBreach();
        this.gameObject.SetActive(false);
    }

    public void Repair( int amount )
    {
        ScoreManager.instance.AddMessage("+", Color.green, new Vector3(.5f, 0, 0), transform);
        this.gameObject.SetActive(true);
        _currHealth += amount;
        if (_currHealth > maxHealth) { _currHealth = maxHealth; }
    }

}
