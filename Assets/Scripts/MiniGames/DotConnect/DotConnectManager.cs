using System;
using UnityEngine;

namespace PotanoCloud.MiniGames.DotConnect
{
    /// <summary>
    /// Owns Dot Connect: timed math unlock, then Flow-style connect with move tracking.
    /// </summary>
    public class DotConnectManager : MonoBehaviour
    {
        #region Serialized Fields
        [SerializeField] private DotConnectLibrary levelLibrary;
        [SerializeField] private MiniGameDifficulty difficulty = MiniGameDifficulty.Hard;
        [SerializeField] private DotPathBoard pathBoard;
        [SerializeField] private bool startOnAwake = true;
        [SerializeField] private bool allowPlaceholderFallback = true;
        #endregion

        #region State
        private DotConnectLevel activeLevel;
        private DotConnectLevel runtimePlaceholder;
        private MathQuestion[] questions;
        private int questionIndex;
        private int questionsSolved;
        private int movementCount;
        private float remainingSeconds;
        private DotConnectPhase phase = DotConnectPhase.MathUnlock;
        private MiniGameOutcome outcome = MiniGameOutcome.Playing;
        private bool isRunning;
        #endregion

        #region Events
        public event Action<int, int, string> HudChanged;
        public event Action<DotConnectPhase> PhaseChanged;
        public event Action<MathQuestion, int, int> MathQuestionChanged;
        public event Action<float> TimeChanged;
        public event Action<int> MovementsChanged;
        public event Action<string> FeedbackChanged;
        public event Action<MiniGameOutcome> RoundEnded;
        #endregion

        #region Properties
        public DotConnectPhase Phase => phase;
        public MiniGameOutcome Outcome => outcome;
        public int MovementCount => movementCount;
        #endregion

        #region Unity Lifecycle
        private void OnEnable()
        {
            if (pathBoard == null)
            {
                return;
            }

            pathBoard.AllPairsConnected += HandleAllPairsConnected;
            pathBoard.PathDragStarted += HandlePathDragStarted;
        }

        private void OnDisable()
        {
            if (pathBoard == null)
            {
                return;
            }

            pathBoard.AllPairsConnected -= HandleAllPairsConnected;
            pathBoard.PathDragStarted -= HandlePathDragStarted;
        }

        private void Start()
        {
            if (startOnAwake)
            {
                StartNewRound();
            }
        }

        private void Update()
        {
            if (!isRunning || outcome != MiniGameOutcome.Playing)
            {
                return;
            }

            if (phase != DotConnectPhase.MathUnlock && phase != DotConnectPhase.Connecting)
            {
                return;
            }

            remainingSeconds = Mathf.Max(0f, remainingSeconds - Time.deltaTime);
            TimeChanged?.Invoke(remainingSeconds);

            if (remainingSeconds <= 0f)
            {
                string msg = phase == DotConnectPhase.MathUnlock
                    ? "Time's up — math unlock failed."
                    : "Time's up — paths incomplete.";
                EndRound(MiniGameOutcome.Lost, msg);
            }
        }
        #endregion

        #region Public Methods
        public void StartNewRound()
        {
            if (pathBoard == null)
            {
                Debug.LogError("[DotConnectManager] Path board is not assigned.", this);
                return;
            }

            ResolveLevel();
            questions = MathQuestionGenerator.BuildSequence(difficulty);
            questionIndex = 0;
            questionsSolved = 0;
            movementCount = 0;
            remainingSeconds = MathQuestionGenerator.GetMathTimeLimit(difficulty);
            outcome = MiniGameOutcome.Playing;
            isRunning = true;

            pathBoard.Build(activeLevel);
            pathBoard.SetInputEnabled(false);
            pathBoard.SetBoardDimmed(true);
            SetPhase(DotConnectPhase.MathUnlock);

            HudChanged?.Invoke(activeLevel.LevelNumber, activeLevel.GridSize, activeLevel.LevelName);
            MovementsChanged?.Invoke(movementCount);
            FeedbackChanged?.Invoke($"Solve {questions.Length} math questions to unlock the board.");
            MathQuestionChanged?.Invoke(questions[0], 1, questions.Length);
            TimeChanged?.Invoke(remainingSeconds);
        }

        public void SubmitMathAnswer(string rawAnswer)
        {
            if (!isRunning || phase != DotConnectPhase.MathUnlock)
            {
                return;
            }

            if (!int.TryParse(rawAnswer?.Trim(), out int answer))
            {
                FeedbackChanged?.Invoke("Enter a whole number.");
                return;
            }

            if (answer != questions[questionIndex].CorrectAnswer)
            {
                FeedbackChanged?.Invoke("Not quite — try again.");
                return;
            }

            questionsSolved++;
            FeedbackChanged?.Invoke("Correct!");
            if (questionsSolved >= questions.Length)
            {
                UnlockBoard();
                return;
            }

            questionIndex++;
            MathQuestionChanged?.Invoke(questions[questionIndex], questionIndex + 1, questions.Length);
        }

        public void ResetBoardPaths()
        {
            if (phase != DotConnectPhase.Connecting || pathBoard == null)
            {
                return;
            }

            pathBoard.ResetPaths();
            FeedbackChanged?.Invoke("Board reset — connect the matching icons again.");
        }

        public void SetDifficultyAndRestart(MiniGameDifficulty nextDifficulty)
        {
            difficulty = nextDifficulty;
            StartNewRound();
        }
        #endregion

        #region Private Methods
        private void ResolveLevel()
        {
            activeLevel = levelLibrary != null ? levelLibrary.GetRandomLevel(difficulty) : null;
            if (activeLevel != null && activeLevel.IsValid(out _))
            {
                return;
            }

            if (!allowPlaceholderFallback)
            {
                Debug.LogError("[DotConnectManager] No valid level.", this);
                return;
            }

            if (runtimePlaceholder != null)
            {
                Destroy(runtimePlaceholder);
            }

            runtimePlaceholder = DotConnectLevelFactory.CreatePlaceholder(difficulty);
            activeLevel = runtimePlaceholder;
        }

        private void UnlockBoard()
        {
            remainingSeconds = activeLevel.ConnectTimeLimitSeconds;
            pathBoard.SetBoardDimmed(false);
            pathBoard.SetInputEnabled(true);
            SetPhase(DotConnectPhase.Connecting);
            FeedbackChanged?.Invoke("Unlocked! Drag matching icons together. Paths cannot overlap.");
            TimeChanged?.Invoke(remainingSeconds);
        }

        private void HandlePathDragStarted()
        {
            if (phase != DotConnectPhase.Connecting)
            {
                return;
            }

            movementCount++;
            MovementsChanged?.Invoke(movementCount);
        }

        private void HandleAllPairsConnected()
        {
            if (phase == DotConnectPhase.Connecting)
            {
                EndRound(MiniGameOutcome.Won, "All icons connected!");
            }
        }

        private void SetPhase(DotConnectPhase next)
        {
            phase = next;
            PhaseChanged?.Invoke(phase);
        }

        private void EndRound(MiniGameOutcome result, string message)
        {
            outcome = result;
            isRunning = false;
            pathBoard.SetInputEnabled(false);
            SetPhase(result == MiniGameOutcome.Won ? DotConnectPhase.Won : DotConnectPhase.Lost);
            FeedbackChanged?.Invoke(message);
            RoundEnded?.Invoke(result);
        }
        #endregion
    }
}
