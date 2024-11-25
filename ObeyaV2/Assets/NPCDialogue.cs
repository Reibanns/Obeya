using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FeatureDialogue
{
    public string featureName; // The name of the feature (e.g., "Elongated Limbs")
    public Sprite featureImage; // The UI image for the feature
    public List<string> featureDialogues; // Dialogue lines related to this feature
    public string hearOutDialogue; // Unique dialogue for hearing out the feature
}

[CreateAssetMenu(fileName = "NPCDialogue", menuName = "Dialogue/NPCDialogue")]
public class NPCDialogue : ScriptableObject
{
    public string uniqueID;
    public string npcName; // NPC's Name
    public Sprite npcSprite; // The sprite/image for the NPC
    public Sprite killedNpcSprite; 
    public string[] aswangResponses; // Responses for Aswang
    public string[] eclipseResponses; // Responses for Eclipse
    public string[] killDialogues; // Dialogue lines for the "Kill" option
    public string[] hearOutDialogues; // Dialogue lines for the "Hear Out" option

    //For different features
    public string[] hearOutDialogues1;
    public string[] hearOutDialogues2;
    public string[] hearOutDialogues3;
    public string[] hearOutDialogues4;

    public List<string> dialoguesNight1; // List of dialogues for night 1
    public List<string> dialoguesNight2; // List of dialogues for night 2
    public List<string> dialoguesNight3; // List of dialogues for night 3
    public List<string> dialoguesNight4; // List of dialogues for night 4
    public List<string> dialoguesNight5; // List of dialogues for night 5

    // For outside dialogue
    public List<string> introductoryDialogue;
    public string[] option1Responses; // UI button for Option 1
    public string[] option1_1Responses; // UI button for Option 1.1
    public string[] option2Responses; // UI button for Option 2
    public string[] option2_1Responses; // UI button for Option 2.1


    public List<FeatureDialogue> featureDialoguesList; // List of dialogues for each feature

    public string GetDialogueForFeature(string featureName)
    {
        FeatureDialogue featureDialogue = featureDialoguesList.Find(feature => feature.featureName == featureName);
        if (featureDialogue != null && featureDialogue.featureDialogues.Count > 0)
        {
            return featureDialogue.featureDialogues[0]; // You can randomize this if needed
        }
        return "No dialogue available for this feature.";
    }

    public string GetHearOutDialogue(string featureName)
    {
        FeatureDialogue featureDialogue = featureDialoguesList.Find(feature => feature.featureName == featureName);
        return featureDialogue != null ? featureDialogue.hearOutDialogue : "No reason available for this feature.";
    }
}
