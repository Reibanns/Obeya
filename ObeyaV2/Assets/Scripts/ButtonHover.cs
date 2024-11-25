using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public TMP_Text buttonText;
    public Color normalColor = Color.white;
    public Color hoverColor = Color.blue;

    public AudioClip hoverSound;
    public AudioClip clickSound;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // This method is called when the pointer enters the button
    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonText.color = hoverColor;
        PlaySound(hoverSound); // Play hover sound
    }

    // This method is called when the pointer exits the button
    public void OnPointerExit(PointerEventData eventData)
    {
        buttonText.color = normalColor;
    }

    // This method is called when the button is clicked
    public void OnPointerClick(PointerEventData eventData)
    {
        PlaySound(clickSound); // Play click sound
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}