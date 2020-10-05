using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    public static int dmgMultiplier = 1;
    public enum EventType
    {
        NONE, REPAIR, BOMB, SCOREX2, DAMAGEX2, SCOREX4, DAMAGEX4
    }
    public GameObject costTransform;
    public Animator ButtonAnimator;
    public TextMeshProUGUI textField;
    public TextMeshProUGUI scoreMField;
    public TextMeshProUGUI dmgMField;
    public float eventDelay = 60;
    private float eventTimer;
    private EventType presentedEvent;
    private AudioSource source;

    public AudioClip purchaseSound;

    void Start()
    {
        source = GetComponent<AudioSource>();
        //  eventTimer = Time.time + eventDelay;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > eventTimer)
        {
            PresentEvent(GetRandomEvent());
            eventTimer = Time.time + eventDelay;
        }
        scoreMField.text = ScoreManager.instance.multiplier > 1 ? "SCORE x " + ScoreManager.instance.multiplier : "";
        dmgMField.text = dmgMultiplier > 1 ? "DMG x " + dmgMultiplier : "";
    }

    public void PresentEvent( EventType type )
    {
        if (type != EventType.NONE)
        {
            ButtonAnimator.gameObject.SetActive(true);
            presentedEvent = type;
            textField.text = type + "(" + GetPrice(type) / 1000 + "K)";
            ButtonAnimator.SetTrigger("DoEvent");
        }
    }

    public void DoEvent( EventType type )
    {
        switch (type)
        {
            case EventType.REPAIR:
                StartCoroutine(DoFenceHealing());
                break;
            case EventType.BOMB:
                ScoreManager.instance.explosion.Explode();
                List<BasicEnemy> enemies = GameObject.FindObjectOfType<EnemySpawner>().enemies;
                foreach (BasicEnemy enemy in enemies)
                {
                    enemy.OnHit(enemy.currHP);
                }
                break;
            case EventType.SCOREX2:
                StartCoroutine(DoScoreMultiplier(2));
                break;
            case EventType.DAMAGEX2:
                StartCoroutine(DoDamageMultiplier(2));
                break;
            case EventType.SCOREX4:
                StartCoroutine(DoScoreMultiplier(4));
                break;
            case EventType.DAMAGEX4:
                StartCoroutine(DoDamageMultiplier(4));
                break;
            default:
                break;
        }
    }

    private IEnumerator DoDamageMultiplier( int multiplier )
    {
        dmgMultiplier *= multiplier;
        yield return new WaitForSeconds(60);
        dmgMultiplier /= multiplier;

    }

    private IEnumerator DoScoreMultiplier( int multiplier )
    {
        ScoreManager.instance.multiplier *= multiplier;
        yield return new WaitForSeconds(60);
        ScoreManager.instance.multiplier /= multiplier;
    }

    private IEnumerator DoFenceHealing()
    {
        List<Fence> fences = ScoreManager.instance.fences;
        int healingAmount = (int)fences[0].maxHealth / 10;
        for (int i = 0; i < 10; i++)
        {
            foreach (Fence fence in fences)
            {
                fence.Repair(healingAmount);
            }
            yield return new WaitForSeconds(1);
        }
    }



    public void DoPresentedEvent()
    {
        int price = GetPrice(presentedEvent);
        if (price <= ScoreManager.instance.score)
        {
            source.PlayOneShot(purchaseSound);
            DoEvent(presentedEvent);
            ButtonAnimator.SetTrigger("CancelEvent");
            ButtonAnimator.gameObject.SetActive(false);
        }
        ScoreManager.instance.score -= price;
        ScoreManager.instance.AddMessage("-" + price / 1000 + "K", Color.red, Vector3.zero, costTransform.transform);
    }

    public int GetPrice( EventType type )
    {
        switch (type)
        {
            case EventType.REPAIR:
                return 1000;
            case EventType.BOMB:
                return 3000;
            case EventType.SCOREX2:
                return 5000;
            case EventType.DAMAGEX2:
                return 5000;
            case EventType.SCOREX4:
                return 20000;
            case EventType.DAMAGEX4:
                return 20000;
            default:
                return 0;
        }
    }

    public EventType GetRandomEvent()
    {
        if (ScoreManager.instance.damagedFences > 0)
        {
            return EventType.REPAIR;
        }

        Array values = Enum.GetValues(typeof(EventType));
        List<EventType> types = new List<EventType>();
        foreach (EventType type in values)
        {
            int price = GetPrice(type);
            if (ScoreManager.instance.score >= price)
            {
                int weight = 1 + (100 - (int)(price / 1000));
                for (int i = 0; i < weight; i++)
                {
                    types.Add(type);
                }
            }
        }


        return types.Count > 0 ? types[UnityEngine.Random.Range(0, types.Count)] : EventType.NONE;
    }
}
