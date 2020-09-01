using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityManager : MonoBehaviour
{
    public static CityManager instance;

    [SerializeField] private GameObject cityActiveElements;
    public Camera cityCamera;

    
    void Awake()
    {
        instance = this;
    }

    public void ToggleCity(bool active)
    {
        cityActiveElements.SetActive(active);
        
        if (active)
            StartSimulation();
        else
            StopSimulation();
    }

    void StartSimulation()
    {
    }
    
    void StopSimulation()
    {
    }
}