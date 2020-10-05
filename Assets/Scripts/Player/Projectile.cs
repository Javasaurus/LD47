using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Projectile : MonoBehaviour
{
    public float speed = 10;

    private LineRenderer lineRenderer;
    private Vector3[] positions;
    private Vector3 targetPosition;
    private bool launched;
    // Start is called before the first frame update
    public void Launch( Vector3 direction, Vector3 startPosition, Vector3 targetPosition )
    {
        lineRenderer = GetComponent<LineRenderer>();
        positions = new Vector3[2];
        positions[0] = startPosition;
        positions[1] = startPosition + direction * speed;
        this.targetPosition = targetPosition;
        launched = true;

        GameObject.Destroy(this.gameObject, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!launched) return;
        positions[0] = Vector3.Lerp(positions[0], targetPosition, speed * Time.deltaTime);
        positions[1] = targetPosition;
        lineRenderer.positionCount = 2;
        lineRenderer.SetPositions(positions);
    }
}
