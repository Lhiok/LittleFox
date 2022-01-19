using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class TipController : MonoBehaviour
{
    [Header("属性")]
    public GameObject tipUI;

    private void Start() {
        tipUI.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player")
        {
            tipUI.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Player")
        {
            tipUI.SetActive(false);
        }
    }
}
