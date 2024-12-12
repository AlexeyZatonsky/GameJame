using UnityEngine;
using TMPro;
using System.Collections;
using System.Linq;
using UnityEngine.UI;

public class SubtitlesManager : MonoBehaviour
{
    public static SubtitlesManager Instance { get; private set; }

    [Header("Subtitles")]
    [SerializeField] private GameObject subPanel;
    [SerializeField] private TMP_Text subText;
    [SerializeField] private float horizontalPadding = 40f;
    [SerializeField] private float verticalPadding = 20f;
    
    [Header("Animation Settings")]
    [SerializeField] private float fadeSpeed = 5f;

    [Header("Dialogues")]
    [SerializeField] private DialogueData[] dialogues; 
    
    private Image panelImage;
    
    private Coroutine currentDialogueCoroutine;

    private float originalPanelAlpha;
    private float originalTextAlpha;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        //DontDestroyOnLoad(gameObject);

        subPanel.SetActive(false);
        panelImage = subPanel.GetComponent<Image>();
        originalPanelAlpha = panelImage.color.a;
        originalTextAlpha = subText.color.a;
        subText.text = "";
    }

    private void ShowDialogue(DialogueData dialogue)
    {
        if (currentDialogueCoroutine != null)
        {
            StopCoroutine(currentDialogueCoroutine);
        }
        
        currentDialogueCoroutine = StartCoroutine(ShowDialogueCoroutine(dialogue));
    }

    private IEnumerator ShowDialogueCoroutine(DialogueData dialogue)
    {
        if (dialogue.dialogueLines.Length > 0 && dialogue.dialogueLines[0].initialDelay > 0)
            yield return new WaitForSeconds(dialogue.dialogueLines[0].initialDelay);

        foreach (var line in dialogue.dialogueLines)
        {
            SetAlpha(0f);
            subText.text = line.text;
            subPanel.SetActive(true);
            
            RectTransform panelRect = subPanel.GetComponent<RectTransform>();
            RectTransform textRect = subText.GetComponent<RectTransform>();
            
            yield return null;
            
            Vector2 textSize = subText.GetPreferredValues();
            panelRect.sizeDelta = new Vector2(
                textSize.x + horizontalPadding,
                textSize.y + verticalPadding
            );
            
            yield return FadeElements(true);
            
            yield return new WaitForSeconds(line.displayTime);
            
            yield return FadeElements(false);
            
            subPanel.SetActive(false);
            subText.text = "";
            
            yield return new WaitForSeconds(line.delayBeforeNext);
        }

        currentDialogueCoroutine = null;
    }

    private void SetAlpha(float alphaMultiplier)
    {
        Color panelColor = panelImage.color;
        Color textColor = subText.color;
        
        panelColor.a = originalPanelAlpha * alphaMultiplier;
        textColor.a = originalTextAlpha * alphaMultiplier;
        
        panelImage.color = panelColor;
        subText.color = textColor;
    }

    private IEnumerator FadeElements(bool fadeIn)
    {
        float currentTime = 0f;
        float startAlpha = fadeIn ? 0f : 1f;
        float targetAlpha = fadeIn ? 1f : 0f;
        
        while (currentTime < 1f)
        {
            currentTime += Time.deltaTime * fadeSpeed;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, currentTime);
            SetAlpha(alpha);
            yield return null;
        }
        
        SetAlpha(targetAlpha);
    }

    public void PlayDialogue(string dialogueName)
    {
        DialogueData dialogue = dialogues.FirstOrDefault(d => d.name == dialogueName);
        if (dialogue == null)
        {
            Debug.LogWarning($"Диалог с именем {dialogueName} не найден!");
            return;
        }

        ShowDialogue(dialogue);
    }

    /* ---- Если вдруг кто захочет по индексам искать диалоги ----
    public void PlayDialogue(int dialogueIndex)
    {
        if (dialogueIndex < 0 || dialogueIndex >= dialogues.Length)
        {
            Debug.LogWarning($"Диалог с индексом {dialogueIndex} не существует!");
            return;
        }

        ShowDialogue(dialogues[dialogueIndex]);
    }
    */
}
