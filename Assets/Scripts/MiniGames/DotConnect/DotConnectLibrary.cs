using System.Collections.Generic;
using UnityEngine;

namespace PotanoCloud.MiniGames.DotConnect
{
    /// <summary>
    /// Pools of Dot Connect levels by difficulty.
    /// </summary>
    [CreateAssetMenu(
        fileName = "DotConnectLibrary",
        menuName = "Potano Cloud/Mini Games/Dot Connect Library")]
    public class DotConnectLibrary : ScriptableObject
    {
        #region Serialized Fields
        [SerializeField] private DotConnectLevel[] easyLevels;
        [SerializeField] private DotConnectLevel[] mediumLevels;
        [SerializeField] private DotConnectLevel[] hardLevels;
        #endregion

        #region Public Methods
        public DotConnectLevel GetRandomLevel(MiniGameDifficulty difficulty)
        {
            List<DotConnectLevel> valid = CollectValid(GetPool(difficulty));
            if (valid.Count == 0)
            {
                Debug.LogWarning($"[DotConnectLibrary] No valid levels for {difficulty}.", this);
                return null;
            }

            return valid[Random.Range(0, valid.Count)];
        }

        public void ConfigureRuntime(
            DotConnectLevel[] easy,
            DotConnectLevel[] medium,
            DotConnectLevel[] hard)
        {
            easyLevels = easy;
            mediumLevels = medium;
            hardLevels = hard;
        }
        #endregion

        #region Private Methods
        private DotConnectLevel[] GetPool(MiniGameDifficulty difficulty)
        {
            switch (difficulty)
            {
                case MiniGameDifficulty.Medium:
                    return mediumLevels;
                case MiniGameDifficulty.Hard:
                    return hardLevels;
                default:
                    return easyLevels;
            }
        }

        private static List<DotConnectLevel> CollectValid(DotConnectLevel[] pool)
        {
            List<DotConnectLevel> valid = new List<DotConnectLevel>();
            if (pool == null)
            {
                return valid;
            }

            for (int i = 0; i < pool.Length; i++)
            {
                if (pool[i] != null && pool[i].IsValid(out _))
                {
                    valid.Add(pool[i]);
                }
            }

            return valid;
        }
        #endregion
    }
}
