using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creaking_Door : MonoBehaviour
{
    [SerializeField] private List<AudioClip> doorCreaks = new List<AudioClip>();
    [SerializeField] private AudioSource audioPlayer;
    private bool allowCreaking = true;

    private void Update()
    {
        if (allowCreaking) StartCoroutine(CreakDoor());
    }

    private IEnumerator CreakDoor()
    {
        allowCreaking = false;

        int _randNumber = Random.Range(0, doorCreaks.Count);
        AudioClip _currentClip = doorCreaks[_randNumber];
        audioPlayer.PlayOneShot(_currentClip);

        int _creakDelay = Random.Range(10, 20);
        yield return new WaitForSeconds(_creakDelay);
        allowCreaking = true;
    }
}
