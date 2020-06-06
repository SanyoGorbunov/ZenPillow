using UnityEngine;

public class DropletsController : MonoBehaviour
{
    public GameObject dropletPrefab;

    private GameObject[] droplets;

    // Start is called before the first frame update
    void Start()
    {
        droplets = new GameObject[1];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Create()
    {
        droplets[0] = Instantiate(dropletPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity, transform);
        droplets[0].transform.localPosition = new Vector3(0f, 100f, 0f);
    }

    public void DestroyDroplets()
    {
        for (int i = 0; i < droplets.Length; i++)
        {
            Destroy(droplets[i]);
        }
    }
}
