using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RandomEventsController : MonoBehaviour
{
    public static RandomEventsController instance;
    public RandomEventsDatabase randomEventsDatabase;
    
    public Animator randomEventScreenAnim;

    private string updateString = "Update";
    private string activeString = "Active";

    public TextMeshProUGUI eventDescription;
    public List<TextMeshProUGUI> answerVariants = new List<TextMeshProUGUI>();

    private GameManager gm;
    
    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        gm = GameManager.instance;
        
        randomEventScreenAnim.SetBool(activeString, false);
    }

    public void CreateRandomEvent()
    {
        RandomEvent newEvent = GetRandomEvent();
        CharacterStats characterOfTheEvent = gm.partyCharacters[Random.Range(0, gm.partyCharacters.Count)];
        
        int answersAmount = newEvent.answers.Count;

        for (int i = 0; i < answerVariants.Count; i++)
        {
            if (i < answersAmount)
            {
                answerVariants[i].text = newEvent.answers[i].text[gm.language];
                answerVariants[i].transform.parent.gameObject.SetActive(true);
            }
            else
            {
                answerVariants[i].transform.parent.gameObject.SetActive(false);
            }
        }

        eventDescription.text = characterOfTheEvent.name[gm.language] + " " + newEvent.eventDescription[gm.language];
        
        randomEventScreenAnim.SetBool(activeString, true);
    }

    RandomEvent GetRandomEvent()
    {
        // replace it later by checking the status of the party
        if (gm.partyCharacters.Count == 1)
            return randomEventsDatabase.soloEvents[Random.Range(0, randomEventsDatabase.soloEvents.Count)];
        else if (gm.partyCharacters.Count == 2)
            return randomEventsDatabase.pairEvents[Random.Range(0, randomEventsDatabase.pairEvents.Count)];
        else
            return randomEventsDatabase.trioEvents[Random.Range(0, randomEventsDatabase.trioEvents.Count)];
    }

    public void ChooseEventVariant(int index)
    {
    }
}