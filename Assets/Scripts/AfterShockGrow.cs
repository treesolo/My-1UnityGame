using UnityEngine;

public class AfterShockGrow : MonoBehaviour
{
    public float growthRate = 100.0f;
    private float maxGrow = 60.0f;
    void Update()
    {
        transform.localScale += new Vector3(growthRate, 0, growthRate) * Time.deltaTime;
        if (transform.localScale.x > maxGrow)
        {
            Destroy(gameObject);
        }
    }
}
