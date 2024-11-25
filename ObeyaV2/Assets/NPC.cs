using UnityEngine;

public class NPC : MonoBehaviour
{
    public NPCDialogue npcDialogue; // Reference to the NPCDialogue ScriptableObject

    public SpriteRenderer spriteRenderer; // Reference to the sprite renderer to change sprite
    public Sprite deadBodySprite; // Sprite for the dead body after killing the NPC

    private Animator animator; // Reference to the Animator component

    private void Awake()
    {
        // Get the Animator component when the NPC is instantiated
        animator = GetComponent<Animator>();
    }

    public void RespondToCheck(string response)
    {
        // Display the response in the game's dialogue UI
        Debug.Log(response);
    }

    public void SwapToDeadSprite()
    {
        // Set the isDead parameter to true to trigger the death animation
        if (animator != null)
        {
            animator.SetTrigger("isDead");
            Debug.Log("NPC marked as dead: " + gameObject.name);
        }

        // Optionally set the sprite here in case the animator doesn't handle it
        if (deadBodySprite != null)
        {
            spriteRenderer.sprite = deadBodySprite; // Change the sprite to dead
            Debug.Log("Swapped to dead sprite for " + gameObject.name); // Log sprite swap
        }
        else
        {
            Debug.LogError("Dead body sprite is not assigned for " + gameObject.name);
        }
    }

    // Check if the NPC has a specific feature based on the ScriptableObject
    public bool HasFeature(int featureIndex)
    {
        // Return true if the feature index is valid, otherwise false
        return featureIndex >= 0 && featureIndex < npcDialogue.featureDialoguesList.Count;
    }
}
