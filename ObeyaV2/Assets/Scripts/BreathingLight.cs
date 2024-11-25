using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BreathingLight : MonoBehaviour
{
    public Light2D light2D;         // Reference to the Light2D component
    public float minIntensity = 0.5f; // Minimum light intensity
    public float maxIntensity = 1.0f; // Maximum light intensity
    public float breathSpeed = 2.0f;  // Speed of breathing

    private bool breathingIn = true; // Direction of breathing
    private float currentIntensity;    // Current light intensity

    void Start()
    {
        if (light2D != null)
        {
            currentIntensity = light2D.intensity; // Set initial intensity
        }
    }

    void Update()
    {
        if (light2D != null)
        {
            // Update light intensity
            if (breathingIn)
            {
                currentIntensity += breathSpeed * Time.deltaTime;
                if (currentIntensity >= maxIntensity)
                {
                    currentIntensity = maxIntensity;
                    breathingIn = false; // Switch to breathing out
                }
            }
            else
            {
                currentIntensity -= breathSpeed * Time.deltaTime;
                if (currentIntensity <= minIntensity)
                {
                    currentIntensity = minIntensity;
                    breathingIn = true; // Switch to breathing in
                }
            }

            // Set the light intensity
            light2D.intensity = currentIntensity;
        }
    }
}
