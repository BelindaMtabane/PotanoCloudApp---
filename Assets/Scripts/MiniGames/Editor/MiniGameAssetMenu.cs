using UnityEditor;
using UnityEngine;
using PotanoCloud.MiniGames;
using PotanoCloud.MiniGames.DotConnect;
using PotanoCloud.MiniGames.SimonSays;
using PotanoCloud.MiniGames.SpaceCar;

namespace PotanoCloud.MiniGames.Editor
{
    /// <summary>
    /// Creates starter ScriptableObject assets for Potano Cloud mini-games.
    /// </summary>
    public static class MiniGameAssetMenu
    {
        private const string DataRoot = "Assets/Data/MiniGames";

        [MenuItem("Potano Cloud/Mini Games/Create Starter Data Assets")]
        public static void CreateStarterAssets()
        {
            EnsureFolder("Assets/Data");
            EnsureFolder(DataRoot);
            EnsureFolder(DataRoot + "/DotConnect");
            EnsureFolder(DataRoot + "/SimonSays");
            EnsureFolder(DataRoot + "/SpaceCar");

            CreateDotConnectShells();
            CreateSimonSaysSample();
            CreateSpaceCarConfigs();

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("[Potano Cloud] Starter mini-game data created under Assets/Data/MiniGames.");
        }

        private static void CreateDotConnectShells()
        {
            DotConnectLevel easy = CreateAsset<DotConnectLevel>(DataRoot + "/DotConnect/Level_Easy_CloudPaths.asset");
            DotConnectLevel medium = CreateAsset<DotConnectLevel>(DataRoot + "/DotConnect/Level_Medium_SkyPaths.asset");
            DotConnectLevel hard = CreateAsset<DotConnectLevel>(DataRoot + "/DotConnect/Level_Hard_StormPaths.asset");

            ApplyPlaceholder(easy, MiniGameDifficulty.Easy);
            ApplyPlaceholder(medium, MiniGameDifficulty.Medium);
            ApplyPlaceholder(hard, MiniGameDifficulty.Hard);

            DotConnectLibrary library = CreateAsset<DotConnectLibrary>(DataRoot + "/DotConnect/DotConnectLibrary.asset");
            SerializedObject so = new SerializedObject(library);
            AssignArray(so, "easyLevels", easy);
            AssignArray(so, "mediumLevels", medium);
            AssignArray(so, "hardLevels", hard);
            so.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(library);
        }

        private static void ApplyPlaceholder(DotConnectLevel target, MiniGameDifficulty difficulty)
        {
            DotConnectLevel temp = DotConnectLevelFactory.CreatePlaceholder(difficulty);
            target.ConfigureRuntime(
                temp.LevelName,
                temp.Difficulty,
                temp.LevelNumber,
                temp.GridSize,
                temp.ConnectTimeLimitSeconds,
                temp.ColorPairs);
            EditorUtility.SetDirty(target);
            Object.DestroyImmediate(temp);
        }

        private static void CreateSimonSaysSample()
        {
            SimonSaysWordSet sample = CreateAsset<SimonSaysWordSet>(
                DataRoot + "/SimonSays/WordSet_FriendlyPhrases.asset");

            SerializedObject wordSo = new SerializedObject(sample);
            wordSo.FindProperty("displayTitle").stringValue = "Friendly Phrases";
            wordSo.FindProperty("targetWord").stringValue = "KIND";
            wordSo.FindProperty("maxMistakes").intValue = 4;

            SerializedProperty prompts = wordSo.FindProperty("prompts");
            prompts.arraySize = 4;
            SetWordPrompt(prompts.GetArrayElementAtIndex(0),
                "On a road when Simon looks left, he looks ....", "RIGHT", true);
            SetWordPrompt(prompts.GetArrayElementAtIndex(1),
                "When Simon waves to a friend, he says ....", "HELLO", true);
            SetWordPrompt(prompts.GetArrayElementAtIndex(2),
                "Type SMILE for a happy face.", "SMILE", false);
            SetWordPrompt(prompts.GetArrayElementAtIndex(3),
                "When you share your snack, you are being ....", "KIND", true);
            wordSo.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(sample);

            SimonSaysLibrary library = CreateAsset<SimonSaysLibrary>(
                DataRoot + "/SimonSays/SimonSaysLibrary.asset");
            SerializedObject libSo = new SerializedObject(library);
            AssignArray(libSo, "wordSets", sample);
            libSo.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(library);
        }

        private static void CreateSpaceCarConfigs()
        {
            CreateSpaceCarConfig(DataRoot + "/SpaceCar/SpaceCar_Easy.asset", MiniGameDifficulty.Easy);
            CreateSpaceCarConfig(DataRoot + "/SpaceCar/SpaceCar_Medium.asset", MiniGameDifficulty.Medium);
            CreateSpaceCarConfig(DataRoot + "/SpaceCar/SpaceCar_Hard.asset", MiniGameDifficulty.Hard);
        }

        private static void CreateSpaceCarConfig(string path, MiniGameDifficulty difficulty)
        {
            SpaceCarConfig config = CreateAsset<SpaceCarConfig>(path);
            config.ConfigureRuntime(difficulty, 3f, 1.5f, 0.35f, 60f, 8);
            EditorUtility.SetDirty(config);
        }

        private static void SetWordPrompt(SerializedProperty element, string riddle, string word, bool simonSays)
        {
            element.FindPropertyRelative("riddleText").stringValue = riddle;
            element.FindPropertyRelative("answerWord").stringValue = word;
            element.FindPropertyRelative("simonSays").boolValue = simonSays;
        }

        private static void AssignArray(SerializedObject so, string propertyName, Object value)
        {
            SerializedProperty property = so.FindProperty(propertyName);
            property.arraySize = 1;
            property.GetArrayElementAtIndex(0).objectReferenceValue = value;
        }

        private static T CreateAsset<T>(string path) where T : ScriptableObject
        {
            T existing = AssetDatabase.LoadAssetAtPath<T>(path);
            if (existing != null)
            {
                return existing;
            }

            T asset = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(asset, path);
            return asset;
        }

        private static void EnsureFolder(string path)
        {
            if (AssetDatabase.IsValidFolder(path))
            {
                return;
            }

            string parent = System.IO.Path.GetDirectoryName(path)?.Replace("\\", "/");
            string name = System.IO.Path.GetFileName(path);
            if (!string.IsNullOrEmpty(parent) && !AssetDatabase.IsValidFolder(parent))
            {
                EnsureFolder(parent);
            }

            AssetDatabase.CreateFolder(parent, name);
        }
    }
}
