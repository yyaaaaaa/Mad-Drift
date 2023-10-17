using UnityEngine;
using UnityEngine.SceneManagement;

public class SkidMarks : MonoBehaviour
{
    private TrailRenderer skidMark;
    private ParticleSystem smoke;
    public ArcadeVehicleController carController;
    float fadeOutSpeed;
    ParticleSystem.MainModule mainModule;
    ParticleSystem.ColorOverLifetimeModule colorover;
    private void Awake()
    {
        
        smoke = GetComponent<ParticleSystem>();
        mainModule = smoke.main;  
        colorover = smoke.colorOverLifetime;     
        skidMark = GetComponent<TrailRenderer>();
        skidMark.emitting = false;
        skidMark.startWidth = carController.skidWidth;

    }


    private void OnEnable()
    {
        string scene = SceneManager.GetActiveScene().name;
        skidMark.enabled = true;
        skidMark.emitting = true;
        if (scene == "Level3")
        {
            mainModule.startColor = new Color(255, 220, 69, 60);
            colorover.color = new Color(255, 220, 69, 60);
        }
        else
        {
            mainModule.startColor = new Color(255, 255, 255, 60);
            colorover.color = new Color(255, 255, 255, 60);
        }
    }
    private void OnDisable()
    {
        skidMark.enabled = false;
        skidMark.emitting = false;
    }

    void FixedUpdate()
    {
        if (carController.transform.position.z <= 5f && carController.transform.position.z >= -5f)
        {
            fadeOutSpeed = 100000000f;
        }
        if (carController.grounded())
        {

            if (Mathf.Abs(carController.carVelocity.x) > 10)
            {
                fadeOutSpeed = 0f;
                skidMark.materials[0].color = Color.black;
                skidMark.emitting = true;
            }
            else
            {
                skidMark.emitting = false;
            }
        }
        else
        {
            skidMark.emitting = false;

        }
        if (!skidMark.emitting)
        {
            fadeOutSpeed += Time.deltaTime / 2;
            Color m_color = Color.Lerp(Color.black, new Color(0f, 0f, 0f, 0f), fadeOutSpeed);
            skidMark.materials[0].color = m_color;
            if (fadeOutSpeed > 1)
            {
                skidMark.Clear();
            }
        }

        // smoke
        if (skidMark.emitting == true)
        {
            smoke.Play();
        }
        else { smoke.Stop(); }

    }
}
