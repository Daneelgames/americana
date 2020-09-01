using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterVisualController : MonoBehaviour
{
    public Animator baseVisualAnim;
    public Animator meshAnim;

    private string actString = "Act";
    private string damagedString = "Damaged";
    private string deathString = "Death";
    
    private string updateString = "Update";
    private string hideString = "Hide";
    private string activeString = "Active";

    public Animator hpObject;
    public Image healthBar;
    public Image consciousBar;

    private GameManager gm;
    
    void Start()
    {
        gm = GameManager.instance;
    }
    
    public void AnimateCharacter(CharacterVisualMessage.Message message)
    {
        switch (message)
        {
            case CharacterVisualMessage.Message.Act:
                baseVisualAnim.SetTrigger(actString);
                break;
            case CharacterVisualMessage.Message.Damaged:
                hpObject.SetTrigger(updateString);
                baseVisualAnim.SetTrigger(damagedString);
                break;
            case CharacterVisualMessage.Message.Dead:
                hpObject.SetBool(activeString, false);
                baseVisualAnim.SetBool(deathString, true);
                break;
            case CharacterVisualMessage.Message.Alive:
                hpObject.SetBool(activeString, true);
                baseVisualAnim.SetBool(deathString, false);
                break;
        }
    }

    void Update()
    {
        hpObject.transform.LookAt(gm.activeCamera.transform.position);
    }

    public void HideCharacter()
    {
        hpObject.SetBool(activeString, false);
        baseVisualAnim.SetBool(hideString, true);
    }
    public void UnhideCharacter()
    {
        hpObject.SetBool(activeString, true);
        baseVisualAnim.SetBool(hideString, false);
    }
}


public static class CharacterVisualMessage
{
    public enum Message
    {
        Act, Damaged, Dead, Alive
    }
}