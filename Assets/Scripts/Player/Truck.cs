using UnityEngine;

public class Truck : MonoBehaviour
{
    public Vector2 center = Vector2.zero;
    public MachineGun machineGun;
    public float speed;
    public Vector2 radius;

    private float currentAngle;
    private float currentAngleRad;

    void Start()
    {
        Vector2 topRightCorner = new Vector2(1, 1);
        Vector2 edgeVector = Camera.main.ViewportToWorldPoint(topRightCorner);
        radius.x = edgeVector.x - 1;
        radius.y = edgeVector.y - 1;
        center = Vector2.zero;
    }
    // Update is called once per frame
    void Update()
    {
        currentAngle += speed * Time.deltaTime;
        currentAngle %= 360;
        currentAngleRad = currentAngle * Mathf.Deg2Rad;
        Vector3 newPosition = CalculateNextPosition();
        Vector2 direction = newPosition - transform.position;
        float localAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(localAngle, Vector3.forward);
        transform.position = newPosition;
    }

    Vector3 CalculateNextPosition()
    {
        //CIRCLE
        return center + new Vector2(Mathf.Cos(currentAngleRad), Mathf.Sin(currentAngleRad)) * radius;
    }

}
