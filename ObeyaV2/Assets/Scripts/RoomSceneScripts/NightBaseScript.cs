using UnityEngine;

public class NightBaseScript : MonoBehaviour
{
    public NightManager nightManager; // Reference to the NightManager
    public int maxGuestsForTheNight = 3; // Number of guests for this night
    protected int currentGuestCount = 0; // Keep it protected to allow access in derived classes

    // Track guest types
    protected int humansAccepted = 0;
    protected int aswangsAccepted = 0;

    public virtual void OnGuestInteraction(GameObject guest)
    {
        // This method should be called when a guest is interacted with (accepted)
        currentGuestCount++;
        nightManager.AddGuest(guest); // Add guest to the global list

        // Check the tag of the guest to increment the appropriate counter
        if (guest.CompareTag("Human"))
        {
            humansAccepted++;
            Debug.Log("Human accepted. Total humans accepted: " + humansAccepted);
        }
        else if (guest.CompareTag("Aswang"))
        {
            aswangsAccepted++;
            Debug.Log("Aswang accepted. Total aswangs accepted: " + aswangsAccepted);
        }

        // Check if the maximum guest count for the night has been reached
        if (currentGuestCount >= maxGuestsForTheNight)
        {
            OnNightComplete(); // Call derived class method to handle the completion
        }
    }

    protected virtual void OnNightComplete()
    {
        // To be implemented in derived classes
        Debug.Log("Night complete. Go to bed to proceed to the next night.");
    }
}
