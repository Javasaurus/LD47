using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Image))]
public class Explosion : MonoBehaviour
{

    private AudioSource audioSource;
    private Image image;
    private Coroutine explosionRoutine;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        image = GetComponent<Image>();
    }

    public void Explode()
    {
        if (explosionRoutine != null)
        {
            StopCoroutine(explosionRoutine);
        }
        explosionRoutine = StartCoroutine(ExplodeAsync());
    }


    private IEnumerator ExplodeAsync( float duration = .75f )
    {
        audioSource.PlayOneShot(audioSource.clip);
        float a = 1f;
        float t = 0;
        while (t <= duration)
        {
            t += Time.deltaTime;
            a = Mathf.Lerp(1f, 0f, t / duration);
            image.color = new Color(image.color.r, image.color.g, image.color.b, a);
            yield return null;
        }
        image.color = new Color(image.color.r, image.color.g, image.color.b, a);
        explosionRoutine = null;
    }


}
