using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieGlow : MonoBehaviour
{
    public float frequency = 5;
    public float amplitude = .03f;

    private float initialZ;

    void Start()
    {
        initialZ = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, initialZ + Mathf.Sin(frequency * Time.time) * amplitude);
    }
}
