using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class NameDisplay : MonoBehaviour
{
    [SerializeField] TMP_Text nameDisplay;
    [SerializeField] GameObject registerBTN;
    NameSystem nameSystem;
    private const string StudentSavedNameKey = "StudentSavedName";

    void Start()
    {
        nameDisplay.text = "Lets tackle the Hardwork!";
    }
    public void MainScreen()
    {
        SceneManager.LoadScene("StarterScreen");
    }

    
}
