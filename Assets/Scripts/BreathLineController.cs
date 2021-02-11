using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;
public class BreathLineController : MonoBehaviour
{

    UILineRenderer LineRenderer;
    public RectTransform Cloud;

    public float UpperBoundPos = 100.0f;
    public float LowerBoundPos = -100.0f;

    private float BoundsDistance = 200.0f;

    const float FocusXCoordinate = -0.5f;

    const float canvasScale = 15.0f;

    public float breathInTime = 4.0f;
    public float breathOutTime = 4.0f;
    public float breathInHoldTime = 4.0f;
    public float breathOutHoldTime = 4.0f;

    private float RoundTime = 0.0f;

    public float pointsPerSecond = 25.0f;

    private float[] BreathingIntervals = { 4.0f, 4.0f, 4.0f, 4.0f };
    private float[] BreathingIntervalsLowerBound = { 0.0f, 4.0f, 8.0f, 12.0f };

    //public float cloudXPosition = ;

    Vector2 StartPoint = new Vector2(FocusXCoordinate * canvasScale, canvasScale * 1.0f);

    float speed = 1;

    const float widthPerSecondScaled = 100.0f;
    const float widthPerSecondUnscaled = 1.0f;

    float holdingBreathInWidth = 4f * 1;
    float holdingBreathOutWidth = 4f * 1;

    float BreathingInWidth = 4f * widthPerSecondUnscaled * canvasScale;
    float BreathingOutWidth = 4f * widthPerSecondUnscaled * canvasScale;

    public bool started = false;

    void SetUpIntervals()
    {
        BreathingIntervals = new float[] { breathInTime, breathInHoldTime, breathOutTime, breathOutHoldTime };
        BreathingIntervalsLowerBound = new float[] { 0.0f,
            BreathingIntervals[0],
            BreathingIntervals[0]+ BreathingIntervals[1],
            BreathingIntervals[0] + BreathingIntervals[1]+ BreathingIntervals[2] };
    }

    void SpawnPointsAtStart()
    {
        float newPointX = StartPoint.x;
        var pointlist = new List<Vector2>(LineRenderer.Points);
        for (int i = 0; i < 50; i++)
        {
            pointlist.Add(new Vector2(newPointX, LowerBoundPos));
            newPointX += BreathingInWidth;
            pointlist.Add(new Vector2(newPointX, UpperBoundPos));
            newPointX += holdingBreathInWidth;
            pointlist.Add(new Vector2(newPointX, UpperBoundPos));
            newPointX += BreathingOutWidth;
            pointlist.Add(new Vector2(newPointX, LowerBoundPos));
            newPointX += holdingBreathOutWidth;
        }

        LineRenderer.Points = pointlist.ToArray();
    }

    void Start()
    {
        GetComponent<CanvasRenderer>().SetAlpha(0);

        BoundsDistance = UpperBoundPos - LowerBoundPos;
        speed = pointsPerSecond;
        holdingBreathInWidth = breathInHoldTime * pointsPerSecond;
        holdingBreathOutWidth = breathOutHoldTime * pointsPerSecond;

        BreathingInWidth = breathInTime * pointsPerSecond;
        BreathingOutWidth = breathOutTime * pointsPerSecond;

        RoundTime = breathInTime + breathOutTime + breathInHoldTime + breathOutHoldTime;
        SetUpIntervals();
        LineRenderer = GetComponent<UILineRenderer>();
        SpawnPointsAtStart();
        //Cloud.position = new Vector2(StartPoint.x, UpperBoundPos);
    }

    float timePassed = 0.0f;
    float distancePassed = 0.0f;

    void SetCloudPosition(float h)
    {
        RectTransform rect = Cloud.GetComponent<RectTransform>();
        rect.localPosition = new Vector2(rect.localPosition.x, h);
    }

    void SwapPosition()
    {
        RectTransform rect = Cloud.GetComponent<RectTransform>();
        rect.localPosition = new Vector2(rect.localPosition.x, -rect.localPosition.y);
    }
    void UpdateCloudPosition()
    {
        float temp = timePassed % RoundTime;

        float[] BoundPoses = new float[] { LowerBoundPos, UpperBoundPos, UpperBoundPos, LowerBoundPos  };

        float[] BoundsDistances = new float[] { BoundsDistance, 0, -BoundsDistance, 0 };

        for (int i = 3; i > -1; i--)
        {
            if (temp > BreathingIntervalsLowerBound[i])
            {
                float h = BoundPoses[i] + BoundsDistances[i] * (temp - BreathingIntervalsLowerBound[i]) / BreathingIntervals[i];

                SetCloudPosition(h);
                break;
            }
        }
        /*
        if (temp > BreathingIntervalsLowerBound[3])
        {
            float h = LowerBoundPos + BoundsDistance * (temp - BreathingIntervalsLowerBound[3]) / BreathingIntervals[3];

            SetCloudPosition(h);
        }
        else if (temp > BreathingIntervalsLowerBound[2])
        {
            SetCloudPosition(LowerBoundPos);
        }
        else if (temp > BreathingIntervalsLowerBound[1])
        {
            float h = UpperBoundPos + (-BoundsDistance) * (temp - BreathingIntervalsLowerBound[1]) / BreathingIntervals[1];

            SetCloudPosition(h);
        }
        else
        {
            SetCloudPosition(UpperBoundPos);
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        if (!started)
        {
            return;
        }

        float deltaTime = Time.deltaTime;
        float distance = deltaTime * speed;

        //var pointlist = new List<Vector2>(LineRenderer.Points);
        /*for (int i=0;i< LineRenderer.Points.Length;i++)
        {
            LineRenderer.Points[i].x += distance;
        }*/
        var transform = this.GetComponent<RectTransform>();
        transform.localPosition = new Vector2(transform.localPosition.x - distance, transform.localPosition.y);
        //Cloud.position = new Vector2(Cloud.position.x-distance, Cloud.position.y);
        //LineRenderer.SetAllDirty();
        distancePassed += distance;
        timePassed += deltaTime;
        UpdateCloudPosition();
    }

    public void Launch()
    {
        started = true;
        GetComponent<CanvasRenderer>().SetAlpha(1);
    }
}
