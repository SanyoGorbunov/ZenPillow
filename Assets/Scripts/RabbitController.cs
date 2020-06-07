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

    private int CarrotCountLeft = 0;

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
        //Debug.Log("touch");
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

        Vector3 currentPos = transform.position;

        StartCoroutine(MoveRabbit(currentPos, nextColorDot.transform.position, 0.7f));
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.name == "Capsule")
        {
            Destroy(collider.transform.parent.gameObject);

            FindObjectOfType<RabbitJumpController>().PickUpCarrot();
        }

        Debug.Log(collider.gameObject.name);
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
        if (activeColorDot.hasCarrot)
        {
            activeColorDot.hideCarrot();
            FindObjectOfType<RabbitJumpController>().PickUpCarrot();
        }
    }

    public void JumpTo()
    {
        Debug.Log("touch");
    }

    void Update()
    {
        
    }
}
