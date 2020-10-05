using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MachineGun : MonoBehaviour
{
    public LoopManager loopManager;
    public Projectile projectilePrefab;
    public Transform top;
    public Transform bot;
    public GameObject flash;

    public float rpm = 120;
    private float nextShotTimer;
    private AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

        flash.SetActive(Input.GetMouseButton(0) && loopManager.targets.Count > 0);



        if (Time.time > nextShotTimer && Input.GetMouseButton(0))
        {
            nextShotTimer = Time.time + (60f / rpm);
            if (loopManager.targets.Count > 0)
            {
                Loopable target = loopManager.targets[0];

                if (audioSource != null)
                {
                    audioSource.pitch = Random.Range(.9f, 1.1f);
                    audioSource.PlayOneShot(audioSource.clip);
                }

                var offset = 0f;
                Vector2 pos = new Vector2(target.transform.position.x, target.transform.position.y);
                bool facingRight = pos.x >= transform.position.x;
                Vector2 direction = (pos - (Vector2)transform.position) * (facingRight ? 1 : -1);
                direction.Normalize();
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                angle = Mathf.Clamp(angle, -50, 50);

                transform.localScale = new Vector3(facingRight ? 1 : -1, 1, 1);

                top.transform.rotation = Quaternion.Euler(Vector3.forward * (angle + offset));

                // foreach (Loopable target in possibleTargets)
                {
                    Projectile instance = GameObject.Instantiate(projectilePrefab);
                    instance.Launch(
                        (target.transform.position - transform.position).normalized,
                        transform.position,
                        target.transform.position);
                    target.OnHit();

                }

            }
        }

    }



}