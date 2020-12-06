using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JadeCave : MonoBehaviour
{   
    public GameObject[] jadePeople;
    public GameObject[] normalPeople;
    public GameObject jadeCam;
    public GameObject victoryText;

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            foreach(GameObject jadePerson in jadePeople)
            {
                jadePerson.SetActive(true);
            }
            foreach(GameObject normalPerson in normalPeople)
            {
                normalPerson.SetActive(false);
            }
        }
        jadeCam.SetActive(true);
        victoryText.SetActive(true);
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

}
