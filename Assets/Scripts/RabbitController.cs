using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitController : MonoBehaviour
{
    private string[] m_buttonNames = new string[] { "Idle", "Run", "Dead" };

    private const int Idle = 0;
    private const int Run = 1;
    private const int Dead = 2;
    private bool canJump = true;


    Vector2 TouchStartPos;

    private Vector3 position;
    private float width;
    private float height;

    private ColorDotController activeColorDot = null;

    private Animator m_animator;
    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_animator.SetInteger("AnimIndex", Idle);
        Debug.Log("touch");
        m_animator.SetTrigger("Next");
        canJump = true;
    }

    public void JumpTo(ColorDotController nextColorDot)
    {
        if (!canJump)
            return;
        if (activeColorDot)
        {
            //activeColorDot.Destroy();
        }

        activeColorDot = nextColorDot;

        transform.forward = nextColorDot.transform.position - transform.position;

        m_animator.SetInteger("AnimIndex", Run);
        m_animator.SetTrigger("Next");

        canJump = false;

        StartCoroutine(MoveRabbit(transform.position, nextColorDot.transform.position, 0.7f));
    }

    private IEnumerator MoveRabbit(Vector3 startPos, Vector3 endPos, float duration)
    {
        float elapsedTime = 0;
        float ratio = elapsedTime / duration;
        while (ratio < 1f)
        {
            elapsedTime += Time.deltaTime;
            ratio = elapsedTime / duration;
            transform.position = Vector3.Lerp(startPos, endPos, ratio);
            yield return null;
        }
        m_animator.SetInteger("AnimIndex", Idle);
        m_animator.SetTrigger("Next");
        canJump = true;
    }

    public void JumpTo()
    {
        Debug.Log("touch");
    }

    void Update()
    {
        // Handle screen touches.
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                TouchStartPos = touch.position;
            }

            // Move the cube if the screen has the finger moving.
            if (touch.phase == TouchPhase.Moved)
            {
                Vector2 pos = touch.position;
                pos.x = pos.x / width;



                //pos.y = (pos.y - height) / height;
                //position = new Vector3((pos.x * maxWidth) - maxWidth / 2, transform.position.y, transform.position.z);

                // Position the cube.
                transform.position = position;
            }

            if (touch.phase == TouchPhase.Ended)
            {
                if (TouchStartPos == touch.position)
                {
                    // Tap event
                    //isJumping = true;
                }
            }
            /*
            if (Input.touchCount == 2)
            {
                touch = Input.GetTouch(1);

                if (touch.phase == TouchPhase.Began)
                {
                    // Halve the size of the cube.
                    transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
                }

                if (touch.phase == TouchPhase.Ended)
                {
                    // Restore the regular size of the cube.
                    transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                }
            }*/
        }
    }
}
