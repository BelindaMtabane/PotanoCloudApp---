using System;
using System.Collections.Generic;
using UnityEngine;

namespace PotanoCloud.MiniGames.SimonSays
{
    /// <summary>
    /// Time-based Simon Says: generated friendly-phrase riddles; type the word only when Simon says.
    /// </summary>
    public class SimonSaysManager : MonoBehaviour
    {
        #region Constants
        private const string SimonSaysPrefix = "Simon says: ";
        private const string TrapHint = " (Don't type — Simon didn't say!)";
        private const int ExtraTrapPrompts = 2;
        #endregion

        #region Serialized Fields
        [SerializeField] private MiniGameDifficulty difficulty = MiniGameDifficulty.Easy;
        [SerializeField] private bool startOnAwake = true;
        [Tooltip("Optional hand-authored prompts. If empty, friendly phrases are generated.")]
        [SerializeField] private SimonSaysLibrary library;
        [SerializeField] private bool preferGeneratedPhrases = true;
        #endregion

        #region State
        private readonly List<SimonSaysPrompt> promptQueue = new List<SimonSaysPrompt>();
        private readonly SimonSaysTypingBuffer typingBuffer = new SimonSaysTypingBuffer();
        private int promptIndex;
        private int mistakeCount;
        private int maxMistakes;
        private int successCount;
        private int winsNeeded;
        private float promptTimeLimit;
        private float remainingSeconds;
        private MiniGameOutcome outcome = MiniGameOutcome.Playing;
        private bool isRunning;
        private bool waitingForInput;
        #endregion

        #region Events
        public event Action<string> PromptChanged;
        public event Action<string> TypedTextChanged;
        public event Action<float> TimeChanged;
        public event Action<int, int> MistakesChanged;
        public event Action<int, int> ProgressChanged;
        public event Action<string> FeedbackChanged;
        public event Action<string> RoundStarted;
        public event Action<MiniGameOutcome> RoundEnded;
        #endregion

        #region Properties
        public MiniGameOutcome Outcome => outcome;
        #endregion

        #region Unity Lifecycle
        private void OnEnable()
        {
            typingBuffer.TextChanged += HandleTypedChanged;
        }

        private void OnDisable()
        {
            typingBuffer.TextChanged -= HandleTypedChanged;
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
            if (!isRunning || outcome != MiniGameOutcome.Playing || !waitingForInput)
            {
                return;
            }

            remainingSeconds -= Time.deltaTime;
            remainingSeconds = Mathf.Max(0f, remainingSeconds);
            TimeChanged?.Invoke(remainingSeconds);

            if (remainingSeconds <= 0f)
            {
                HandlePromptTimeout();
                return;
            }

            ReadKeyboard();
        }
        #endregion

        #region Public Methods
        /// <summary>Starts a new timed, generated (or library) round.</summary>
        public void StartNewRound()
        {
            maxMistakes = FriendlyPhraseGenerator.GetMaxMistakes(difficulty);
            winsNeeded = FriendlyPhraseGenerator.GetWinsNeeded(difficulty);
            promptTimeLimit = FriendlyPhraseGenerator.GetPromptTimeSeconds(difficulty);

            promptQueue.Clear();
            promptQueue.AddRange(FriendlyPhraseGenerator.BuildPromptQueue(
                preferGeneratedPhrases,
                library,
                winsNeeded,
                ExtraTrapPrompts,
                out int mistakeOverride));
            if (mistakeOverride > 0)
            {
                maxMistakes = mistakeOverride;
            }

            promptIndex = 0;
            mistakeCount = 0;
            successCount = 0;
            typingBuffer.Clear();
            outcome = MiniGameOutcome.Playing;
            isRunning = true;
            waitingForInput = true;

            RoundStarted?.Invoke($"Simon Says — {difficulty}");
            MistakesChanged?.Invoke(mistakeCount, maxMistakes);
            ProgressChanged?.Invoke(successCount, winsNeeded);
            FeedbackChanged?.Invoke("Type the friendly word. Only when Simon says!");
            ShowCurrentPrompt();
        }
        #endregion

        #region Private Methods
        private void HandleTypedChanged(string text)
        {
            TypedTextChanged?.Invoke(text);
        }

        private void ShowCurrentPrompt()
        {
            if (successCount >= winsNeeded || promptIndex >= promptQueue.Count)
            {
                EndRound(successCount >= winsNeeded ? MiniGameOutcome.Won : MiniGameOutcome.Lost);
                return;
            }

            typingBuffer.Clear();
            remainingSeconds = promptTimeLimit;
            TimeChanged?.Invoke(remainingSeconds);

            SimonSaysPrompt prompt = promptQueue[promptIndex];
            string command = prompt.SimonSays
                ? SimonSaysPrefix + prompt.RiddleText
                : prompt.RiddleText + TrapHint;

            PromptChanged?.Invoke(command);
            waitingForInput = true;
        }

        private void ReadKeyboard()
        {
            string input = Input.inputString;
            if (string.IsNullOrEmpty(input))
            {
                return;
            }

            SimonSaysPrompt prompt = promptQueue[promptIndex];
            if (!prompt.SimonSays && SimonSaysTypingBuffer.ContainsLetter(input))
            {
                RegisterMistake("Simon didn't say — that counts as a mistake!");
                AdvancePrompt();
                return;
            }

            bool submit = typingBuffer.ProcessInputString(input, out _, prompt.AnswerWord.Length);
            if (submit)
            {
                SubmitTypedWord();
            }
        }

        private void SubmitTypedWord()
        {
            if (!waitingForInput || outcome != MiniGameOutcome.Playing)
            {
                return;
            }

            waitingForInput = false;
            SimonSaysPrompt prompt = promptQueue[promptIndex];

            if (!prompt.SimonSays)
            {
                RegisterMistake("Simon didn't say — that counts as a mistake!");
                AdvancePrompt();
                return;
            }

            if (typingBuffer.Text == prompt.AnswerWord)
            {
                successCount++;
                ProgressChanged?.Invoke(successCount, winsNeeded);
                FeedbackChanged?.Invoke($"Yes! '{prompt.AnswerWord}' fits the phrase.");
            }
            else
            {
                RegisterMistake($"Not quite — the phrase needed '{prompt.AnswerWord}'.");
            }

            AdvancePrompt();
        }

        private void HandlePromptTimeout()
        {
            waitingForInput = false;
            SimonSaysPrompt prompt = promptQueue[promptIndex];

            if (!prompt.SimonSays)
            {
                FeedbackChanged?.Invoke("Good patience — Simon didn't say.");
            }
            else
            {
                RegisterMistake("Time's up — the phrase needed a word in time.");
            }

            AdvancePrompt();
        }

        private void RegisterMistake(string message)
        {
            mistakeCount++;
            MistakesChanged?.Invoke(mistakeCount, maxMistakes);
            FeedbackChanged?.Invoke(message);

            if (mistakeCount >= maxMistakes)
            {
                EndRound(MiniGameOutcome.Lost);
            }
        }

        private void AdvancePrompt()
        {
            if (outcome != MiniGameOutcome.Playing)
            {
                return;
            }

            promptIndex++;
            ShowCurrentPrompt();
        }

        private void EndRound(MiniGameOutcome result)
        {
            outcome = result;
            isRunning = false;
            waitingForInput = false;
            RoundEnded?.Invoke(result);
        }
        #endregion
    }
}
