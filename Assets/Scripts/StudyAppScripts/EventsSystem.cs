using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class EventsSystem : MonoBehaviour
{
    // Create the variables
    [SerializeField] TMP_InputField eventName;
    [SerializeField] TextMeshProUGUI eventDisplay;
    [SerializeField] GameObject eventInserter;
    [SerializeField] GameObject saveBTN;
    private int currentButtonID = -1;

    // Called when any button is clicked
    public void SelectButton(int buttonID)
    {
        currentButtonID = buttonID;

        string eventkey = "Event_" + currentButtonID;

        if (PlayerPrefs.HasKey(eventkey))
        {
            eventDisplay.text = PlayerPrefs.GetString(eventkey);
        }
        else
        {
            eventDisplay.text = "These is nothing present currently!";
        }
    }

    // Called when Save is pressed
    public void SaveEvent()
    {
        // Check if a button is selected before saving the event
        if (currentButtonID == -1) return;

        if (!string.IsNullOrEmpty(eventName.text))
        {
            string key = "Event_" + currentButtonID;

            // Save the event name to PlayerPrefs using the defined key
            PlayerPrefs.SetString(key, eventName.text);
            PlayerPrefs.Save();

            eventDisplay.text = "Your Events are: " + eventName.text;
            eventName.text = "";
        }
    }

    public void ClearEvent()
    {
        string EventNameKey = "Event_" + currentButtonID;
        // Clear the saved event
        PlayerPrefs.DeleteKey(EventNameKey);
        PlayerPrefs.Save(); // Ensure that the data is saved to disk

        // Clear the event display text and show the input field again
        eventDisplay.text = "These is nothing present currently!";

    }
    

    public void NextBTN()
    {
        //Set to Open the next scene
        //SceneManager.LoadScene("OpenPager");
    }
    public void PrevioustBTN()
    {
        //Set to Open the next scene
        //SceneManager.LoadScene("OpenPager");
    }

}