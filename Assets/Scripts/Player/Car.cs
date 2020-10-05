using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public GameObject liveCar;
    public GameObject brokenCar;
    public Color redColor;
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

    public float blinkSpeed = 5f;

    private float tick = 1f;
    private float tickTimer;

    private SpriteRenderer carRenderer;
    private void Start()
    {
        carRenderer = liveCar.GetComponent<SpriteRenderer>();
    }

    public void Update()
    {
        if (ScoreManager.instance.damagedFences > 0)
        {
            carRenderer.color = Color.Lerp(Color.white, redColor, Mathf.PingPong(Time.time * blinkSpeed, 1));
        }
    }

    public void OnCollisionStay2D( Collision2D collision )
    {
        if (ScoreManager.instance.damagedFences <= 0) return;

        if (Time.time > tickTimer)
        {

            Loopable loopable = collision.collider.GetComponent<Loopable>();
            if (loopable != null && !loopable.expired)
            {
                _currHealth--;

                if (_currHealth <= 0)
                {
                    liveCar.SetActive(false);
                    brokenCar.SetActive(true);
                    ScoreManager.instance.GameOver();
                }
                else
                {
                    liveCar.SetActive(true);
                    brokenCar.SetActive(false);
                }
            }
            tickTimer = Time.time + tick;
        }
    }
}
