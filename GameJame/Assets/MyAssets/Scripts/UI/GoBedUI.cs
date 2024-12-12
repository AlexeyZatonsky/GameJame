using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class GoBedUI : MonoBehaviour
{
    [SerializeField] private GameObject choicePanel;
    [SerializeField] private RawImage blackPanel;
    [SerializeField] private GameObject dot;
    [SerializeField] private float fadeSpeed = 10f;

    [Header("Buttons")]
    [SerializeField] private Button sleepButton;
    [SerializeField] private Button checkButton;

    private readonly Color highlightColor = Color.white;
    private Color originalButtonColor;
    private float originalPanelAlpha;

    private Image panelImage;
    private Image sleepButtonImage;
    private Image checkButtonImage;
    private TMP_Text sleepButtonText;
    private TMP_Text checkButtonText;
    private TMP_Text orText;

    private void Awake()
    {
        InitializeUIComponents();
        SetupInitialState();
        AddButtonListeners();
    }

    private void InitializeUIComponents()
    {
        panelImage = choicePanel.GetComponent<Image>();
        sleepButtonImage = sleepButton.image;
        checkButtonImage = checkButton.image;
        sleepButtonText = sleepButton.GetComponentInChildren<TMP_Text>();
        checkButtonText = checkButton.GetComponentInChildren<TMP_Text>();
        orText = choicePanel.GetComponentInChildren<TMP_Text>();
        originalButtonColor = sleepButtonImage.color;
        originalPanelAlpha = panelImage.color.a;
    }

    private void SetupInitialState()
    {
        blackPanel.gameObject.SetActive(false);
        blackPanel.color = new Color(0, 0, 0, 0);
        choicePanel.SetActive(true);
        SetAlpha(0f);
    }

    private void AddButtonListeners()
    {
        sleepButton.onClick.AddListener(OnSleepButtonClick);
        checkButton.onClick.AddListener(OnCheckButtonClick);
    }

    private void SetAlpha(float alpha)
    {
        panelImage.color = new Color(panelImage.color.r, panelImage.color.g, panelImage.color.b, alpha * originalPanelAlpha);

        Color UpdateAlpha(Color color) => new Color(color.r, color.g, color.b, alpha);
        
        sleepButtonImage.color = UpdateAlpha(sleepButtonImage.color);
        checkButtonImage.color = UpdateAlpha(checkButtonImage.color);
        sleepButtonText.color = UpdateAlpha(sleepButtonText.color);
        checkButtonText.color = UpdateAlpha(checkButtonText.color);
        orText.color = UpdateAlpha(orText.color);
    }

    private void OnSleepButtonClick()
    {
        sleepButtonImage.color = highlightColor;
        StartCoroutine(HandleSleepClick());
    }

    private void OnCheckButtonClick()
    {
        checkButtonImage.color = highlightColor;
        StartCoroutine(HandleCheckClick());
    }

    private IEnumerator HandleSleepClick()
    {
        StartCoroutine(FadeOut());
        yield return StartCoroutine(FadeBlackScreen(true, 2f));
        
        GameManager.Instance.SleepButtonPressed();
        CursorController.Instance.HideCursor();
    }

    private IEnumerator HandleCheckClick()
    {
        StartCoroutine(FadeOut());
        yield return StartCoroutine(FadeBlackScreen(true, 2f));
        
        GameManager.Instance.CheckButtonPressed();
        CursorController.Instance.HideCursor();
    }

    public void Show()
    {
        choicePanel.SetActive(true);
        StartCoroutine(FadeIn());
        
        sleepButtonImage.color = originalButtonColor;
        checkButtonImage.color = originalButtonColor;
    }

    private IEnumerator FadeIn()
    {
        float alpha = 0f;
        while (alpha < 1f)
        {
            alpha += Time.deltaTime * fadeSpeed;
            SetAlpha(alpha);
            yield return null;
        }
    }

    private IEnumerator FadeOut()
    {
        float alpha = 1f;
        while (alpha > 0f)
        {
            alpha -= Time.deltaTime * fadeSpeed;
            SetAlpha(alpha);
            yield return null;
        }
        choicePanel.SetActive(false);
    }

    public IEnumerator FadeBlackScreen(bool fadeIn, float speed = 2f, float delayBeforeFadeOut = 1f)
    {
        blackPanel.gameObject.SetActive(true);
        
        float startAlpha = fadeIn ? 0f : 1f;
        float endAlpha = fadeIn ? 1f : 0f;
        float currentAlpha = startAlpha;
        float alphaChangeDirection = fadeIn ? 1f : -1f;

        if (!fadeIn)
        {
            yield return new WaitForSeconds(delayBeforeFadeOut);
        }
        
        while (fadeIn ? currentAlpha < endAlpha : currentAlpha > endAlpha)
        {
            currentAlpha += alphaChangeDirection * Time.deltaTime * speed;
            blackPanel.color = new Color(0, 0, 0, Mathf.Clamp01(currentAlpha));
            yield return null;
        }
        
        if (!fadeIn)
        {
            blackPanel.gameObject.SetActive(false);
        }
    }

    public void SetBlackScreenInstant(bool isBlack)
    {
        blackPanel.gameObject.SetActive(true);
        blackPanel.color = new Color(0, 0, 0, isBlack ? 1f : 0f);
    }

    public void ShowDot(bool isActive)
    {
        dot.SetActive(isActive);
    }
}
