using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class NameSystem : MonoBehaviour
{

    // Create the Variables
    [SerializeField] TMP_InputField studentName;
    [SerializeField] TextMeshProUGUI nameDisplay;
    [SerializeField] TextMeshProUGUI errorDisplay;
    [SerializeField] GameObject nameInserter;
    [SerializeField] GameObject nameInstructor;
    [SerializeField] GameObject clearNameBTN;
    [SerializeField] GameObject beginBTN;

    // Have a constant that will be used as the key for storing and retrieving the player's name in PlayerPrefs
    private const string StudentNameKey = "StudentName";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Call the LoadName method to check if there is a saved name and display it when the game starts
        LoadName();
    }
    void Update()
    {
        //Check if the clear name button is clicked and call the ClearName method if it is
        if (EventSystem.current.currentSelectedGameObject == clearNameBTN)
        {
            ClearName();
        }
        else if (EventSystem.current.currentSelectedGameObject == beginBTN)
        {
            SaveName();
        }
    }
    public void SaveName()
    {
        //Set the name given by the player to the name display text
        string name = studentName.text;

        //Check if there is no blank space in the inputfield
        if(!string.IsNullOrEmpty(name))
        {
            //Display the name on the name display text
            nameDisplay.text = "HI , " + name + "! Happy to see you Join Potano CLOUD !";
            PlayerPrefs.SetString(StudentNameKey, name);// Save the student name to the PLayerPrefs using the defined key
            PlayerPrefs.Save(); // Ensure that the data is saved to disk
            nameInserter.SetActive(false); // Make the name input field and instructor invisible after saving the name
            nameInstructor.SetActive(false);    
            errorDisplay.text = ""; // Clear any error messages

            if(EventSystem.current.currentSelectedGameObject == beginBTN)
            {
                nameDisplay.text = "Welcome " + name + "! Let us put in the work. "; // Display the saved name on the name display text
            }
        }
    }
    public void LoadName()
    {
        // Check if a name is already saved in PlayerPrefs
        if (PlayerPrefs.HasKey(StudentNameKey))
        {
            //Make the gameobject invisible
            nameInserter.SetActive(false);
            nameInstructor.SetActive(false);
            // If a name is saved, retrieve it and display it on the name display text
            string savedName = PlayerPrefs.GetString(StudentNameKey);
            nameDisplay.text = "Welcome " + savedName + "! Let us put in the work. "; // Display the saved name on the name display text
        }
        else if (!PlayerPrefs.HasKey(StudentNameKey))
        {
            //Display ann error messgaee if there is no name saved
            nameInserter.SetActive(true);
            nameInstructor.SetActive(false);
            errorDisplay.text = "Please enter your name First: ";
        }
    }
    
    public void ClearName()
    {
        // Clear the saved name
        PlayerPrefs.DeleteKey(StudentNameKey);
        PlayerPrefs.Save(); // Ensure that the data is saved to disk

        // Clear the name display text and show the input field again
        nameDisplay.text = "";
        nameInserter.SetActive(true);
        nameInstructor.SetActive(true);

    }
    // Call this from the button's OnClick event in the Inspector
    public void OnBeginButtonClicked()
    {
        //Set to not react until th name is inserted
        if (!PlayerPrefs.HasKey(StudentNameKey)) { return; }
        else
        {
            //Set to Open the next scene
            SceneManager.LoadScene("OpenPager");
        }
    }
    public void ExitGame()
    {
        //Exit the entire game
        Application.Quit();
    }

}

