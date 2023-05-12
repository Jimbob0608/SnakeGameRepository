using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class HelpScript : MonoBehaviour
{
    private void Start() {
        this.GetComponent<UniversalAdditionalCameraData>().renderPostProcessing =
            bool.Parse(PlayerPrefs.GetString("CRTToggle", "True"));
    }

    public void ReturnToMenu() {
        SceneManager.LoadScene("Scenes/MenuScene");
    }
}
