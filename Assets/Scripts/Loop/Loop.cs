using System.Collections.Generic;
using UnityEngine;
using static LoopUtils;


[RequireComponent(typeof(LineRenderer))]
public class Loop : MonoBehaviour
{
    public enum LoopState
    {
        DRAWING, DRAWN, TERMINATED
    }

    public LoopState loopState = LoopState.DRAWING;

    public LayerMask loopables;
    public float maxLoopRadius = 1f;
    public float loopDuration = 5f;
    private float loopTimer;
    [Range(0f, 1f)]
    public float hitThreshold = .5f;
    public int maxSegments = 50;

    public float loopDetectionThreshold = 1f;
    public float mouseDetectionInterval = .3f;
    private float mouseDetectionTimer;

    [HideInInspector]
    public LoopManager loopManager;
    public List<Vector2> points;

    private Dictionary<Loopable, float> containedLoopables;
    private Winding winding;
    private List<LoopSegment> segments;
    private LineRenderer lr;


    private Vector2 currMousePosition;
    private Vector2 prevMousePosition;
    private float minDistanceMoved = .05f;

    public float TimeTillReload = 5f;

    void Start()
    {
        lr = GetComponent<LineRenderer>();
        points = new List<Vector2>();
        segments = new List<LoopSegment>();
        containedLoopables = new Dictionary<Loopable, float>();
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            loopState = LoopState.TERMINATED;
        }

        switch (loopState)
        {
            case LoopState.DRAWING:
                HandleDrawing();
                break;
            case LoopState.TERMINATED:
                HandleTerminate();
                break;
        }
        HandleEffect();
    }

    void HandleDrawing()
    {
        TimeTillReload -= Time.deltaTime;
        if (TimeTillReload <= 0)
        {
            loopState = LoopState.TERMINATED;
            //Play The "empty gun" sound
            loopManager.PlayEmpty();
            return;
        }

        loopTimer = Time.time + loopDuration;
        currMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Vector2.Distance(currMousePosition, prevMousePosition) > minDistanceMoved)
        {
            segments = CalculateSegments();
            winding = LoopUtils.CalculateWinding(segments);

            if (Time.time > mouseDetectionTimer)
            {
                mouseDetectionTimer = Time.time + mouseDetectionInterval;
                if (points.Count >= maxSegments && points.Count > 0)
                {
                    points.RemoveAt(0);
                }
                points.Add(currMousePosition);
            }
            prevMousePosition = currMousePosition;
        }



    }

    void HandleEffect()
    {

        segments = CalculateSegments();
        winding = LoopUtils.CalculateWinding(segments);

        if (Time.time > loopTimer)
        {
            if (points.Count > 0)
            {
                points.RemoveAt(0);
            }
            if (points.Count == 0)
            {
                loopState = LoopState.TERMINATED;
            }
        }
    }

    void HandleTerminate()
    {
        foreach (Loopable loopable in containedLoopables.Keys)
        {
            loopManager.targets.Remove(loopable);
        }
        GameObject.Destroy(this.gameObject);
    }

    void LateUpdate()
    {
        CheckLoopables();
        DrawLoop();
    }

    private List<LoopSegment> CalculateSegments()
    {
        segments = new List<LoopSegment>();
        for (int i = 1; i < points.Count; i++)
        {
            segments.Add(new LoopSegment
            {
                start = points[i - 1],
                end = points[i]
            });
        }
        return segments;
    }

    private void CheckLoopables()
    {
        loopManager.targets.Clear();

        foreach (Loopable loopable in containedLoopables.Keys)
        {
            loopable.isActive = false;
        }
        containedLoopables.Clear();

        foreach (LoopSegment segment in segments)
        {
            Vector2 midPoint = (segment.start + segment.end) / 2;
            Vector2 dir = LoopUtils.CrossProduct(segment.start, segment.end, winding);
            RaycastHit2D hit = Physics2D.Raycast(midPoint, dir, maxLoopRadius, loopables);

            if (hit.collider != null)
            {
                Debug.DrawLine(midPoint, hit.point, Color.green);
                Loopable hitLoopable = hit.collider.GetComponent<Loopable>();
                if (containedLoopables.ContainsKey(hitLoopable))
                {
                    containedLoopables[hitLoopable]++;
                }
                else
                {
                    containedLoopables.Add(hitLoopable, 1);
                }
            }
            else
            {
                Debug.DrawLine(midPoint, midPoint + dir.normalized * maxLoopRadius, Color.red);
            }
        }

        foreach (Loopable loopable in containedLoopables.Keys)
        {
            float currentRatio = containedLoopables[loopable] / segments.Count;
            if (currentRatio >= hitThreshold)
            {
                loopManager.targets.Add(loopable);
                loopable.onLoopActivate();
            }
        }

    }

    void DrawLoop()
    {
        if (segments.Count == 0) return;
        lr.positionCount = segments.Count;

        Vector3[] positions = new Vector3[lr.positionCount];


        for (int i = 0; i < lr.positionCount - 1; i++)
        {
            positions[i] = segments[i].start;
            positions[i + 1] = segments[i].end;
        }

        lr.positionCount = positions.Length;
        lr.SetPositions(positions);

    }

    void FinalizeLoop()
    {
        Vector2 centerPoint = Vector2.zero;
        foreach (Vector2 point in points)
        {
            centerPoint += point;
        }
        centerPoint = centerPoint / points.Count;
        float averageRadius = 0;
        foreach (Vector2 point in points)
        {
            averageRadius += Vector2.Distance(centerPoint, point);
        }
        averageRadius = averageRadius / points.Count;
        points.Clear();
        float deltaAngle = Mathf.Deg2Rad * (360f / maxSegments);
        for (int i = 0; i < maxSegments; i++)
        {
            points.Add(centerPoint + new Vector2(Mathf.Cos(i * deltaAngle), Mathf.Sin(i * deltaAngle)).normalized * averageRadius);
        }
        points.Add(points[0]);
    }

    bool isCrossing( Vector2 newPoint )
    {
        foreach (Vector2 point in points)
        {
            if (Vector2.Distance(newPoint, point) <= loopDetectionThreshold)
            {
                return true;
            }
        }
        return false;
    }

}
