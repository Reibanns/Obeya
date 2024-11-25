using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class FadeManager : MonoBehaviour
{
    public GameObject fadeToBlackPanel; // Reference to the fade to black panel GameObject
    public GameObject fadeFromBlackPanel; // Reference to the fade from black panel GameObject
    public float fadeDuration = 1f; // Duration of the fade

    private Image fadeToBlackImage; // Reference to the Image component of the fade to black panel
    private Image fadeFromBlackImage; // Reference to the Image component of the fade from black panel

    private void Start()
    {
        // Get the Image components from the panels
        fadeToBlackImage = fadeToBlackPanel.GetComponent<Image>();
        fadeFromBlackImage = fadeFromBlackPanel.GetComponent<Image>();

        // Start with both panels completely transparent
        fadeToBlackImage.color = new Color(0, 0, 0, 0);
        fadeFromBlackImage.color = new Color(0, 0, 0, 0);
    }

    // Call this method to start the scene transition
    public void FadeToScene(string sceneName)
    {
        StartCoroutine(FadeOut(sceneName));
    }

    // Coroutine to fade out and then load the scene
    private IEnumerator FadeOut(string sceneName)
    {
        // Fade to black
        yield return StartCoroutine(Fade(fadeToBlackImage, 1)); // Fade to black
        SceneManager.LoadScene(sceneName); // Load new scene
        yield return StartCoroutine(Fade(fadeFromBlackImage, 0)); // Fade from black
    }

    // Coroutine to handle fading
    private IEnumerator Fade(Image panel, float targetAlpha)
    {
        float startAlpha = panel.color.a;
        float time = 0;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, time / fadeDuration);
            panel.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        panel.color = new Color(0, 0, 0, targetAlpha);
    }
}
