using UnityEngine;

public class Night2Script : NightBaseScript
{
    protected override void OnNightComplete()
    {
        // Custom logic for Night 1 completion
        Debug.Log("Night 2 complete. Go to bed to proceed to the next night.");
        // You can add any additional logic specific to Night 1 here
    }
}
