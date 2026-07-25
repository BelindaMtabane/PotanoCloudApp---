using System;
using UnityEngine;

namespace PotanoCloud.MiniGames.SimonSays
{
    /// <summary>
    /// One friendly-phrase riddle. Player types the full answer word when Simon says.
    /// </summary>
    [Serializable]
    public class SimonSaysPrompt
    {
        #region Serialized Fields
        [Tooltip("Riddle / unfinished phrase shown to the player (without Simon Says prefix).")]
        [SerializeField] private string riddleText = "On a road when Simon looks left, he looks ....";

        [Tooltip("Friendly word the player must type (letters only).")]
        [SerializeField] private string answerWord = "RIGHT";

        [Tooltip("If true, command starts with Simon says and the player should type. If false, typing is a mistake.")]
        [SerializeField] private bool simonSays = true;
        #endregion

        #region Properties
        /// <summary>Riddle body without prefix.</summary>
        public string RiddleText => riddleText;

        /// <summary>Expected answer word, uppercased.</summary>
        public string AnswerWord => (answerWord ?? string.Empty).Trim().ToUpperInvariant();

        /// <summary>Whether this command includes Simon Says.</summary>
        public bool SimonSays => simonSays;
        #endregion

        #region Constructors
        /// <summary>Empty prompt for serialization.</summary>
        public SimonSaysPrompt()
        {
        }

        /// <summary>Creates a runtime prompt.</summary>
        public SimonSaysPrompt(string riddle, string word, bool requiresSimonSays)
        {
            riddleText = riddle;
            answerWord = word;
            simonSays = requiresSimonSays;
        }
        #endregion
    }
}
