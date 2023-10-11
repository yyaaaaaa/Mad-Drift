using UnityEngine;


public class DisableOverTime : MonoBehaviour
{
    float timer;
    public float deathtimer = 10;
    private void OnEnable()
    {
        timer = 0f;
    }
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= deathtimer)
        {
            gameObject.SetActive(false);
        }

    }
}