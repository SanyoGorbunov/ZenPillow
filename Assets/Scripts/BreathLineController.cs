using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;
using UnityEngine.EventSystems;
using Input = InputWrapper.Input;

public class BreathLineController : MonoBehaviour
{
    public DisplayTimerController displayTimerController;

    UILineRenderer LineRenderer;
    public RectTransform Cloud;
    public GameObject drop;

    public bool useDrops = false;

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

    private bool _canTap = false;
    private int _tapDropId = -1;
    public float dropTouchRadius = 1.0f;

    int dropsCountPerPhase = 3;

    private struct Drop
    {
        public Drop(GameObject obj, float pos)
        {
            DropObject = obj;
            XPositionOnLine = pos;
        }
        public GameObject DropObject;
        public float XPositionOnLine;
    }

    private int LastNearDrop = 0;
    public float dropRange = 15.0f;
    private float dropHalfRange = 7.5f;

    private Drop[] dropsLinePos;
    private Interval[] dropsLineInterval;

    private struct Interval
    {
        public Interval(float left, float right, int idInPhase)
        {
            this.left = left;
            this.right = right;
            this.idInPhase = idInPhase;
        }

        public float left, right;

        public int idInPhase;
    }


    void SetUpIntervals()
    {
        BreathingIntervals = new float[] { breathInTime, breathInHoldTime, breathOutTime, breathOutHoldTime };
        BreathingIntervalsLowerBound = new float[] { 0.0f,
            BreathingIntervals[0],
            BreathingIntervals[0]+ BreathingIntervals[1],
            BreathingIntervals[0] + BreathingIntervals[1]+ BreathingIntervals[2] };
    }


    void CreateDropsLine(int count, int startId, float startXPos, float endXPos, float YPos)
    {
        float PhaseLength = (endXPos - startXPos);

        float interval = PhaseLength / (count + 1);

        for (int i = 0; i < count; i++)
        {
            float XPos = startXPos + (interval * (i + 1));

            CreateNewDrop(startId+i, XPos, YPos);
            dropsLineInterval[startId + i] = new Interval(((interval * (i + 1)) - dropHalfRange) / PhaseLength, ((interval * (i + 1)) + dropHalfRange) / PhaseLength, i);
        }
    }

    void CreateNewDrop(int id, float XPos, float YPos)
    {
        GameObject temp_drop = Instantiate<GameObject>(drop);
        temp_drop.transform.SetParent(gameObject.transform, false);

        var transform = temp_drop.GetComponent<RectTransform>();

        transform.localPosition = new Vector2(XPos, YPos);

        dropsLinePos[id] = new Drop(temp_drop, XPos);
        temp_drop.SetActive(isVisible && started);
    }

    void SpawnPointsAtStart()
    {

        dropsLinePos = new Drop[dropsCountPerPhase*2*50];
        dropsLineInterval = new Interval[dropsCountPerPhase * 2 * 50];

        float newPointX = StartPoint.x;
        var pointlist = new List<Vector2>(LineRenderer.Points);
        for (int i = 0; i < 50; i++)
        {
            pointlist.Add(new Vector2(newPointX, LowerBoundPos));
            newPointX += BreathingInWidth;
            pointlist.Add(new Vector2(newPointX, UpperBoundPos));
            newPointX += holdingBreathInWidth;

            if (useDrops)
            {
                CreateDropsLine(dropsCountPerPhase, 2 * dropsCountPerPhase * i, newPointX - holdingBreathInWidth, newPointX, UpperBoundPos);
            }

            pointlist.Add(new Vector2(newPointX, UpperBoundPos));
            newPointX += BreathingOutWidth;
            pointlist.Add(new Vector2(newPointX, LowerBoundPos));
            newPointX += holdingBreathOutWidth;

            if (useDrops)
            {
                CreateDropsLine(dropsCountPerPhase, 2 * dropsCountPerPhase * i + dropsCountPerPhase, newPointX - holdingBreathOutWidth, newPointX, LowerBoundPos);
            }
        }

        LineRenderer.Points = pointlist.ToArray();
    }

    Utils.BreathParams params1 = null;

    void Start()
    {
        params1 = GameStateManager.Instance.getActiveBreathParams();

        if (params1 != null)
        {
            this.breathInTime = params1.breathInTime;
            this.breathOutTime = params1.breathOutTime;
            this.breathInHoldTime = params1.breathInHoldTime;
            this.breathOutHoldTime = params1.breathOutHoldTime;
        }

        breathInTime = breathInTime >= 2.0f ? breathInTime : 2.0f;
        breathOutTime = breathOutTime >= 2.0f ? breathOutTime : 2.0f;

        dropHalfRange = dropRange / 2;
        GetComponent<CanvasRenderer>().SetAlpha((isVisible && started )? 1:0);

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
        Cloud.GetComponent<CanvasRenderer>().SetAlpha((isVisible && started) ? 1 : 0);
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

    float PhaseAlpha = 0.0f;

    void UpdateCloudPosition()
    {
        float temp = timePassed % RoundTime;

        int RoundId = (int)((timePassed - temp) / RoundTime);

        float[] BoundPoses = new float[] { LowerBoundPos, UpperBoundPos, UpperBoundPos, LowerBoundPos  };

        float[] BoundsDistances = new float[] { BoundsDistance, 0, -BoundsDistance, 0 };
        
        for (int i = 3; i > -1; i--)
        {
            if (temp > BreathingIntervalsLowerBound[i])
            {
                float h = BoundPoses[i];
                if (useDrops && BoundsDistances[i] == 0)
                {
                    PhaseAlpha = (temp - BreathingIntervalsLowerBound[i]) / BreathingIntervals[i];

                    if (i == 1)
                    {
                        LastNearDrop = RoundId * 2 * dropsCountPerPhase;
                    }
                    else if(i==3)
                    {
                        LastNearDrop = RoundId * 2 * dropsCountPerPhase + dropsCountPerPhase;
                    }

                    _canTap = true;
                }
                else
                {
                    h += BoundsDistances[i] * (temp - BreathingIntervalsLowerBound[i]) / BreathingIntervals[i];
                    _canTap = false;
                }

                SetCloudPosition(h);
                break;
            }
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        if (!started || displayTimerController.isPaused())
        {
            return;
        }

        float deltaTime = Time.deltaTime;
        float distance = deltaTime * speed;
        var transform = this.GetComponent<RectTransform>();
        transform.localPosition = new Vector2(transform.localPosition.x - distance, transform.localPosition.y);
        distancePassed += distance;
        timePassed += deltaTime;
        UpdateCloudPosition();
        HandleInput();
    }

    public void Launch()
    {
        started = true;
    }

    private bool isVisible = false;

    public void SetVisibility(bool isVisible)
    {
        GetComponent<CanvasRenderer>().SetAlpha(isVisible? 1: 0);
        Cloud.gameObject.GetComponent<CanvasRenderer>().SetAlpha(isVisible ? 1 : 0);
        if (useDrops)
        {
            SetDropsVisibility(isVisible);
        }
        this.isVisible = isVisible;
    }

    private void SetDropsVisibility(bool isVisible)
    {
        if(dropsLinePos!=null)
        { 
            for (int i = 0; i < dropsLinePos.Length; i++)
            {
                dropsLinePos[i].DropObject.SetActive(isVisible);
            }
        }
    }

    void HandleInput()
    {
        if (_canTap && Input.touchCount > 0 && EventSystem.current.currentSelectedGameObject == null)
        {
            var touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:

                    for (int j = LastNearDrop; j < LastNearDrop + dropsCountPerPhase; j++)
                    {
                        if (PhaseAlpha <= dropsLineInterval[j].right)
                        {
                            if (PhaseAlpha >= dropsLineInterval[j].left)
                            {
                                _tapDropId = j;
                            }
                            else
                            {
                                _tapDropId = -1;
                            }
                            break;
                        }
                    }

                    if (_tapDropId > -1)
                    {
                        dropsLinePos[_tapDropId].DropObject.SetActive(false);
                    }

                    break;
            }
        }
    }
}
