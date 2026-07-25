using UnityEngine;

namespace PotanoCloud.MiniGames.SimonSays
{
    /// <summary>
    /// Optional hand-authored Simon Says round (word answers). Prefer generated phrases in the manager.
    /// Create via Assets → Create → Potano Cloud → Simon Says Word Set.
    /// </summary>
    [CreateAssetMenu(
        fileName = "SimonSaysWordSet",
        menuName = "Potano Cloud/Mini Games/Simon Says Word Set")]
    public class SimonSaysWordSet : ScriptableObject
    {
        #region Constants
        private const int DefaultMaxMistakes = 4;
        #endregion

        #region Serialized Fields
        [SerializeField] private string displayTitle = "Friendly Phrases";
        [SerializeField] private string targetWord = "KIND";
        [SerializeField] private int maxMistakes = DefaultMaxMistakes;
        [SerializeField] private SimonSaysPrompt[] prompts;
        #endregion

        #region Properties
        /// <summary>Short title for UI.</summary>
        public string DisplayTitle => displayTitle;

        /// <summary>Legacy display word (optional).</summary>
        public string TargetWord => (targetWord ?? string.Empty).Trim().ToUpperInvariant();

        /// <summary>Mistakes allowed before lose.</summary>
        public int MaxMistakes => maxMistakes;

        /// <summary>Riddle prompts for this round.</summary>
        public SimonSaysPrompt[] Prompts => prompts;
        #endregion

        #region Public Methods
        /// <summary>Runtime setup for temporary word sets.</summary>
        public void ConfigureRuntime(string title, string word, int mistakes, SimonSaysPrompt[] roundPrompts)
        {
            displayTitle = title;
            targetWord = word;
            maxMistakes = mistakes;
            prompts = roundPrompts;
        }

        /// <summary>Validates prompts for play.</summary>
        public bool IsValid(out string error)
        {
            if (maxMistakes < 1)
            {
                error = "Max mistakes must be at least 1.";
                return false;
            }

            if (prompts == null || prompts.Length == 0)
            {
                error = "Need at least one prompt.";
                return false;
            }

            for (int i = 0; i < prompts.Length; i++)
            {
                if (prompts[i] == null || string.IsNullOrWhiteSpace(prompts[i].AnswerWord))
                {
                    error = $"Prompt {i} is missing an answer word.";
                    return false;
                }
            }

            error = string.Empty;
            return true;
        }
        #endregion
    }
}
