using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LogWriterController : MonoBehaviour
{
    public static LogWriterController instance;

    public TextMeshProUGUI typewriterTextField;
    public Animator typewriterTextFieldAnim;
    
    public List<string> savedStrings = new List<string>();

    private string updateString = "Update";
    
    void Awake()
    {
        instance = this;
    }

    public void NewLine(string line)
    {
        if (savedStrings.Count <= 0 || savedStrings[savedStrings.Count - 1] != line)
        {
            savedStrings.Add(line);
            if (savedStrings.Count > 3)
                savedStrings.RemoveAt(0);

            line = "";
            for (int i = 0; i < savedStrings.Count; i++)
            {
                line = line + Environment.NewLine + savedStrings[i];
            }

            typewriterTextField.text = line;
        }
        typewriterTextFieldAnim.SetTrigger(updateString);
    }
    
}