using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private Vector3 worldSpawnPoint = new Vector3(0, 36f, -1.35f);
    //private Vector3 kitchenPos = new Vector3(-105.3f, 18.08f, -1.35f);

    [SerializeField] private Transform livingRoomWorld;

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
    private int newIncreaseNumber = 96;
    private int sanitySteps = 4;

    private bool enableNextSound = false;

    private bool getAnnoyedBySanity = true;
    private bool isUsingComfortObject = false;

    private int amountOfSanityWarnings = 0;

    [SerializeField] private AudioManager audioManager;

    private bool endGame = false;

    private bool win = false;
    private bool stoppingGame = false;

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

        StartCoroutine(CoreGameEvenTimers());
    }

    // Update is called once per frame
    void Update()
    {
        if (endGame)
        {
            if (!stoppingGame)
            {
                StopAllCoroutines();
                StartCoroutine(PlayGameOver(win));
            }
            return;
        }

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
            sanity -= ((float)currentSoundsActive / 600f);
        }
        else
        {
            sanity = 0;
            if (getAnnoyedBySanity && !isUsingComfortObject)
            {
                StartCoroutine(getAnnoyedBySounds());
            }
        }

        if(amountOfSanityWarnings >= 3)
        {
            endGame = true;
            win = false;
        }
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

    private IEnumerator PlayGameOver(bool win)
    {
        stoppingGame = true;
        int waitTime = 10;
        int secondWaitTime = 1;

        if (win)
        {
            ShortAudioRange();
            audioManager.DoorbellSound();
            waitTime = 4;
            secondWaitTime = 15;
        }
        else
        {
            audioManager.GameOver();
            waitTime = 7;
        }
        
        yield return new WaitForSeconds(waitTime);
        if(win) audioManager.GameComplete();
        yield return new WaitForSeconds(secondWaitTime);

        GameOver();
    }

    private IEnumerator getAnnoyedBySounds()
    {
        audioManager.NeedComfort();
        getAnnoyedBySanity = false;
        yield return new WaitForSeconds(7);
        getAnnoyedBySanity = true;
        amountOfSanityWarnings++;
    }

    private void GameOver()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void RevertAudioRange()
    {
        currentRangeIncrease = 0;
        lastIncreaseNumber = 100;
        newIncreaseNumber = 96;
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
        amountOfSanityWarnings = 0;
        isUsingComfortObject = true;
        audioManager.UsingComfort();
        canUseComfortObject = false;
        getAnnoyedBySanity = false;

        ShortAudioRange();
        yield return new WaitForSeconds(5);
        RevertAudioRange();
        yield return new WaitForSeconds(5);
        isUsingComfortObject = false;
        amountOfSanityWarnings = 0;
        getAnnoyedBySanity = true;
        yield return new WaitForSeconds(20);
        canUseComfortObject = true;
    }

    private IEnumerator CoreGameEvenTimers()
    {
        audioManager.PlayIntroSound();
        yield return new WaitForSeconds(15);
        enableNextSound = true;
        yield return new WaitForSeconds(28);
        audioManager.PlayStoryLine();
        yield return new WaitForSeconds(28);
        audioManager.PlayStoryLine();
        yield return new WaitForSeconds(28);
        audioManager.PlayStoryLine();
        yield return new WaitForSeconds(28);
        audioManager.GameHalfway();
        yield return new WaitForSeconds(28);
        audioManager.PlayStoryLine();
        yield return new WaitForSeconds(28);
        audioManager.PlayStoryLine();
        yield return new WaitForSeconds(28);
        audioManager.PlayStoryLine();
        yield return new WaitForSeconds(28);
        endGame = true;
        win = true;
    }

    private void switchRooms()
    {
        /*if(currenRoom == 0)
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
        }*/
    }

    public void MoveGameobjectToInactive(GameObject obj)
    {
        int index = activeAudioObjects.IndexOf(obj);

        inactiveAudioObjects.Add(activeAudioObjects[index]);

        activeAudioObjects.RemoveAt(index);

        inactiveAudioObjects.Last().GetComponent<AudioObject>().ChangeAudioState(AudioState.STOP);
        currentSoundsActive--;
    }

    private IEnumerator MoveGameObjectToActivePool(int number)
    {
        enableNextSound = false;

        string objectName = inactiveAudioObjects[number].gameObject.name;

        if (objectName == "DrippingWater" || objectName == "Shower")
        {
            audioManager.NewWaterRunning();
        }
        else
        {
            audioManager.NewSoundTurnedOn();
        }
        

        activeAudioObjects.Add(inactiveAudioObjects[number]);

        inactiveAudioObjects.RemoveAt(number);

        activeAudioObjects.Last().GetComponent<AudioObject>().ChangeAudioState(AudioState.PLAY);
        currentSoundsActive++;

        //Formule: 50 x 0.3 bijvoorbleed. Hoe meer geluiden er aan staan, hoe meer tijd je krijgt.
        float minTime = (25 * (float)(currentSoundsActive * 0.1f));
        float maxTime = (50 * (float)(currentSoundsActive * 0.1f));

        int cooldownTime = Random.Range((int)minTime, (int)maxTime);

        yield return new WaitForSeconds(cooldownTime);
        enableNextSound = true;
    }
}
