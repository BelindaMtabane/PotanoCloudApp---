using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PotanoCloud.MiniGames.DotConnect
{
    /// <summary>
    /// Flow-reference HUD: Level, Time, Movements, grid size, math panel, footer actions.
    /// </summary>
    public class DotConnectUI : MonoBehaviour
    {
        #region Constants
        private const string LevelFormat = "Level {0}";
        private const string TimeFormat = "Time : {0:0}";
        private const string MovementsFormat = "Movements : {0}";
        private const string GridFormat = "{0}x{0}";
        private const string ProgressFormat = "Question {0}/{1}";
        private const string WonMessage = "Level complete!";
        private const string LostMessage = "Try again!";
        #endregion

        #region Serialized Fields
        [Header("Manager")]
        [SerializeField] private DotConnectManager gameManager;

        [Header("Flow HUD")]
        [SerializeField] private TextMeshProUGUI levelLabel;
        [SerializeField] private TextMeshProUGUI timeLabel;
        [SerializeField] private TextMeshProUGUI movementsLabel;
        [SerializeField] private TextMeshProUGUI gridSizeLabel;
        [SerializeField] private TextMeshProUGUI feedbackLabel;
        [SerializeField] private TextMeshProUGUI resultLabel;
        [SerializeField] private GameObject resultPanel;

        [Header("Math Unlock")]
        [SerializeField] private GameObject mathPanel;
        [SerializeField] private TextMeshProUGUI questionLabel;
        [SerializeField] private TextMeshProUGUI progressLabel;
        [SerializeField] private TMP_InputField answerInput;
        [SerializeField] private Button submitAnswerButton;

        [Header("Footer (optional)")]
        [SerializeField] private Button previousButton;
        [SerializeField] private Button homeButton;
        [SerializeField] private Button menuButton;
        [SerializeField] private Button resetButton;
        [SerializeField] private Button nextButton;
        [SerializeField] private string homeSceneName = "OpenPager";
        #endregion

        #region Unity Lifecycle
        private void OnEnable()
        {
            if (gameManager != null)
            {
                gameManager.HudChanged += HandleHudChanged;
                gameManager.PhaseChanged += HandlePhaseChanged;
                gameManager.MathQuestionChanged += HandleQuestionChanged;
                gameManager.TimeChanged += HandleTimeChanged;
                gameManager.MovementsChanged += HandleMovementsChanged;
                gameManager.FeedbackChanged += HandleFeedback;
                gameManager.RoundEnded += HandleRoundEnded;
            }

            BindButton(submitAnswerButton, SubmitAnswer);
            BindButton(resetButton, HandleResetClicked);
            BindButton(previousButton, HandlePreviousClicked);
            BindButton(nextButton, HandleNextClicked);
            BindButton(homeButton, LoadHome);
            BindButton(menuButton, HandleMenuClicked);

            if (answerInput != null)
            {
                answerInput.onSubmit.AddListener(OnInputSubmit);
            }
        }

        private void OnDisable()
        {
            if (gameManager != null)
            {
                gameManager.HudChanged -= HandleHudChanged;
                gameManager.PhaseChanged -= HandlePhaseChanged;
                gameManager.MathQuestionChanged -= HandleQuestionChanged;
                gameManager.TimeChanged -= HandleTimeChanged;
                gameManager.MovementsChanged -= HandleMovementsChanged;
                gameManager.FeedbackChanged -= HandleFeedback;
                gameManager.RoundEnded -= HandleRoundEnded;
            }

            UnbindButton(submitAnswerButton, SubmitAnswer);
            UnbindButton(resetButton, HandleResetClicked);
            UnbindButton(previousButton, HandlePreviousClicked);
            UnbindButton(nextButton, HandleNextClicked);
            UnbindButton(homeButton, LoadHome);
            UnbindButton(menuButton, HandleMenuClicked);

            if (answerInput != null)
            {
                answerInput.onSubmit.RemoveListener(OnInputSubmit);
            }
        }

        private void Start()
        {
            if (resultPanel != null)
            {
                resultPanel.SetActive(false);
            }
        }
        #endregion

        #region Event Handlers
        private void HandleHudChanged(int levelNumber, int gridSize, string _)
        {
            if (levelLabel != null)
            {
                levelLabel.text = string.Format(LevelFormat, levelNumber);
            }

            if (gridSizeLabel != null)
            {
                gridSizeLabel.text = string.Format(GridFormat, gridSize);
            }

            if (resultPanel != null)
            {
                resultPanel.SetActive(false);
            }
        }

        private void HandlePhaseChanged(DotConnectPhase phase)
        {
            bool math = phase == DotConnectPhase.MathUnlock;
            if (mathPanel != null)
            {
                mathPanel.SetActive(math);
            }
        }

        private void HandleQuestionChanged(MathQuestion question, int index, int total)
        {
            if (questionLabel != null)
            {
                questionLabel.text = question.PromptText;
            }

            if (progressLabel != null)
            {
                progressLabel.text = string.Format(ProgressFormat, index, total);
            }

            if (answerInput != null)
            {
                answerInput.text = string.Empty;
                answerInput.ActivateInputField();
            }
        }

        private void HandleTimeChanged(float seconds)
        {
            if (timeLabel != null)
            {
                timeLabel.text = string.Format(TimeFormat, seconds);
            }
        }

        private void HandleMovementsChanged(int movements)
        {
            if (movementsLabel != null)
            {
                movementsLabel.text = string.Format(MovementsFormat, movements);
            }
        }

        private void HandleFeedback(string message)
        {
            if (feedbackLabel != null)
            {
                feedbackLabel.text = message ?? string.Empty;
            }
        }

        private void HandleRoundEnded(MiniGameOutcome outcome)
        {
            if (resultPanel != null)
            {
                resultPanel.SetActive(true);
            }

            if (resultLabel != null)
            {
                resultLabel.text = outcome == MiniGameOutcome.Won ? WonMessage : LostMessage;
            }

            if (mathPanel != null)
            {
                mathPanel.SetActive(false);
            }
        }
        #endregion

        #region Input
        private void OnInputSubmit(string _) => SubmitAnswer();

        private void SubmitAnswer()
        {
            if (gameManager == null || answerInput == null)
            {
                return;
            }

            gameManager.SubmitMathAnswer(answerInput.text);
        }

        private void HandleResetClicked() => gameManager?.ResetBoardPaths();

        private void HandlePreviousClicked() =>
            gameManager?.SetDifficultyAndRestart(MiniGameDifficulty.Easy);

        private void HandleNextClicked() =>
            gameManager?.SetDifficultyAndRestart(MiniGameDifficulty.Hard);

        private void HandleMenuClicked() => gameManager?.StartNewRound();

        private void LoadHome()
        {
            if (string.IsNullOrEmpty(homeSceneName))
            {
                return;
            }

            UnityEngine.SceneManagement.SceneManager.LoadScene(homeSceneName);
        }

        private static void BindButton(Button button, UnityEngine.Events.UnityAction action)
        {
            if (button != null)
            {
                button.onClick.AddListener(action);
            }
        }

        private static void UnbindButton(Button button, UnityEngine.Events.UnityAction action)
        {
            if (button != null)
            {
                button.onClick.RemoveListener(action);
            }
        }
        #endregion
    }
}
