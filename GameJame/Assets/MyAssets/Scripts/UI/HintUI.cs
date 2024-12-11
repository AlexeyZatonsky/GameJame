using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HintUI : SingletonManager<HintUI>
{
    [SerializeField] private GameObject HintPanel;
    [SerializeField] private TextMeshProUGUI HintText;

    protected override void Awake()
    {
        base.Awake();

        if (HintPanel != null)
        {
            HintPanel.SetActive(false);
        }
    }

    public void ShowHint(string description)
    {
        if (HintPanel == null || HintText == null)
        {
            Debug.LogError("HintPanel or HintText is not assigned in the inspector!");
            return;
        }

        HintText.text = description;
        HintPanel.SetActive(true);
    }

    public void HideTooltip()
    {
        if (HintPanel == null || HintText == null)
        {
            Debug.LogError("HintPanel or HintText is not assigned in the inspector!");
            return;
        }

        HintText.text = "";
        HintPanel.SetActive(false);
    }
}