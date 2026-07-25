using System;
using UnityEngine;

namespace PotanoCloud.MiniGames.DotConnect
{
    /// <summary>
    /// One algebra / arithmetic question with a numeric answer.
    /// </summary>
    [Serializable]
    public class MathQuestion
    {
        #region Serialized Fields
        [SerializeField] private string promptText = "2 + 2 = ?";
        [SerializeField] private int correctAnswer = 4;
        #endregion

        #region Properties
        /// <summary>Question shown to the player.</summary>
        public string PromptText => promptText;

        /// <summary>Expected integer answer.</summary>
        public int CorrectAnswer => correctAnswer;
        #endregion

        #region Constructors
        public MathQuestion()
        {
        }

        public MathQuestion(string prompt, int answer)
        {
            promptText = prompt;
            correctAnswer = answer;
        }
        #endregion
    }
}
