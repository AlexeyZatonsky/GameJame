using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public enum GameState
{
    StartCutscene,
    FirstTimer,
    InBed,
    LastChanceTimer,
    Win,
    Lose
}

public enum PlayerState
{
    NoAction,
    Action
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game States")]
    [SerializeField] private GameState currentState = GameState.StartCutscene;
    [SerializeField] private PlayerState playerState = PlayerState.Action;

    [Header("Timer Settings")]
    [SerializeField] private float timeToBed = 180f;
    [SerializeField] private float timeForLastChance = 60f;
    [SerializeField] private float loseWalkDuration = 2f;
    [SerializeField] private float loseTurnDuration = 1.5f;
    [SerializeField] private float loseBlackScreenDuration = 2f;
    [SerializeField] private float delayBeforeShowChoiceUI = 2f;
    [SerializeField] private float delayBeforeBlackScreenFadeOut = 1f;
    [SerializeField] private float blackScreenSpeedFade = 1f;
    [SerializeField] private float blackScreenSpeedFadeBackMenu = 2f;
    [SerializeField] private float delayBeforeMainMenu = 5f;
    [SerializeField] private Vector3 enemySpawnOffset = new Vector3(0, 0, 2f);
    [SerializeField] private Vector3 enemyRotationAngles = new Vector3(0, 180, 0);

    [Header("Game Settings")]
    [SerializeField] private int unfixedEventObjectsCount = 0;

    [Header("GameState Objects")]
    [SerializeField] private Camera winCamera;
    [SerializeField] private Transform playerLosePosition;
    [SerializeField] private GameObject enemy;

    public event Action<GameState> OnGameStateChanged;
    public event Action<PlayerState> OnPlayerStateChanged;

    private EventObjectManager eventObjectManager;
    private TimeManager timeManager;
    private PlayerInfo playerInfo;
    private BedController bedController;
    private GoBedUI goBedUI;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        //DontDestroyOnLoad(gameObject);

        winCamera.gameObject.SetActive(false);
        eventObjectManager = FindFirstObjectByType<EventObjectManager>();
        timeManager = FindFirstObjectByType<TimeManager>();
        playerInfo = FindFirstObjectByType<PlayerInfo>();
        bedController = FindFirstObjectByType<BedController>();
        goBedUI = FindFirstObjectByType<GoBedUI>();
    }
    
    private void Start()
    {
        OnGameStateChanged?.Invoke(currentState);
        OnPlayerStateChanged?.Invoke(playerState);

        SoundManager.Instance.PlayMusic("GameMusic",  1);
        //SetGameState();
    }

    private void OnEnable()
    {
        timeManager.OnTimerEnd += SetGameState;
    }

    private void OnDisable()
    {
        timeManager.OnTimerEnd -= SetGameState;
    }

    public void SetGameState()
    {
        switch (currentState)
        {
            case GameState.StartCutscene:
                StartCoroutine(HandleStartCutscene());
                break;
            
            case GameState.FirstTimer:
                UpdateGameState(GameState.FirstTimer, PlayerState.NoAction);
                StartCoroutine(HandleBedTransition());
                break;
            
            case GameState.LastChanceTimer:
                GameState newState = CheckFixes();
                UpdateGameState(newState, playerState);
                StartCoroutine(HandleLastChanceTimerEnd(newState));
                break;
        }
    }

    private void UpdateGameState(GameState newGameState, PlayerState newPlayerState)
    {
        currentState = newGameState;
        playerState = newPlayerState;
        OnGameStateChanged?.Invoke(currentState);
        OnPlayerStateChanged?.Invoke(playerState);
    }

    private IEnumerator HandleStartCutscene()
    {
        goBedUI.SetBlackScreenInstant(true);
        timeManager.SetTimerUI(false);
        yield return StartCoroutine(goBedUI.FadeBlackScreen(false, blackScreenSpeedFade));
        UpdateGameState(GameState.FirstTimer, PlayerState.Action);
        timeManager.SetTimerUI(true);
        SubtitlesManager.Instance.PlayDialogue("D_FirstTimer_Start");
    }

    private IEnumerator HandleBedTransition()
    {
        timeManager.SetTimerUI(false);
        yield return StartCoroutine(goBedUI.FadeBlackScreen(true, blackScreenSpeedFade));
        
        timeManager.SetTimerUI(false);
        goBedUI.ShowDot(false);
        playerInfo.GetComponent<PlayerInventory>().DropItem(true, 2f);
        UpdateGameState(GameState.InBed, PlayerState.NoAction);
        PlayerSleeping();

        SubtitlesManager.Instance.PlayDialogue("D_Bed");
        
        yield return StartCoroutine(goBedUI.FadeBlackScreen(false, blackScreenSpeedFade, delayBeforeBlackScreenFadeOut));
    }

    private IEnumerator HandleLastChanceTimerEnd(GameState newState)
    {
        yield return StartCoroutine(goBedUI.FadeBlackScreen(true, blackScreenSpeedFade));
        
        if (newState == GameState.Lose) ShowLoseEnd();
        else if (newState == GameState.Win) ShowWinEnd();

        timeManager.SetTimerUI(false);
        playerInfo.GetComponent<PlayerInventory>().DropItem(true, 2f);
        
        yield return StartCoroutine(goBedUI.FadeBlackScreen(false, blackScreenSpeedFade, delayBeforeBlackScreenFadeOut));
    }

    private void PlayerSleeping()
    {
        playerInfo.transform.position = bedController.GetSleepPosition.position;
        playerInfo.transform.rotation = bedController.GetSleepPosition.rotation;

        playerInfo.GetComponent<CharacterController>().enabled = false;
        playerInfo.GetComponent<Animator>().SetBool("IS_Sleep", true);
        playerInfo.GetComponent<Animator>().applyRootMotion = true;
        playerInfo.GetComponent<PlayerRigController>().SetHeadUpperChestWeight(0f);

        StartCoroutine(ShowChoiceUIAfterDelay());
    }

    private IEnumerator ShowChoiceUIAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeShowChoiceUI);
        goBedUI.Show();
        CursorController.Instance.ShowCursor();
    }

    private void PlayerWakeUp()
    {
        playerInfo.transform.position = bedController.GetWakeUpPosition.position;
        playerInfo.transform.rotation = bedController.GetWakeUpPosition.rotation;

        playerInfo.GetComponent<Animator>().SetBool("IS_Sleep", false);
        playerInfo.GetComponent<Animator>().applyRootMotion = false;
        playerInfo.GetComponent<CharacterController>().enabled = true;
        playerInfo.GetComponent<PlayerRigController>().SetHeadUpperChestWeight(1f);
    }

    private void PlayerLose()
    {
        playerInfo.transform.position = playerLosePosition.position;
        playerInfo.transform.rotation = playerLosePosition.rotation;

        playerInfo.GetComponent<CharacterController>().enabled = false;
        playerInfo.GetComponent<Animator>().SetBool("IS_LoseWalk", true);
        playerInfo.GetComponent<Animator>().applyRootMotion = true;
        playerInfo.GetComponent<PlayerRigController>().SetHeadUpperChestWeight(0f);
    }

    public void CheckButtonPressed()
    {
        GameState newState = CheckFixes();
        
        if (newState == GameState.LastChanceTimer)
        {
            UpdateGameState(newState, PlayerState.Action);
            PlayerWakeUp();
            goBedUI.ShowDot(true);
            //SubtitlesManager.Instance.PlayDialogue("D_LastChance");
            StartCoroutine(goBedUI.FadeBlackScreen(false, blackScreenSpeedFade, delayBeforeBlackScreenFadeOut));
        }
        else
        {
            HandleEndGame(newState);
        }
    }

    public void SleepButtonPressed()
    {
        GameState newState = eventObjectManager.EventObjectsList.Count == eventObjectManager.FixedCount ? GameState.Win : GameState.Lose;
        HandleEndGame(newState);
    }

    private void HandleEndGame(GameState newState)
    {
        UpdateGameState(newState, PlayerState.NoAction);
        if (newState == GameState.LastChanceTimer || newState == GameState.Lose)
        {
            ShowLoseEnd();
            StartCoroutine(goBedUI.FadeBlackScreen(false, blackScreenSpeedFade, delayBeforeBlackScreenFadeOut));
        }
        else if (newState == GameState.Win)
        {
            ShowWinEnd();
            StartCoroutine(goBedUI.FadeBlackScreen(false, blackScreenSpeedFade, delayBeforeBlackScreenFadeOut));
        }
    }

    private void ShowWinEnd()
    {
        EnableEventManager.Instance.rebootEventPool();
        
        playerState = PlayerState.NoAction;
        OnPlayerStateChanged?.Invoke(playerState);

        GameObject player = playerInfo.GameObject();
        player.SetActive(false);
        timeManager.SetTimerUI(false);
        goBedUI.ShowDot(false);
        var playerCamera = playerInfo.GetComponentInChildren<PlayerCamera>().GetComponentInChildren<Camera>();
        playerCamera.gameObject.SetActive(false);
        winCamera.gameObject.SetActive(true);
        SubtitlesManager.Instance.PlayDialogue("D_WinEnd");

        StartCoroutine(ReturnToMainMenu());
    }

    private void ShowLoseEnd()
    {
        EnableEventManager.Instance.rebootEventPool();
        
        playerState = PlayerState.NoAction;
        OnPlayerStateChanged?.Invoke(playerState);

        PlayerLose();
        StartCoroutine(HandleLoseEndSequence());
    }

    private IEnumerator HandleLoseEndSequence()
    {
        SubtitlesManager.Instance.PlayDialogue("D_EndGame");

        yield return new WaitForSeconds(loseWalkDuration);

        Vector3 enemyPosition = playerInfo.transform.position + playerInfo.transform.TransformDirection(enemySpawnOffset);
        Quaternion enemyRotation = Quaternion.Euler(enemyRotationAngles);
        GameObject spawnedEnemy = Instantiate(enemy, enemyPosition, enemyRotation);
        
        playerInfo.GetComponent<Animator>().SetBool("IS_LoseWalk", false);
        playerInfo.GetComponent<Animator>().SetBool("IS_LoseTurn", true);

        spawnedEnemy.GetComponent<Animator>().SetTrigger("Punch");
        SoundManager.Instance.PlaySound("Punch");
        
        yield return new WaitForSeconds(loseTurnDuration);
        
        goBedUI.SetBlackScreenInstant(true);
        
        yield return new WaitForSeconds(loseBlackScreenDuration);
        
        CursorController.Instance.ShowCursor();
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    private IEnumerator ReturnToMainMenu()
    {
        yield return new WaitForSeconds(delayBeforeMainMenu);
        yield return StartCoroutine(goBedUI.FadeBlackScreen(true, blackScreenSpeedFadeBackMenu));
        CursorController.Instance.ShowCursor();

        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    private GameState CheckFixes()
    {
        if (currentState == GameState.InBed)
        {
            if (eventObjectManager.EventObjectsList.Count <= eventObjectManager.FixedCount)
            {
                return GameState.Win;
            }
            else if (eventObjectManager.EventObjectsList.Count - eventObjectManager.FixedCount <= unfixedEventObjectsCount)
            {
                return GameState.LastChanceTimer;
            }
            else
            {
                return GameState.Lose;
            }
        }
        else if (currentState == GameState.LastChanceTimer)
        {
            if (eventObjectManager.EventObjectsList.Count <= eventObjectManager.FixedCount)
            {
                return GameState.Win;
            }
            else
            {
                return GameState.Lose;
            }
        }

        return currentState;
    }

    public PlayerState GetPlayerState() => playerState;
    public GameState GetCurrentState() => currentState;
    public float GetTimeToBed() => timeToBed;
    public float GetTimeForLastChance() => timeForLastChance;
}
