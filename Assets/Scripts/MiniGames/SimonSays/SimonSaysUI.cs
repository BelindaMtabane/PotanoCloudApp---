using TMPro;
using UnityEngine;

namespace PotanoCloud.MiniGames.SimonSays
{
    /// <summary>
    /// Binds timed Simon Says events to TextMeshPro UI.
    /// </summary>
    public class SimonSaysUI : MonoBehaviour
    {
        #region Constants
        private const string MistakesFormat = "Mistakes: {0}/{1}";
        private const string ProgressFormat = "Phrases: {0}/{1}";
        private const string TimeFormat = "Time: {0:0.0}s";
        private const string TypedFormat = "> {0}";
        private const string WonMessage = "Friendly phrases complete — you win!";
        private const string LostMessage = "Round over — try again!";
        #endregion

        #region Serialized Fields
        [SerializeField] private SimonSaysManager simonSaysManager;
        [SerializeField] private TextMeshProUGUI titleLabel;
        [SerializeField] private TextMeshProUGUI promptLabel;
        [SerializeField] private TextMeshProUGUI typedLabel;
        [SerializeField] private TextMeshProUGUI timeLabel;
        [SerializeField] private TextMeshProUGUI mistakesLabel;
        [SerializeField] private TextMeshProUGUI progressLabel;
        [SerializeField] private TextMeshProUGUI feedbackLabel;
        [SerializeField] private TextMeshProUGUI resultLabel;
        [SerializeField] private GameObject resultPanel;
        #endregion

        #region Unity Lifecycle
        private void OnEnable()
        {
            if (simonSaysManager == null)
            {
                return;
            }

            simonSaysManager.RoundStarted += HandleRoundStarted;
            simonSaysManager.PromptChanged += HandlePromptChanged;
            simonSaysManager.TypedTextChanged += HandleTypedChanged;
            simonSaysManager.TimeChanged += HandleTimeChanged;
            simonSaysManager.MistakesChanged += HandleMistakesChanged;
            simonSaysManager.ProgressChanged += HandleProgressChanged;
            simonSaysManager.FeedbackChanged += HandleFeedbackChanged;
            simonSaysManager.RoundEnded += HandleRoundEnded;
        }

        private void OnDisable()
        {
            if (simonSaysManager == null)
            {
                return;
            }

            simonSaysManager.RoundStarted -= HandleRoundStarted;
            simonSaysManager.PromptChanged -= HandlePromptChanged;
            simonSaysManager.TypedTextChanged -= HandleTypedChanged;
            simonSaysManager.TimeChanged -= HandleTimeChanged;
            simonSaysManager.MistakesChanged -= HandleMistakesChanged;
            simonSaysManager.ProgressChanged -= HandleProgressChanged;
            simonSaysManager.FeedbackChanged -= HandleFeedbackChanged;
            simonSaysManager.RoundEnded -= HandleRoundEnded;
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
        private void HandleRoundStarted(string title)
        {
            if (resultPanel != null)
            {
                resultPanel.SetActive(false);
            }

            if (titleLabel != null)
            {
                titleLabel.text = title;
            }

            if (resultLabel != null)
            {
                resultLabel.text = string.Empty;
            }
        }

        private void HandlePromptChanged(string prompt)
        {
            if (promptLabel != null)
            {
                promptLabel.text = prompt;
            }
        }

        private void HandleTypedChanged(string typed)
        {
            if (typedLabel != null)
            {
                typedLabel.text = string.Format(TypedFormat, typed);
            }
        }

        private void HandleTimeChanged(float remaining)
        {
            if (timeLabel != null)
            {
                timeLabel.text = string.Format(TimeFormat, remaining);
            }
        }

        private void HandleMistakesChanged(int used, int max)
        {
            if (mistakesLabel != null)
            {
                mistakesLabel.text = string.Format(MistakesFormat, used, max);
            }
        }

        private void HandleProgressChanged(int success, int needed)
        {
            if (progressLabel != null)
            {
                progressLabel.text = string.Format(ProgressFormat, success, needed);
            }
        }

        private void HandleFeedbackChanged(string feedback)
        {
            if (feedbackLabel != null)
            {
                feedbackLabel.text = feedback ?? string.Empty;
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
        }
        #endregion
    }
}
