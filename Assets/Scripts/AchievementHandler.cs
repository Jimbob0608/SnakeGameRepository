using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AchievementHandler : MonoBehaviour {
    private int count;
    public Button blackMamba;
    public Button hungry;
    public Button hatchling;
    public Button python;
    public Button millennial;
    public Button secretAchievement;
    
    // Start is called before the first frame update
    void Start() {
        this.GetComponent<UniversalAdditionalCameraData>().renderPostProcessing =
            bool.Parse(PlayerPrefs.GetString("CRTToggle", "true"));
        if (PlayerPrefs.GetString("BlackMambaUnlocked", "") == "true") {
            blackMamba.interactable = true;
        }
        if (PlayerPrefs.GetString("HungryUnlocked", "") == "true") {
            hungry.interactable = true;
        }
        if (PlayerPrefs.GetString("HatchlingUnlocked", "") == "true") {
            hatchling.interactable = true;
        }
        if (PlayerPrefs.GetString("PythonUnlocked", "") == "true") {
            python.interactable = true;
        }
        if (PlayerPrefs.GetString("MillennialUnlocked", "") == "true") {
            millennial.interactable = true;
        }
    }

    private void Update() {
        if (PlayerPrefs.GetString("AchievementUnlocked", "") == "true") {
            changeButtonColour();
        }
    }

    public void resetAchievements() {
        PlayerPrefs.SetString("AchievementUnlocked", "false");
        PlayerPrefs.SetString("BlackMambaUnlocked", "false");
        PlayerPrefs.SetString("HungryUnlocked", "false");
        PlayerPrefs.SetString("HatchlingUnlocked", "false");
        PlayerPrefs.SetString("PythonUnlocked", "false");
        PlayerPrefs.SetString("MillennialUnlocked", "false");
        SceneManager.LoadScene("Scenes/AchievementScene");

    }

    private void changeButtonColour() {
        Button secretButton = secretAchievement.GetComponent<Button>();
        ColorBlock buttonColors = secretButton.colors;
        buttonColors.normalColor = Color.white;
        buttonColors.highlightedColor = Color.white;
        buttonColors.pressedColor = Color.white;
        buttonColors.selectedColor = Color.white;
        secretButton.colors = buttonColors;
        secretAchievement.GetComponent<Button>().colors = secretButton.colors;
    }
    public void SecretAchievementUnlocker() {
        count++;
        if (count > 5) {
            PlayerPrefs.SetString("AchievementUnlocked", "true");
        }
    }

    public void ReturnToMenu() {
        SceneManager.LoadScene("Scenes/MenuScene");
    }
}
