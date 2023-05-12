using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour {
    public GameObject menuUI;
    public Button playButton;
    public GameObject canvas;
    
    void Start() {
        this.GetComponent<UniversalAdditionalCameraData>().renderPostProcessing =
            bool.Parse(PlayerPrefs.GetString("CRTToggle", "True"));
        playButton.GetComponent<Button>().onClick.AddListener(PlayGame);
    }

    public void PlayGame() {
        SceneManager.LoadScene("Scenes/GameScene");
    }

    public void LoadAchievements() {
        SceneManager.LoadScene("Scenes/AchievementScene");
    }
    
    public void LoadSettings() {
        SceneManager.LoadScene("Scenes/SettingsScene");
    }
    
    public void LoadHelp() {
        SceneManager.LoadScene("Scenes/HelpScene");
    }
    
    public void QuitGame() {
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
    }
}