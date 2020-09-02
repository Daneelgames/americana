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

    private CharacterStats characterOfTheEvent;
    private RandomEvent currentEvent;
    private bool answerChosen = false;

    private GameManager gm;
    private CharacterGenerator characterGenerator;
    private RoadManager roadManager;
    private GameTimeManager gtm;
    
    
    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        gm = GameManager.instance;
        characterGenerator = CharacterGenerator.instance;
        roadManager = RoadManager.instance;
        gtm = GameTimeManager.instance;
        
        randomEventScreenAnim.SetBool(activeString, false);
    }

    public void CreateRandomEvent()
    {
        
        currentEvent = GetRandomEvent();
        answerChosen = false;
        characterOfTheEvent = gm.partyCharacters[Random.Range(0, gm.partyCharacters.Count)];
        
        int answersAmount = currentEvent.answers.Count;

        for (int i = 0; i < answerVariants.Count; i++)
        {
            if (i < answersAmount)
            {
                answerVariants[i].text = currentEvent.answers[i].text[gm.language];
                answerVariants[i].transform.parent.gameObject.SetActive(true);
            }
            else
            {
                answerVariants[i].transform.parent.gameObject.SetActive(false);
            }
        }

        eventDescription.text = characterOfTheEvent.name[gm.language] + " " + currentEvent.eventDescription[gm.language];
        
        randomEventScreenAnim.SetBool(activeString, true);
    }

    RandomEvent GetRandomEvent()
    {
        // replace it later by checking the status of the party
        if (Random.value >= 0.5f)
        {
            // universal event
            return randomEventsDatabase.universalEvents[Random.Range(0, randomEventsDatabase.universalEvents.Count)];
        }
        else
        {
            if (gm.partyCharacters.Count == 1)
                return randomEventsDatabase.soloEvents[Random.Range(0, randomEventsDatabase.soloEvents.Count)];
            else if (gm.partyCharacters.Count == 2)
                return randomEventsDatabase.pairEvents[Random.Range(0, randomEventsDatabase.pairEvents.Count)];
            else
                return randomEventsDatabase.trioEvents[Random.Range(0, randomEventsDatabase.trioEvents.Count)];   
        }
    }

    public void ChooseEventVariant(int index)
    {
        if (answerChosen)
        {
            CloseEventScreen();
            if (roadManager.moveRoad)
                roadManager.ContinueCountingSteps();
            return;
        }
        
        answerChosen = true;
        switch (currentEvent.randomEvent)
        {
            case RandomEvent.Event.StrangerOnTheRoad:
                if (index == 0)
                {
                    // stop the car
                    // create a character on the road
                    gm.StopDriving();
                    characterGenerator.SpawnNewCharactersOnTheRoad(Random.Range(1,4));
                    eventDescription.text = currentEvent.results[0].text[gm.language];
                }
                else if (index == 1) // drive by
                {
                    if (Random.value >= 0.5f)
                    {
                        eventDescription.text = currentEvent.results[1].text[gm.language];
                    }
                    else
                    {
                        eventDescription.text = currentEvent.results[2].text[gm.language];
                    }
                }
                
                for (int i = 0; i < answerVariants.Count; i++)
                {
                    if (i == 0)
                    {
                        answerVariants[i].text = randomEventsDatabase.ok[gm.language];
                        answerVariants[i].transform.parent.gameObject.SetActive(true);
                    }
                    else
                    {
                        answerVariants[i].transform.parent.gameObject.SetActive(false);
                    }
                }
                randomEventScreenAnim.SetTrigger(updateString);
                
                break;
            case RandomEvent.Event.FightOverMoney:
                if (index == 0)
                {
                    // share the money
                    eventDescription.text = currentEvent.results[0].text[gm.language];
                }
                else if (index == 1)
                {
                    // fight
                    if (Random.value >= 0.5f)
                    {
                        eventDescription.text = currentEvent.results[1].text[gm.language];
                        characterOfTheEvent.conscious -= Mathf.RoundToInt(characterOfTheEvent.consciousMax * 0.25f);
                    }
                    else
                    {
                        eventDescription.text = currentEvent.results[2].text[gm.language];
                        for (int i = 0; i < gm.partyCharacters.Count; i++)
                        {
                            gm.partyCharacters[i].conscious -= Mathf.RoundToInt(gm.partyCharacters[i].consciousMax * 0.1f);
                        }
                    }
                }
                
                for (int i = 0; i < answerVariants.Count; i++)
                {
                    if (i == 0)
                    {
                        answerVariants[i].text = randomEventsDatabase.ok[gm.language];
                        answerVariants[i].transform.parent.gameObject.SetActive(true);
                    }
                    else
                    {
                        answerVariants[i].transform.parent.gameObject.SetActive(false);
                    }
                }
                randomEventScreenAnim.SetTrigger(updateString);
                break;
            
            case RandomEvent.Event.BittenBySnake:
                if (index == 0)
                {
                    // stop car
                    gm.StopDriving();
                    gtm.AddHour();
                    eventDescription.text = currentEvent.results[0].text[gm.language];
                }
                else if (index == 1)
                {
                    // drive by
                    if (Random.value >= 0.5f)
                    {
                        // you vomited
                        gm.StopDriving();
                        eventDescription.text = currentEvent.results[1].text[gm.language];
                        characterOfTheEvent.conscious -= Mathf.RoundToInt(characterOfTheEvent.consciousMax * 0.25f);
                        characterOfTheEvent.hp -= Mathf.RoundToInt(characterOfTheEvent.hp * 0.25f);
                    }
                    else
                    {
                        // snake was not poisonous
                        eventDescription.text = currentEvent.results[2].text[gm.language];
                    }
                }
                
                for (int i = 0; i < answerVariants.Count; i++)
                {
                    if (i == 0)
                    {
                        answerVariants[i].text = randomEventsDatabase.ok[gm.language];
                        answerVariants[i].transform.parent.gameObject.SetActive(true);
                    }
                    else
                    {
                        answerVariants[i].transform.parent.gameObject.SetActive(false);
                    }
                }
                randomEventScreenAnim.SetTrigger(updateString);
                break;
            
            case RandomEvent.Event.ShackNearRoad:
                if (index == 0)
                {
                    // stop car
                    gm.StopDriving();
                    eventDescription.text = currentEvent.results[0].text[gm.language];
                }
                else if (index == 1)
                {
                    // drive by
                    eventDescription.text = currentEvent.results[1].text[gm.language];
                }
                
                for (int i = 0; i < answerVariants.Count; i++)
                {
                    if (i == 0)
                    {
                        answerVariants[i].text = randomEventsDatabase.ok[gm.language];
                        answerVariants[i].transform.parent.gameObject.SetActive(true);
                    }
                    else
                    {
                        answerVariants[i].transform.parent.gameObject.SetActive(false);
                    }
                }
                randomEventScreenAnim.SetTrigger(updateString);
                break;
        }
    }

    void CloseEventScreen()
    {
        randomEventScreenAnim.SetBool(activeString, false);
    }
}