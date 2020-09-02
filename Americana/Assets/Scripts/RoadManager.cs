using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoadManager : MonoBehaviour
{
    public static RoadManager instance;
    
    public bool moveRoad = false;
    public bool moveRoadProps = false;
    public bool countingSteps = false;
    public float moveSpeed = 10;
    private float moveSpeedCurrent = 0;

    [Header("Settings")] 
    public int stepsToNextTown = 5;
    public int currentStep = 0;

    public float timeBetweenStepsMin = 3;
    public float timeBetweenStepsMax = 6;
    
    public int farPropsAmount = 10;
    public float farPropsBetweenDistance = 15;
    public int mediumPropsAmount = 20;
    public float mediumPropsBetweenDistance = 20;
    public int closePropsAmount = 30;
    public float closePropsBetweenDistance = 5;
    
    [Header("Links")]
    public List<GameObject> farBackgroundPropsPrefabs = new List<GameObject>(); // -100 / -75
    public List<GameObject> mediumBackgroundPropsPrefabs = new List<GameObject>(); // -60 / -40
    public List<GameObject> closeBackgroundPropsPrefabs = new List<GameObject>(); // -30 / -5
    public Transform propsParent;
    public Animator partyCar;
    
    List<GameObject> farBackgroundProps = new List<GameObject>(); // -100 / -75
    List<GameObject> mediumBackgroundProps = new List<GameObject>(); // -60 / -40
    List<GameObject> closeBackgroundProps = new List<GameObject>(); // -30 / -5
    List<GameObject> objectsOnRoad = new List<GameObject>(); // -30 / -5

    private string drivingString = "Driving";

    private Coroutine drivingCoroutine;

    private RandomEventsController rec;

    void Awake()
    {
        instance = this;
    }
    
    void Start()
    {
        rec = RandomEventsController.instance;
        
        GenerateRoad();
    }
    
    void Update()
    {
        if (moveRoadProps)
            MoveRoad();
    }

    public void StartDriving()
    {
        partyCar.SetBool(drivingString, true);
        countingSteps = true;
        drivingCoroutine = StartCoroutine(Driving());
        StartCoroutine(IncreaseSpeed());
    }

    IEnumerator IncreaseSpeed()
    {
        float tCur = 0;
        float t = 2;
        moveSpeedCurrent = 0;
        moveRoadProps = true;
        moveRoad = true;
        while (tCur < t)
        {
            moveSpeedCurrent = Mathf.Lerp(moveSpeedCurrent, moveSpeed, tCur / t);
            tCur += Time.deltaTime;
            yield return null;
        }
    }

    public void StopDriving()
    {
        partyCar.SetBool(drivingString, false);
        countingSteps = false;
        
        StopCoroutine(drivingCoroutine);
        StartCoroutine(DecreaseSpeed());
    }
    
    IEnumerator DecreaseSpeed()
    {
        float tCur = 0;
        float t = 2;
        moveSpeedCurrent = moveSpeed;
        moveRoad = false;
        while (tCur < t)
        {
            moveSpeedCurrent = Mathf.Lerp(moveSpeedCurrent, 0, tCur / t);
            tCur += Time.deltaTime;
            yield return null;
        }
        moveRoadProps = false;
    }

    public void ContinueCountingSteps()
    {
        countingSteps = true;
        drivingCoroutine = StartCoroutine(Driving());
    }
    public void StopCountingSteps()
    {
        countingSteps = false;
        StopCoroutine(drivingCoroutine);
    }

    IEnumerator Driving()
    {
        yield return new WaitForSeconds(Random.Range(timeBetweenStepsMin, timeBetweenStepsMax));
        
        currentStep++;

        if (currentStep >= stepsToNextTown)
        {
            //arrive to town
            //temporal new events 
            rec.CreateRandomEvent();
            StopCountingSteps();
            currentStep = 0;
        }
        else
        {
            // create event
            // show event message
            rec.CreateRandomEvent();
            StopCountingSteps();
        }
    }

    void GenerateRoad()
    {
        // spawn for current screen
        SpawnProps(farBackgroundPropsPrefabs, farBackgroundProps, farPropsAmount, -100, farPropsBetweenDistance, -100, -75);
        SpawnProps(mediumBackgroundPropsPrefabs, mediumBackgroundProps, mediumPropsAmount, -80, mediumPropsBetweenDistance, -60, -40);
        SpawnProps(closeBackgroundPropsPrefabs, closeBackgroundProps, closePropsAmount, -75, closePropsBetweenDistance, -30, -5);
    }

    void SpawnProps(List<GameObject> prefabs, List<GameObject> targetList, int amount, float startZ, float propsBetweenDistance, float xMin, float xMax)
    {
        for (int i = 0; i < amount; i++)
        {
            Vector3 spawnPos = Vector3.forward * startZ;
            spawnPos = new Vector3(Random.Range(xMin, xMax), 0, spawnPos.z + propsBetweenDistance * i);
            var newProp = Instantiate(prefabs[Random.Range(0, prefabs.Count)], spawnPos, Quaternion.identity);
            newProp.transform.Rotate(Vector3.up, Random.Range(0,360));
            newProp.transform.parent = propsParent;
            targetList.Add(newProp);
        }
    }

    void MoveRoad()
    {
        for (int i = 0; i < farBackgroundProps.Count; i++)
        {
            farBackgroundProps[i].transform.Translate(Vector3.forward * -moveSpeedCurrent * Time.deltaTime, Space.World);
            if (farBackgroundProps[i].transform.position.z < -200)
            {
                farBackgroundProps[i].transform.position += Vector3.forward * 400;      
                farBackgroundProps[i].transform.Rotate(Vector3.up, 45);
            }
        }
        for (int i = 0; i < mediumBackgroundProps.Count; i++)
        {
            mediumBackgroundProps[i].transform.Translate(Vector3.forward * -moveSpeedCurrent * Time.deltaTime, Space.World);
            if (mediumBackgroundProps[i].transform.position.z < -150)
            {
                mediumBackgroundProps[i].transform.position += Vector3.forward * 300;   
                mediumBackgroundProps[i].transform.Rotate(Vector3.up, 45);
            }
        }
        for (int i = 0; i < closeBackgroundProps.Count; i++)
        {
            closeBackgroundProps[i].transform.Translate(Vector3.forward * -moveSpeedCurrent * Time.deltaTime, Space.World);
            if (closeBackgroundProps[i].transform.position.z < -120)
            {
                closeBackgroundProps[i].transform.position += Vector3.forward * 240;
                closeBackgroundProps[i].transform.Rotate(Vector3.up, 45);
            }
        }
        for (int i = objectsOnRoad.Count - 1; i >= 0 ; i--)
        {
            if (objectsOnRoad[i] == null)
            {
                objectsOnRoad.RemoveAt(i);
                continue;
            }
            objectsOnRoad[i].transform.Translate(Vector3.forward * -moveSpeedCurrent * Time.deltaTime, Space.World);
            if (objectsOnRoad[i].transform.position.z < -100)
            {
                Destroy(objectsOnRoad[i]);
                objectsOnRoad.RemoveAt(i);
            }
            
        }
    }

    public void AddObjectToMovingObjectsOnRoad(GameObject go)
    {
        objectsOnRoad.Add(go);
    }
}