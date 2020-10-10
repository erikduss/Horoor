using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    private Vector3 livingRoomPos = new Vector3(-20.5f, 0, 20.5f);
    private Vector3 kitchenPos = new Vector3(25f, 0, 25f);

    [SerializeField] private GameObject playerObject;

    private int currenRoom = 0;

    [SerializeField] private List<GameObject> inactiveAudioObjects = new List<GameObject>();
    private List<GameObject> activeAudioObjects = new List<GameObject>();

    public int currentSoundsActive = 0;

    private bool enableNextSound = true;

    // Start is called before the first frame update
    void Start()
    {
        //playerObject.transform.position = livingRoomPos;

        if(inactiveAudioObjects != null)
        {
            foreach (GameObject item in inactiveAudioObjects)
            {
                AudioObject audio = item.GetComponent<AudioObject>();
                audio.ChangeAudioState(AudioState.STOP);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (enableNextSound && inactiveAudioObjects.Count > 0)
        {
            int randomNumber = Random.Range(0, inactiveAudioObjects.Count);

            StartCoroutine(MoveGameObjectToActivePool(randomNumber));
        }
    }

    private void switchRooms()
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

    public void removeSoundSource(GameObject objectToRemove)
    {
        activeAudioObjects.Remove(objectToRemove);
        currentSoundsActive--;
    }

    private IEnumerator MoveGameObjectToActivePool(int number)
    {
        enableNextSound = false;

        activeAudioObjects.Add(inactiveAudioObjects[number]);

        inactiveAudioObjects.RemoveAt(number);

        activeAudioObjects.Last().GetComponent<AudioObject>().ChangeAudioState(AudioState.PLAY);
        currentSoundsActive++;

        //Formule: 50 x 0.3 bijvoorbleed. Hoe meer geluiden er aan staan, hoe meer tijd je krijgt.
        float minTime = (50 * (float)(currentSoundsActive * 0.1f));
        float maxTime = (100 * (float)(currentSoundsActive * 0.1f));

        int cooldownTime = Random.Range((int)minTime, (int)maxTime);

        Debug.Log("New sound in: " + cooldownTime + " Seconds");

        Debug.Log("Added new sound");

        yield return new WaitForSeconds(cooldownTime);
        enableNextSound = true;
    }
}
