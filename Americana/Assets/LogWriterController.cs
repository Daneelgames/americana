using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LogWriterController : MonoBehaviour
{
    public static LogWriterController instance;

    public TextMeshProUGUI typewriterTextField;
    public Animator typewriterTextFieldAnim;

    private string updateString = "Update";
    
    void Awake()
    {
        instance = this;
    }

    public void NewLine(string line)
    {
        typewriterTextField.text = line;
        typewriterTextFieldAnim.SetTrigger(updateString);
    }
    
}
