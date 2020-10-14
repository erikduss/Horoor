using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private List<AudioClip> duringComfortSounds = new List<AudioClip>();
    [SerializeField] private List<AudioClip> needingComfortSounds = new List<AudioClip>();
    [SerializeField] private List<AudioClip> soundTurnedOnSounds = new List<AudioClip>();

    [SerializeField] private List<AudioClip> storyLines = new List<AudioClip>();

    [SerializeField] private AudioClip doorbell;

    [SerializeField] private AudioClip waterTurnedOn;

    [SerializeField] private AudioClip introSound;

    [SerializeField] private AudioClip firstTimeLivingRoom;

    [SerializeField] private AudioClip alarmBedroom;

    [SerializeField] private AudioClip gameOverSound;
    [SerializeField] private AudioClip halfwaySound;
    [SerializeField] private AudioClip gameCompleteSound;

    [SerializeField] private AudioSource voiceListener;
    [SerializeField] private AudioSource childVoiceSource;

    private List<AudioClip> audioQueue = new List<AudioClip>();

    private bool waitingForNext = false;

    private void Update()
    {
        if(audioQueue.Count > 0 && !voiceListener.isPlaying && !waitingForNext)
        {
            StartCoroutine(playQueuedAudio());
        }
    }

    private IEnumerator playQueuedAudio()
    {
        waitingForNext = true;
        yield return new WaitForSeconds(2);
        if(audioQueue[0].name == "katey") voiceListener.volume = .4f;
        voiceListener.PlayOneShot(audioQueue[0]);
        yield return new WaitForSeconds(2);
        audioQueue.RemoveAt(0);
        waitingForNext = false;
    }

    public void PlayStoryLine()
    {
        int number = Random.Range(0,storyLines.Count);

        if (!voiceListener.isPlaying)
        {
            if (storyLines[number].name == "katey") voiceListener.volume = .4f;
            else voiceListener.volume = 1f;
            voiceListener.PlayOneShot(storyLines[number]);
        }
        else
        {
            audioQueue.Add(storyLines[number]);
        }

        storyLines.RemoveAt(number);
    }

    public void PlayIntroSound()
    {
        voiceListener.PlayOneShot(introSound);
    }

    public void GameOver()
    {
        audioQueue.Clear();
        voiceListener.Stop();
        waitingForNext = true;
        voiceListener.volume = .8f;
        voiceListener.PlayOneShot(gameOverSound);
    }

    public void GameComplete()
    {
        audioQueue.Clear();
        waitingForNext = true;
        voiceListener.Stop();
        voiceListener.PlayOneShot(gameCompleteSound);
    }

    public void DoorbellSound()
    {
        audioQueue.Clear();
        waitingForNext = true;
        voiceListener.Stop();
        voiceListener.PlayOneShot(doorbell);
    }

    public void GameHalfway()
    {
        if (!voiceListener.isPlaying)
        {
            voiceListener.PlayOneShot(halfwaySound);
        }
        else
        {
            audioQueue.Add(halfwaySound);
        }
    }

    public void NewSoundTurnedOn()
    {
        int number = Random.Range(0, soundTurnedOnSounds.Count);
        childVoiceSource.PlayOneShot(soundTurnedOnSounds[number]);
    }

    public void NewWaterRunning()
    {
        childVoiceSource.PlayOneShot(waterTurnedOn);
    }

    public void UsingComfort()
    {
        int number = Random.Range(0, duringComfortSounds.Count);
        voiceListener.Stop();
        voiceListener.PlayOneShot(duringComfortSounds[number]);
    }

    public void NeedComfort()
    {
        int number = Random.Range(0, needingComfortSounds.Count);

        if (!voiceListener.isPlaying)
        {
            voiceListener.PlayOneShot(needingComfortSounds[number]);
        }
        else
        {
            audioQueue.Add(needingComfortSounds[number]);
        }
    }

    public void FirstTimeLivingRoom()
    {
        voiceListener.PlayOneShot(firstTimeLivingRoom);
    }
}
