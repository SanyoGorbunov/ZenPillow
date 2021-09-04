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

    public float pauseDuration = 6.0f;

    private bool _isInhale, _isOver;

    public bool isHorizontal = false, _isInstruction;

    public float minCloudScale = 0.3f;
    public float maxCloudScale = 1.0f;

    public float estimatedBreathTime = 4.0f;

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

    IEnumerator Wait()
    {
        yield return new WaitForSecondsPaused(pauseDuration);
        PauseEnd();
    }

    void StartTimer()
    {
        var gameLength = GameStateManager.Instance.GetAdjustedTimeLengthInSecs();
        displayTimerController.Activate(gameLength, () => { _isOver = true; });
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
        UIMenuController.StaticLoadScene("RateScene");
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

                //Inhale();
                return;
            }
        }
        else 
        {
            if (Input.touchCount > 0)
            {
                if(!isExpanding)
                {
                    LastScaleChangeTimer = 0.0f;
                    LastCloudScale = Cloud.transform.localScale.x;
                    isExpanding = true;
                }
            }
            else
            {
                if (isExpanding)
                {
                    LastScaleChangeTimer = 0.0f;
                    LastCloudScale = Cloud.transform.localScale.x;
                    isExpanding = false;
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

        Cloud.transform.localScale = new Vector3(temp_scale, temp_scale, temp_scale);
    }
}
