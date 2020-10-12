using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    private Vector3 livingRoomPos = new Vector3(0, 18.08f, -1.35f);
    private Vector3 kitchenPos = new Vector3(-105.3f, 18.08f, -1.35f);

    [SerializeField] private Transform livingRoomWorld;
    [SerializeField] private Transform kitchenWorld;

    [SerializeField] private GameObject playerObject;
    [SerializeField] private PlayerController playerScript;

    private int currenRoom = 0;

    [SerializeField] private List<GameObject> inactiveAudioObjects = new List<GameObject>();
    private List<GameObject> activeAudioObjects = new List<GameObject>();

    public int currentSoundsActive = 0;
    private int currentRangeIncrease = 0;

    private int maxRange = 60;

    private bool canUseComfortObject = true;

    private float sanity = 100;

    private int lastIncreaseNumber = 100;
    private int newIncreaseNumber = 97;
    private int sanitySteps = 3;

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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            switchRooms();
        }

        if (Input.GetKeyDown(KeyCode.E) && canUseComfortObject)
        {
            StartCoroutine(UseComfortObject());
        }

        if(sanity < newIncreaseNumber)
        {
            currentRangeIncrease++;
            IncreaseAudioRange();

            int last = newIncreaseNumber;

            newIncreaseNumber = lastIncreaseNumber - sanitySteps;
            lastIncreaseNumber = last;
        }

        if (sanity > 0)
        {
            sanity -= ((float)currentSoundsActive / 10000f);
        }
        else sanity = 0;
        
    }

    private void IncreaseAudioRange()
    {
        int newRange = 30 + currentRangeIncrease;

        if (newRange > 60) newRange = 60;

        foreach (GameObject item in activeAudioObjects)
        {
            item.GetComponent<AudioSource>().maxDistance = (newRange);
        }
    }

    private void RevertAudioRange()
    {
        currentRangeIncrease = 0;
        lastIncreaseNumber = 100;
        newIncreaseNumber = 97;
        sanity = 100;

        foreach(GameObject item in activeAudioObjects)
        {
            item.GetComponent<AudioSource>().mute = false;
            item.GetComponent<AudioSource>().maxDistance = 30;
        }
    }

    private void ShortAudioRange()
    {
        foreach (GameObject item in activeAudioObjects)
        {
            item.GetComponent<AudioSource>().mute = true;
        }
    }

    private IEnumerator UseComfortObject()
    {
        canUseComfortObject = false;

        ShortAudioRange();
        yield return new WaitForSeconds(5);
        RevertAudioRange();
        yield return new WaitForSeconds(30);
        canUseComfortObject = true;
    }

    private void switchRooms()
    {
        if(currenRoom == 0)
        {
            playerScript.ChangeAttachedRoom(kitchenWorld);
            playerObject.transform.position = kitchenPos;
            currenRoom = 1;
        }
        else
        {
            playerScript.ChangeAttachedRoom(livingRoomWorld);
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

        yield return new WaitForSeconds(cooldownTime);
        enableNextSound = true;
    }
}
