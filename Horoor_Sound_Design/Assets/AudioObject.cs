using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AudioState
{
    STOP,
    PLAY
}

public class AudioObject : MonoBehaviour
{
    [SerializeField] private List<AudioClip> objectAudioClip;
    [SerializeField] private bool useDelay;
    [SerializeField] private int minDelay;
    [SerializeField] private int maxDelay;
    [SerializeField] private float clipVolume;

    private AudioSource objectAudioPlayer;
    private bool allowAudio = true;

    private bool isDisabled = true;

    private void Awake()
    {
        objectAudioPlayer = GetComponent<AudioSource>();
        objectAudioPlayer.volume = clipVolume;
        if (!useDelay)
        {
            objectAudioPlayer.loop = true;
            objectAudioPlayer.clip = objectAudioClip[0];
            objectAudioPlayer.Play();
        }
    }

    public void ChangeAudioState(AudioState state)
    {
        if(state == AudioState.PLAY)
        {
            objectAudioPlayer.Play();
            isDisabled = false;
        }
        else if (state == AudioState.STOP)
        {
            objectAudioPlayer.Stop();
            isDisabled = true;
        }
    }

    private void Update()
    {
        if (allowAudio && useDelay && !isDisabled) StartCoroutine(PlayDelayedSound());
    }

    private IEnumerator PlayDelayedSound()
    {
        allowAudio = false;

        int _randNumber = Random.Range(0, objectAudioClip.Count);
        AudioClip _currentClip = objectAudioClip[_randNumber];

        objectAudioPlayer.PlayOneShot(_currentClip);

        int _interferenceDelay = Random.Range(minDelay, maxDelay);
        yield return new WaitForSeconds(_interferenceDelay);
        allowAudio = true;
    }
}
