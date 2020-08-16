using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Random = UnityEngine.Random;

public class PopulationManager : MonoBehaviour
{

    [SerializeField] private GameObject personPrefab;
    [SerializeField] private int populationSize = 10;
    
    private List<GameObject> population = new List<GameObject>();

    public static float elapsed = 0;
    private int _trialTime = 10;
    private int _generation = 1;

    private GUIStyle _guiStyle = new GUIStyle();
    
    private void Start()
    {
        for (int i = 0; i < populationSize; i++)
        {
            Vector3 pos = new Vector3(Random.Range(-9,9), Random.Range(-4.5f, 4.5f),0 );
            GameObject go = Instantiate(personPrefab, pos, Quaternion.identity);

            go.GetComponent<DNA>().r = Random.Range(0.0f, 1.0f);
            go.GetComponent<DNA>().g = Random.Range(0.0f, 1.0f);
            go.GetComponent<DNA>().b = Random.Range(0.0f, 1.0f);
            go.GetComponent<DNA>().s = Random.Range(0.1f, 0.3f);
            
            population.Add(go);
        }    
    }

    private void Update()
    {
        elapsed += Time.deltaTime;
        if (elapsed > _trialTime)
        {
            BreedNewPopulation();
            elapsed = 0;
        }
    }

    private void OnGUI()
    {
        _guiStyle.fontSize = 50;
        _guiStyle.normal.textColor = Color.white;
        GUI.Label(new Rect(10,10,100,20), "Generation: " + _generation, _guiStyle);
        GUI.Label(new Rect(10, 65, 100, 20), "Trial Time: " + (int) elapsed, _guiStyle);
        
    }

    private GameObject Breed(GameObject parent1, GameObject parent2)
    {
        Vector3 pos = new Vector3(Random.Range(-9,9), Random.Range(-4.5f,4.5f),0);
        GameObject offspring = Instantiate(personPrefab, pos, Quaternion.identity);

        DNA dna1 = parent1.GetComponent<DNA>();
        DNA dna2 = parent2.GetComponent<DNA>();
        
        //swap parent dna
        if (Random.Range(0, 1000) > 5)
        {
            offspring.GetComponent<DNA>().r = Random.Range(0, 10) < 5 ? dna1.r : dna2.r;
            offspring.GetComponent<DNA>().b = Random.Range(0, 10) < 5 ? dna1.g : dna2.g;
            offspring.GetComponent<DNA>().b = Random.Range(0, 10) < 5 ? dna1.b : dna2.b;
            offspring.GetComponent<DNA>().s = Random.Range(0, 10) < 5 ? dna1.s : dna2.s;
        }
        else
        {
            offspring.GetComponent<DNA>().r = Random.Range(0.0f,1.0f);
            offspring.GetComponent<DNA>().g = Random.Range(0.0f,1.0f);
            offspring.GetComponent<DNA>().b = Random.Range(0.0f,1.0f);
            offspring.GetComponent<DNA>().s = Random.Range(0.1f,0.3f);
        }

        return offspring;
    }

    private void BreedNewPopulation()
    {
        List<GameObject> newPopulation = new List<GameObject>();
        
        // get rid of unfit individuals
        List<GameObject> sortedList = population.OrderByDescending(o => 
            o.GetComponent<DNA>().timeToDie).ToList();
        population.Clear();
        
        // breed upper half of sorted list
        for (int i = (int) (sortedList.Count / 2.0f) - 1; i < sortedList.Count - 1; i++)
        {
            population.Add(Breed(sortedList[i], sortedList[i+1]));
            population.Add(Breed(sortedList[i+1], sortedList[i]));
        }
        
        // destroy all parents and previous population
        for (int i = 0; i < sortedList.Count; i++)
        {
            Destroy(sortedList[i]);
        }

        _generation++;
    }
}
