using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Input = InputWrapper.Input;
using Utils;
public class BreathingPressController : MonoBehaviour
{
    public DisplayTimerController displayTimerController;

    public InstructionsPressController instructionsPressController;

    public GameObject Cloud;
    public GameObject UICenter;

    public float pauseDuration = 6.0f;

    private bool _isInhale, _isOver;

    public bool isHorizontal = false, _isInstruction;

    public float minCloudScale = 0.3f;
    public float maxCloudScale = 1.0f;

    public float estimatedBreathTime = 4.0f;

    private const float MeasurementDuration = 30f;

    // Start is called before the first frame update
    void Start()
    {
        CheckScreenOrientation();
        instructionsPressController.Reset();

        var isGamePlayed = GameStateManager.Instance.HasPlayedSelectedGame();

        if (!isGamePlayed)
        {
            _isOver = false;
            _isInstruction = true;
            instructionsPressController.SetStartTutorialActive(true);
        }
        else
        {
            StartTimer();
            _isOver = false;
            instructionsPressController.SetStartTutorialActive(false);
            Inhale();
        }
    }

    IEnumerator Instructions()
    {
        _isOver = false;
        _isInstruction = true;
        instructionsPressController.SetStartTutorialActive(true);
        yield return new WaitForSecondsPaused(0.1f);
    }

    public float CloudMoveToCenterTime = 0.2f;

    IEnumerator MoveCloudToCenter()
    {
        //Vector3 originalScale = transform.localScale;
        //Vector3 destinationScale = previousScale;

        Vector3 originalPosition = Cloud.transform.localPosition;
        Vector3 destinationPosition = UICenter.transform.localPosition;

        float currentTime = 0.0f;

        do
        {
            if (!DisplayTimerController.isPausedStatic())
            {
                Cloud.transform.localPosition = Vector3.Lerp(originalPosition, destinationPosition, currentTime / CloudMoveToCenterTime);
                currentTime += Time.deltaTime;
            }
            yield return null;
        } while (currentTime <= CloudMoveToCenterTime);
    }

    IEnumerator Wait()
    {
        yield return new WaitForSecondsPaused(pauseDuration);
        PauseEnd();
    }

    void StartTimer()
    {
        displayTimerController.Activate(MeasurementDuration, () => { GameOver(); });
    }

    void Inhale()
    {
        _isInhale = true;
        instructionsPressController.ShowInhale();
    }

    void Exhale()
    {
        _isInhale = false;
        instructionsPressController.ShowExhale();
    }

    void PauseStart()
    {
    }

    void PauseGame()
    {
        instructionsPressController.ShowHold();
        StartCoroutine(nameof(Wait));
    }

    void PauseEnd()
    {
        instructionsPressController.Reset();
        Action action = Inhale;
        if (_isInhale)
        {
            action = Exhale;
        }
        if (!_isInhale && _isOver)
        {
            GameOver();
            return;
        }
    }

    void GameOver()
    {
        //UIMenuController.StaticLoadScene("RateScene");
        BreathParams params1 = new BreathParams(MaxInhaleTime, MaxExhaleTime, 0.0f, 0.0f);
        GameStateManager.Instance.setActiveBreathParams(params1);
        UIMenuController.StaticLoadScene("BreathingScene");
    }

    void CheckScreenOrientation()
    {
        isHorizontal = (Screen.width > Screen.height);
        instructionsPressController.RotateScreen(isHorizontal);
        
    }
    private bool isPaused = false;

    private void checkIsPaused()
    {
        bool temp = displayTimerController.isPaused();
        if (temp != isPaused)
        {
            isPaused = temp;
        }
    }

    bool isExpanding = false;

    private float MaxInhaleTime = 0.0f;
    private float MaxExhaleTime = 0.0f;

    // Update is called once per frame
    void Update()
    {
        if (isHorizontal && Screen.width < Screen.height)
        {
            CheckScreenOrientation();
        }
        else if (!isHorizontal && Screen.width > Screen.height)
        {
            CheckScreenOrientation();
        }

        checkIsPaused();

        if (_isInstruction)

        {
            if (Input.touchCount > 0 && EventSystem.current.currentSelectedGameObject == null)
            {
                StartTimer();
                _isInstruction = false;
                instructionsPressController.SetStartTutorialActive(false);
                //StartCoroutine(MoveCloudToCenter());
                return;
            }
        }
        else 
        {
            if (Input.touchCount > 0)
            {
                if(!isExpanding)
                {
                    MaxExhaleTime = LastScaleChangeTimer > MaxExhaleTime ? LastScaleChangeTimer : MaxExhaleTime;
                    LastScaleChangeTimer = 0.0f;
                    LastCloudScale = Cloud.transform.localScale.x;
                    isExpanding = true;
                    instructionsPressController.ShowInhale();
                }
            }
            else
            {
                if (isExpanding)
                {
                    MaxInhaleTime = LastScaleChangeTimer > MaxInhaleTime ? LastScaleChangeTimer : MaxInhaleTime;
                    LastScaleChangeTimer = 0.0f;
                    LastCloudScale = Cloud.transform.localScale.x;
                    isExpanding = false;
                    instructionsPressController.ShowExhale();
                }
            }

            UpdateCloudSize();
        }
    }

    private float LastCloudScale = 1.0f;
    private float LastScaleChangeTimer = 0.0f;

    private void UpdateCloudSize()
    {
        LastScaleChangeTimer += Time.deltaTime;
        float alpha = LastScaleChangeTimer / estimatedBreathTime;
        float nextSize = isExpanding ? maxCloudScale : minCloudScale;

        float temp_scale = (alpha * (nextSize - LastCloudScale) + LastCloudScale);

        temp_scale = Mathf.Clamp(temp_scale, minCloudScale, maxCloudScale);

        Cloud.transform.localScale = new Vector3(temp_scale, temp_scale, temp_scale);
    }
}
