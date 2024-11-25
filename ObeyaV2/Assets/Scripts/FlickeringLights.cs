using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FlickeringLightPattern : MonoBehaviour
{
    public Light2D light2D;                 // Reference to the Light2D component
    public float minIntensity = 0.0f;       // Minimum light intensity (completely off)
    public float maxIntensity = 1.0f;       // Maximum light intensity (fully on)
    
    public float flickerDuration = 0.1f;    // How long each flicker lasts
    public float normalLightDuration = 2.0f; // How long the light stays on after flickering
    public int flickerCount = 3;            // Number of flickers before normal light

    private int currentFlicker = 0;         // Tracks the current flicker count
    private float timer = 0.0f;             // Timer for managing flicker and normal phases
    private bool isFlickering = true;       // Is the light in a flicker phase or normal light phase
    private bool lightOn = false;           // Is the light currently on or off

    void Start()
    {
        if (light2D != null)
        {
            // Start with the light on
            light2D.intensity = maxIntensity;
            timer = flickerDuration;       // Start the timer for the flicker phase
        }
    }

    void Update()
    {
        if (light2D != null)
        {
            timer -= Time.deltaTime;

            if (isFlickering)
            {
                // Handle flickering phase
                if (timer <= 0)
                {
                    // Toggle the light between on and off
                    lightOn = !lightOn;
                    light2D.intensity = lightOn ? maxIntensity : minIntensity;

                    currentFlicker++;

                    // If we've reached the number of flickers, move to the normal light phase
                    if (currentFlicker >= flickerCount)
                    {
                        isFlickering = false;
                        currentFlicker = 0;
                        timer = normalLightDuration; // Start normal light phase
                    }
                    else
                    {
                        // Continue flickering with a new timer
                        timer = flickerDuration;
                    }
                }
            }
            else
            {
                // Handle normal light phase
                if (timer <= 0)
                {
                    // After the normal phase, go back to flickering
                    isFlickering = true;
                    timer = flickerDuration; // Start flickering again
                }
                else
                {
                    // Keep the light fully on during the normal phase
                    light2D.intensity = maxIntensity;
                }
            }
        }
    }
}
