using UnityEngine;

namespace PotanoCloud.MiniGames.DotConnect
{
    /// <summary>
    /// Builds math questions from hard algebra down to simple arithmetic.
    /// Question count: Easy=2, Medium=3, Hard=4.
    /// </summary>
    public static class MathQuestionGenerator
    {
        #region Constants
        private const int EasyQuestionCount = 2;
        private const int MediumQuestionCount = 3;
        private const int HardQuestionCount = 4;
        private const float EasyMathSeconds = 60f;
        private const float MediumMathSeconds = 75f;
        private const float HardMathSeconds = 90f;
        #endregion

        #region Public Methods
        /// <summary>How many questions must be solved to unlock the board.</summary>
        public static int GetQuestionCount(MiniGameDifficulty difficulty)
        {
            switch (difficulty)
            {
                case MiniGameDifficulty.Medium:
                    return MediumQuestionCount;
                case MiniGameDifficulty.Hard:
                    return HardQuestionCount;
                default:
                    return EasyQuestionCount;
            }
        }

        /// <summary>Seconds allowed for the full math unlock phase.</summary>
        public static float GetMathTimeLimit(MiniGameDifficulty difficulty)
        {
            switch (difficulty)
            {
                case MiniGameDifficulty.Medium:
                    return MediumMathSeconds;
                case MiniGameDifficulty.Hard:
                    return HardMathSeconds;
                default:
                    return EasyMathSeconds;
            }
        }

        /// <summary>
        /// Builds a sequence that starts harder and ends simpler within the round.
        /// </summary>
        public static MathQuestion[] BuildSequence(MiniGameDifficulty difficulty)
        {
            int count = GetQuestionCount(difficulty);
            MathQuestion[] questions = new MathQuestion[count];

            for (int i = 0; i < count; i++)
            {
                // index 0 = hardest in this round, last = simplest
                float hardness = 1f - (i / (float)Mathf.Max(1, count - 1));
                questions[i] = CreateQuestion(difficulty, hardness);
            }

            return questions;
        }
        #endregion

        #region Private Methods
        private static MathQuestion CreateQuestion(MiniGameDifficulty difficulty, float hardness)
        {
            // Blend game difficulty with in-round hardness (1 = hard, 0 = easy)
            float score = ((int)difficulty / 2f) * 0.5f + hardness * 0.5f;

            if (score >= 0.75f)
            {
                return CreateHardAlgebra();
            }

            if (score >= 0.4f)
            {
                return CreateMediumAlgebra();
            }

            return CreateSimpleArithmetic();
        }

        private static MathQuestion CreateSimpleArithmetic()
        {
            int mode = UnityEngine.Random.Range(0, 3);
            int a = UnityEngine.Random.Range(2, 12);
            int b = UnityEngine.Random.Range(2, 12);

            switch (mode)
            {
                case 0:
                    return new MathQuestion($"{a} + {b} = ?", a + b);
                case 1:
                    int sum = a + b;
                    return new MathQuestion($"{sum} - {a} = ?", b);
                default:
                    int m = UnityEngine.Random.Range(2, 9);
                    int n = UnityEngine.Random.Range(2, 9);
                    return new MathQuestion($"{m} × {n} = ?", m * n);
            }
        }

        private static MathQuestion CreateMediumAlgebra()
        {
            // Solve: ax + b = c  →  x = (c - b) / a
            int a = UnityEngine.Random.Range(2, 6);
            int x = UnityEngine.Random.Range(2, 10);
            int b = UnityEngine.Random.Range(1, 9);
            int c = a * x + b;
            return new MathQuestion($"{a}x + {b} = {c}. Find x", x);
        }

        private static MathQuestion CreateHardAlgebra()
        {
            // Solve: a(x - b) = c  →  x = c/a + b
            int a = UnityEngine.Random.Range(2, 5);
            int b = UnityEngine.Random.Range(1, 6);
            int x = UnityEngine.Random.Range(3, 12);
            int c = a * (x - b);
            return new MathQuestion($"{a}(x − {b}) = {c}. Find x", x);
        }
        #endregion
    }
}
