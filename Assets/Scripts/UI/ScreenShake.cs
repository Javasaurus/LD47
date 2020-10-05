using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    // How long the object should shake for.
    public float shakeDuration = 0f;
    public float shakeIncrement = .05f;
    private float shakeCounter = 0;
    private float maxShakeCounter = 100;
    public float maxShakeAmount = .7f;
    public float decreaseFactor = 1.0f;

    Vector3 originalPos;

    public static ScreenShake instance;

    private void Awake()
    {
        instance = this;
    }

    void OnEnable()
    {
        originalPos = transform.localPosition;
    }

    void Update()
    {
        if (shakeDuration > 0)
        {
            float shakeAmount = Mathf.Lerp(0, maxShakeAmount, (Mathf.Min(shakeCounter, maxShakeCounter) / maxShakeCounter));
            transform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;

            shakeDuration -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            shakeDuration = 0f;
            transform.localPosition = originalPos;
        }
        if (Input.GetMouseButtonUp(0))
        {
            shakeCounter = 0;
        }
    }

    public void AddShake( int amount = 1 )
    {
        shakeCounter++;
        shakeDuration += (amount * shakeIncrement);
    }
}
