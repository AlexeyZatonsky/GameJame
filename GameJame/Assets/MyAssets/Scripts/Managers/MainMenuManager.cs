using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button settingButton;
    [SerializeField] private Button backSettingButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button backCreditsButton;
    [SerializeField] private Button exitButton;

    [Header("Panels")]
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject settingPanel;
    [SerializeField] private GameObject creditsPanel;

    private void Awake()
    {
        startButton.onClick.AddListener(StartGame);
        settingButton.onClick.AddListener(OpenSetting);
        //backSettingButton.onClick.AddListener(BackToMain);
        creditsButton.onClick.AddListener(OpenCredits);
        //backCreditsButton.onClick.AddListener(BackToMain);
        exitButton.onClick.AddListener(ExitGame);
    }

    private void StartGame()
    {
        SceneManager.LoadScene("MainLevel");
    }

    private void OpenSetting()
    {
        //settingPanel.SetActive(true);
    }

    private void OpenCredits()
    {
        //creditsPanel.SetActive(true);
    }

    private void BackToMain()
    {
        mainPanel.SetActive(true);
        settingPanel.SetActive(false);
        creditsPanel.SetActive(false);
    }

    private void ExitGame()
    {
        Application.Quit();
    }
}
