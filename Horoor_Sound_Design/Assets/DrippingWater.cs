using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrippingWater : MonoBehaviour
{
    [SerializeField] private List<AudioClip> waterDrips = new List<AudioClip>();
    [SerializeField] private AudioSource audioPlayer;
    private bool allowDripping = true;

    private void Update()
    {
        if(allowDripping) StartCoroutine(DripWater());
    }

    private IEnumerator DripWater()
    {
        allowDripping = false;

        int _randNumber = Random.Range(0, waterDrips.Count);
        AudioClip _currentClip = waterDrips[_randNumber];
        audioPlayer.PlayOneShot(_currentClip);

        int _dripDelay = Random.Range(2, 4);
        yield return new WaitForSeconds(_dripDelay);
        allowDripping = true;
    }
}
