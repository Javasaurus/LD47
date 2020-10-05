using System.Collections.Generic;
using UnityEngine;

public class LoopManager : MonoBehaviour
{
    public AudioClip empty;

    public List<Loopable> targets = new List<Loopable>();

    public Loop LoopPrefab;
    private Loop LoopInstance;
    private bool mouseIsDown;

    private AudioSource audioSource;
    void Awake()
    {
        Application.runInBackground = true;
        audioSource = GetComponent<AudioSource>();
    }
    // Update is called once per frame
    void Update()
    {
        if (!mouseIsDown && Input.GetMouseButtonDown(0))
        {
            mouseIsDown = true;
            LoopInstance = GameObject.Instantiate(LoopPrefab);
            LoopInstance.loopManager = this;
            LoopInstance.loopState = Loop.LoopState.DRAWING;
        }
        else if (mouseIsDown && Input.GetMouseButtonUp(0))
        {
            mouseIsDown = false;
            if (LoopInstance != null)
            {
                LoopInstance.loopState = Loop.LoopState.DRAWN;
            }
            LoopInstance = null;
            targets.Clear();
        }

        targets.RemoveAll(f => f == null || f.expired);

    }

    public void PlayEmpty()
    {
        audioSource.PlayOneShot(empty);
    }

}
