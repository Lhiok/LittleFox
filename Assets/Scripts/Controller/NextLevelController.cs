using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelController : MonoBehaviour
{
    private bool pressE = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("E"))
        {
            pressE = true;
        }       
    }

    private void FixedUpdate() {
        if (pressE)
        {
            pressE = true;
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex+1, LoadSceneMode.Single);
        }
    }
}
