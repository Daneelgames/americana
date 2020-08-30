using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClickInWorld : MonoBehaviour
{
    public static PlayerClickInWorld instance;
    
    public CharacterStats selectedCharacter;

    public LayerMask layerMask;
    private GameManager gm;
    public Animator characterTargetFeedbackAnim;

    RaycastHit hit;

    void Awake()
    {
        instance = this;
    }
    
    void Start()
    {
        gm = GameManager.instance;
    }
    
    void Update()
    {
        Ray ray = gm.activeCamera.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray, out hit, layerMask)) 
        {
            CharacterStats character = hit.collider.gameObject.transform.parent.parent.gameObject.GetComponent<CharacterStats>();
            if (character != null)
            {
                if (character != selectedCharacter)
                {
                    selectedCharacter = character;
                    selectedCharacter.interactor.SelectCharacter();   
                }
            }
            else
                selectedCharacter = null;
        }
        else
        {
            selectedCharacter = null;
        }
        
        if (!selectedCharacter)
            characterTargetFeedbackAnim.gameObject.SetActive(false);

        if (Input.GetMouseButtonDown(0))
        {
            if (selectedCharacter != null)
            {
                selectedCharacter.interactor.ClickCharacter();
            }
        }
    }
    
    
    
}
