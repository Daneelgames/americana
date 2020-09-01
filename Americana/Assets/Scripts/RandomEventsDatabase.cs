using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RandomEventsDatabase", menuName = "ScriptableObjects/RandomEventsDatabase", order = 1)]
public class RandomEventsDatabase : ScriptableObject
{
    public List<RandomEvent> soloEvents = new List<RandomEvent>();
    public List<RandomEvent> pairEvents = new List<RandomEvent>();
    public List<RandomEvent> trioEvents = new List<RandomEvent>();
}

[Serializable]
public class RandomEvent
{
    public enum Event
    {
        BittenBySnake, StrangerOnTheRoad, ParkedCar, BumpyRoad, ShackNearRoad, GasStation, BrokenBridge, FightOverMoney, AnimalOnRoad, BodyOnRoad, RandomObjectOnRoad, CopPatrol
    }

    public Event randomEvent = Event.BittenBySnake;

    public List<string> eventDescription = new List<string>();
    
    public List<ListOfStrings> answers = new List<ListOfStrings>();
    public List<ListOfStrings> results = new List<ListOfStrings>();
}

[Serializable]
public class ListOfStrings
{
    public List<string> text = new List<string>();
}