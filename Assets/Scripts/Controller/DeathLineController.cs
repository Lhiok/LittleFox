using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathLineController : MonoBehaviour
{
    [Header("配置")]
    public float delayTime;

    private void OnTriggerEnter2D(Collider2D other) {
        Invoke(nameof(RestartGame), delayTime);
    }

    private void RestartGame()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }
}
