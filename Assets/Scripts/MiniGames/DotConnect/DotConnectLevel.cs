using UnityEngine;

namespace PotanoCloud.MiniGames.DotConnect
{
    /// <summary>
    /// One Dot Connect level: grid size, colored pairs, and connect-phase timer.
    /// Math unlock count/time comes from <see cref="MathQuestionGenerator"/> + difficulty.
    /// </summary>
    [CreateAssetMenu(
        fileName = "DotConnectLevel",
        menuName = "Potano Cloud/Mini Games/Dot Connect Level")]
    public class DotConnectLevel : ScriptableObject
    {
        #region Constants
        private const int DefaultGridSize = 9;
        private const int DefaultLevelNumber = 1;
        private const float DefaultConnectSeconds = 180f;
        #endregion

        #region Serialized Fields
        [SerializeField] private string levelName = "Level 1";
        [SerializeField] private int levelNumber = DefaultLevelNumber;
        [SerializeField] private MiniGameDifficulty difficulty = MiniGameDifficulty.Easy;
        [SerializeField] private int gridSize = DefaultGridSize;
        [SerializeField] private float connectTimeLimitSeconds = DefaultConnectSeconds;
        [SerializeField] private ColorDotPair[] colorPairs;
        #endregion

        #region Properties
        public string LevelName => levelName;
        public int LevelNumber => levelNumber;
        public MiniGameDifficulty Difficulty => difficulty;
        public int GridSize => gridSize;
        public float ConnectTimeLimitSeconds => connectTimeLimitSeconds;
        public ColorDotPair[] ColorPairs => colorPairs;
        #endregion

        #region Public Methods
        public void ConfigureRuntime(
            string name,
            MiniGameDifficulty gameDifficulty,
            int number,
            int size,
            float connectSeconds,
            ColorDotPair[] pairs)
        {
            levelName = name;
            levelNumber = number;
            difficulty = gameDifficulty;
            gridSize = size;
            connectTimeLimitSeconds = connectSeconds;
            colorPairs = pairs;
        }

        public bool IsValid(out string error)
        {
            if (gridSize < 4 || gridSize > 9)
            {
                error = "Grid size must be between 4 and 9.";
                return false;
            }

            if (colorPairs == null || colorPairs.Length < 2)
            {
                error = "Need at least 2 color pairs.";
                return false;
            }

            if (connectTimeLimitSeconds <= 0f)
            {
                error = "Connect time must be greater than zero.";
                return false;
            }

            for (int i = 0; i < colorPairs.Length; i++)
            {
                ColorDotPair pair = colorPairs[i];
                if (pair == null)
                {
                    error = $"Color pair {i} is null.";
                    return false;
                }

                if (!IsInside(pair.StartCell) || !IsInside(pair.EndCell))
                {
                    error = $"Color pair {i} has a cell outside the grid.";
                    return false;
                }

                if (pair.StartCell == pair.EndCell)
                {
                    error = $"Color pair {i} start and end are the same cell.";
                    return false;
                }
            }

            error = string.Empty;
            return true;
        }

        private bool IsInside(Vector2Int cell)
        {
            return cell.x >= 0 && cell.y >= 0 && cell.x < gridSize && cell.y < gridSize;
        }
        #endregion
    }
}
