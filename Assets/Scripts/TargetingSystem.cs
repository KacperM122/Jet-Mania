using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TargetingSystem : MonoBehaviour
{
    public GameObject[] allTargets;
    public GameObject currentTarget;
    public Image marker;
    public float nearestDistance;
    public TextMeshProUGUI targetsNumUI;
    public GameObject victoryUI;

    public float defaultNearestDistanceValue = 50000f;

    float distance;
    

    void Start()
    {
        allTargets = GameObject.FindGameObjectsWithTag("Target");
        nearestDistance = Vector3.Distance(transform.position, allTargets[0].transform.position);
    }

    // Update is called once per frame
    void Update()
    {

        allTargets = GameObject.FindGameObjectsWithTag("Target");


        if (currentTarget == null)
        {
            marker.enabled = false;
            nearestDistance = defaultNearestDistanceValue;
            FindClosestTarget();

        }
        else
        {    
            marker.enabled = true;
            Vector2 markerPos = Camera.main.WorldToScreenPoint(currentTarget.transform.position);          

            if (Vector3.Dot((currentTarget.transform.position - transform.position), transform.forward) < 0)
            {
                // marker jest za graczem
                if (markerPos.x < Screen.width / 2)
                {
                    markerPos.x = Screen.width - marker.GetPixelAdjustedRect().width / 2;
                }
                else
                {
                    markerPos.x = marker.GetPixelAdjustedRect().width / 2;
                }
            }

            marker.transform.position = markerPos;
        }

        targetsNumUI.text = "Cele: " + allTargets.Length.ToString();

        if (allTargets.Length == 0) 
        {
            victoryUI.SetActive(true);
            StartCoroutine(ReturnToMainMenu());
            //Time.timeScale = 0;
        }
    }

    // wykrywanie najbli¿szego celu
    public void FindClosestTarget()
    {
        if (allTargets.Length > 0)
        {
            for (int i = 0; i<allTargets.Length; i++)
            {
                distance = Vector3.Distance(transform.position, allTargets[i].transform.position);         

                if (distance<nearestDistance)
                {
                    currentTarget = allTargets[i];
                    nearestDistance = distance;
                }
            }
        }
    }

    // powrót do menu
    IEnumerator ReturnToMainMenu()
    {
        yield return new WaitForSeconds(5);

        SceneManager.LoadSceneAsync("MainMenu");
    }

}
