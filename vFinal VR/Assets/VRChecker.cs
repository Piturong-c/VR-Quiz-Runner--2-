using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VRChecker : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] availableOnVR;
    public GameObject[] notAvailableOnVR;

    private void Awake()
    {
        if (!OVRManager.isHmdPresent && !OVRManager.tracker.isPresent)
        {
            SceneManager.LoadScene(3);
            foreach (var e in availableOnVR)
            {
                e.SetActive(false);
                Destroy(GameObject.Find("Canvas_VR"));
            }

            foreach (var e in notAvailableOnVR)
            {
                e.SetActive(true);
            }
        }
        else
        {
            foreach (var e in availableOnVR)
            {
                e.SetActive(true);

            }

            foreach (var e in notAvailableOnVR)
            {
                e.SetActive(false);
            }
        }
    }

    void Start()
    {
        StartCoroutine(MoreOculusChecks());
    }

    // Update is called once per frame
    public IEnumerator MoreOculusChecks()
    {
        while ( !OVRManager.isHmdPresent && !OVRManager.tracker.isPresent )
        {
            Debug.LogError ( "not f" );
            yield return new WaitForSeconds(.50f);
        }
        Debug.LogWarning( "present" );
        foreach (var e in availableOnVR)
        {
            e.SetActive(true);
        }

        foreach (var e in notAvailableOnVR)
        {
            e.SetActive(false);
        }
    }
}
