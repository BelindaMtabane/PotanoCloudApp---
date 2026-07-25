using System.Collections.Generic;
using UnityEngine;

namespace PotanoCloud.MiniGames.SimonSays
{
    /// <summary>
    /// Collection of Simon Says word sets for random round selection.
    /// Create via Assets → Create → Potano Cloud → Simon Says Library.
    /// </summary>
    [CreateAssetMenu(
        fileName = "SimonSaysLibrary",
        menuName = "Potano Cloud/Mini Games/Simon Says Library")]
    public class SimonSaysLibrary : ScriptableObject
    {
        #region Serialized Fields
        [SerializeField] private SimonSaysWordSet[] wordSets;
        #endregion

        #region Public Methods
        /// <summary>
        /// Returns a random valid word set, or null if none are valid.
        /// </summary>
        public SimonSaysWordSet GetRandomWordSet()
        {
            List<SimonSaysWordSet> valid = new List<SimonSaysWordSet>();
            if (wordSets == null)
            {
                return null;
            }

            for (int i = 0; i < wordSets.Length; i++)
            {
                SimonSaysWordSet set = wordSets[i];
                if (set != null && set.IsValid(out _))
                {
                    valid.Add(set);
                }
            }

            if (valid.Count == 0)
            {
                Debug.LogWarning("[SimonSaysLibrary] No valid word sets.", this);
                return null;
            }

            return valid[Random.Range(0, valid.Count)];
        }
        #endregion
    }
}
