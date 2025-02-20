using TMPro;
using UnityEngine;

public class TextLog : MonoBehaviour
{
    public TextMeshProUGUI logText;  
    private string lastLogMessage = "";   

    void OnEnable()
    {
        Application.logMessageReceived += CaptureLog; 
    }

    void OnDisable()
    {
        Application.logMessageReceived -= CaptureLog;  
    }

    void CaptureLog(string logString, string stackTrace, LogType type)
    {
        lastLogMessage = logString;  
    }

    public void ShowLogOnScreen()
    {
        logText.text = lastLogMessage; 
    }
}
