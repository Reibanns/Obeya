using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    public NPCDialogueManager dialogueManager;
    public NPCDialogue npcDialogue;
    private bool isInRange = false;
    public NPC currentNPC; // Add this line to hold the current NPC reference

    void Update()
    {
        if (isInRange && Input.GetKeyDown(KeyCode.Q))
        {
            dialogueManager.StartDialogue(npcDialogue);
        }
    }

    public void StartDialogueOnSpawn()
    {
        // Automatically trigger the dialogue when the NPC is spawned
        if (dialogueManager != null)
        {
            dialogueManager.StartDialogue(npcDialogue);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isInRange = true;
            currentNPC = GetComponent<NPC>(); // Set the current NPC reference when player is in range
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isInRange = false;
            currentNPC = null; // Clear the current NPC reference when player exits
        }
    }

    // Call this method when exiting the dialogue
    public void EnableInteraction()
    {
        this.enabled = true; // Re-enable interaction when exiting the dialogue
    }
}