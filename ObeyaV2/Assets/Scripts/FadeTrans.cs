using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class FadeTrans : MonoBehaviour
{
    public Image fadeToBlackPanel; // Reference to the fade to black panel
    public Image fadeFromBlackPanel; // Reference to the fade from black panel
    public float fadeDuration = 1f; // Duration of the fade

    private void Start()
    {
        // Start with both panels completely transparent
        fadeToBlackPanel.color = new Color(0, 0, 0, 0);
        fadeFromBlackPanel.color = new Color(0, 0, 0, 0);
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
        yield return StartCoroutine(Fade(fadeToBlackPanel, 1)); // Fade to black
        SceneManager.LoadScene(sceneName); // Load new scene
        yield return StartCoroutine(Fade(fadeFromBlackPanel, 0)); // Fade from black
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
