using UnityEngine;

public class ScoreIndicator : MonoBehaviour
{
    public float upSpeed = 3f;
    public float duration = 2f;
    // Start is called before the first frame update
    void Start()
    {
        GameObject.Destroy(this.gameObject, duration);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(0, 1, 0) * upSpeed * Time.deltaTime;
    }
}
