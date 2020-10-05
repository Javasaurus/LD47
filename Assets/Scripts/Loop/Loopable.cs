using UnityEngine;

public class Loopable : MonoBehaviour
{
    public bool isActive;
    public bool expired;
    public delegate void ActivateLoop();
    public ActivateLoop onLoopActivate;

    void Awake()
    {
        onLoopActivate += OnLoopActivate;
    }

    protected virtual void OnLoopActivate()
    {
        Debug.Log(gameObject.name + " was looped");
        isActive = true;
        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, Time.deltaTime / 10);
    }

    public virtual void OnHit( int dmg = 1 )
    {
        //DO SOMETHING
    }

}
