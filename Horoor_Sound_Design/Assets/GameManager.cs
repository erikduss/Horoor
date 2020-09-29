using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Vector3 livingRoomPos = new Vector3(-20.5f, 0, 20.5f);
    private Vector3 kitchenPos = new Vector3(25f, 0, 25f);

    [SerializeField] private GameObject playerObject;

    private int currenRoom = 0;

    // Start is called before the first frame update
    void Start()
    {
        playerObject.transform.position = livingRoomPos;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            switchRooms();
        }
    }

    void switchRooms()
    {
        if(currenRoom == 0)
        {
            playerObject.transform.position = kitchenPos;
            currenRoom = 1;
        }
        else
        {
            playerObject.transform.position = livingRoomPos;
            currenRoom = 0;
        }
    }
}
