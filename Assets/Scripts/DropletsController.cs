using System;
using UnityEngine;

public class DropletsController : MonoBehaviour
{
    public GameObject dropletPrefab;

    private GameObject[] droplets;
    private Vector2[] dropletsVelocityBackup;

    private float gravityScale = 1.0f;

    private bool isVisible;

    public int DropletsCount = 5;

    // Start is called before the first frame update
    void Start()
    {
        const int yRef = 320;
        float height = Screen.height;

        gravityScale = height / yRef;

        droplets = new GameObject[DropletsCount];
        dropletsVelocityBackup = new Vector2[DropletsCount];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private bool isPaused;

    private Vector2 FreezeVelocity = new Vector2(0, 0);

    public void SetIsPaused(bool isPaused)
    {
        this.isPaused = isPaused;

        if (isPaused)
        {
            for (int i = 0; i < DropletsCount; i++)
            {
                if (droplets[i] != null)
                {
                    Rigidbody2D body = droplets[i].GetComponent<Rigidbody2D>();
                    body.gravityScale = 0;
                    dropletsVelocityBackup[i] = body.velocity;
                    body.velocity = FreezeVelocity;
                }
            }
        }
        else
        {
            for (int i = 0; i < DropletsCount; i++)
            {
                if(droplets[i] != null)
                { 
                    Rigidbody2D body = droplets[i].GetComponent<Rigidbody2D>();
                    body.gravityScale = gravityScale;
                    body.velocity = dropletsVelocityBackup[i];
                }
            }
        }
        
    }

    public void SetVisibility(bool isVisible)
    {
        this.isVisible = isVisible;
        if (droplets == null)
            return;
        for (int i = 0; i < droplets.Length; i++)
        {
            if (droplets[i] != null && droplets[i].active)
            {
                droplets[i].GetComponent<CanvasRenderer>().SetAlpha(isVisible ? 1 : 0);
            }
        }
    }

    public void Create()
    {
        for (int i = 0; i < droplets.Length; i++)
        {
            droplets[i] = Instantiate(dropletPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity, transform);

            droplets[i].GetComponent<CanvasRenderer>().SetAlpha(isVisible ? 1 : 0);

            Rigidbody2D body = droplets[i].GetComponent<Rigidbody2D>();
            body.gravityScale *= gravityScale;

            bool isFree;
            do
            {
                isFree = true;
                var x = UnityEngine.Random.Range(-70f, 70f);
                var y = UnityEngine.Random.Range(50f, 130f);
                droplets[i].transform.localPosition = new Vector3(x, y, 0f);

                for (int j = 0; j < i; j++)
                {
                    var delta = droplets[i].transform.localPosition - droplets[j].transform.localPosition;
                    if (Math.Abs(delta.x) < 20f && Math.Abs(delta.y) < 20f)
                    {
                        isFree = false;
                        break;
                    }
                }
            } while (!isFree);
        }
    }

    public void DestroyDroplets()
    {
        for (int i = 0; i < droplets.Length; i++)
        {
            Destroy(droplets[i]);
        }
    }
}
