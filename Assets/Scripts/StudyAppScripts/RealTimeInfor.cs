using UnityEngine;
using System;
using System.Collections;
using TMPro;
using System.Runtime.Serialization;

public class RealTimeInfor : MonoBehaviour
{
    //Create Variables
    [SerializeField] TextMeshProUGUI studentTime;
    [SerializeField] TextMeshProUGUI studentDate;
    [SerializeField] TextMeshProUGUI studentZone;
    public float studentTimeUpdate = 1f;
    public float studentTimeIncreaser = 0f;

    private void Update()
    {
        //Check if a second has passed 
        studentTimeIncreaser += Time.deltaTime;
        //Check if it  is time to update the time
        if(studentTimeIncreaser <= studentTimeUpdate)
        {
            Debug.Log("The second has passed you can update");
            studentTimeIncreaser = 0f;
            UpdateInfor();
        }
        else
        {
            Debug.Log("The second is: " + studentTimeIncreaser);
        }
    }
    public void UpdateInfor()
    {
        //Get the time and date from the students computer
        DateTime currentTime = DateTime.Now;
        //Set the time zone for the students area
        TimeZoneInfo studentTimeZone = TimeZoneInfo.Local;

        //Set the format for the time and date
        string formatDate = currentTime.ToString("dddd - dd MMM");
        string formatTime = currentTime.ToString("HH:mm:ss");

        //Display in text
        studentDate.text = formatDate;
        studentZone.text = studentTimeZone.StandardName;
        studentTime.text = formatTime;
       
    }
}
