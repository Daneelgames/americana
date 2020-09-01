using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerClickInWorld : MonoBehaviour
{
    public static PlayerClickInWorld instance;
    
    public CharacterPlayerInteractor selectedInteractor;

    public LayerMask layerMask;
    private GameManager gm;
    public Animator characterTargetFeedbackAnim;

    private ActionWheelController awc;
    private CityManager cityManager;
    private RoadManager roadManager;
    private CombatManager combatManager;

    RaycastHit hit;

    void Awake()
    {
        instance = this;
        
    }
    
    void Start()
    {
        gm = GameManager.instance;
        combatManager = CombatManager.instance;
        cityManager = CityManager.instance;
        roadManager = RoadManager.instance;
        awc = ActionWheelController.instance;
    }
    
    void Update()
    {
        if (roadManager.moveRoad || EventSystem.current.IsPointerOverGameObject())
            return;

        if (!awc.wheelActive)
        {
            Ray ray = gm.activeCamera.ScreenPointToRay(Input.mousePosition);
        
            if (Physics.Raycast(ray, out hit, layerMask)) 
            {
                CharacterPlayerInteractor interactor = hit.collider.gameObject.transform.parent.parent.gameObject.GetComponent<CharacterPlayerInteractor>();
                if (interactor != null)
                {
                    if (interactor != selectedInteractor)
                    {
                        selectedInteractor = interactor;
                        selectedInteractor.SelectCharacter();   
                    }
                }
                else
                    selectedInteractor = null;
            }
            else
            {
                selectedInteractor = null;
            }
        
            if (!selectedInteractor)
                characterTargetFeedbackAnim.gameObject.SetActive(false);
        }
        else
        {
            selectedInteractor = null;
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            if (selectedInteractor != null)
            {
                selectedInteractor.ClickInteractor();
            }
            else
            {
                combatManager.SkipPause();
                awc.HideWheel();   
            }
        }
    }
}
