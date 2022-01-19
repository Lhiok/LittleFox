using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    [Header("属性")]
    public float moveRate;

    private Transform playerTrans;
    private float startPosX;

    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerTrans = player.transform;
        startPosX = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        pos.x = startPosX + playerTrans.position.x * moveRate;
        transform.position = pos;
    }
}
