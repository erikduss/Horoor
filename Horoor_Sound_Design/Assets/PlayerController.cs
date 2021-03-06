﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform planet;
    [SerializeField] private Text objectsLeftText;
    [SerializeField] private GameObject blackPanel;
    private GameManager gameManager;

    private int objectsTurnedOff = 0;
    private Rigidbody rbPlayer;

    float gravityForce = 10;

    private GameObject objectInReach;

    // Start is called before the first frame update
    void Start()
    {
        Physics.gravity = Vector3.zero;
        rbPlayer = GetComponent<Rigidbody>();
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    void FixedUpdate()
    {
        Vector3 gravityDirection = (planet.position - transform.position).normalized;
        rbPlayer.AddForce(gravityDirection * gravityForce);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            AttemptDestroyObjectInReach();
        }

        if (Input.GetKeyDown(KeyCode.Equals))
        {
            TogglePanel();
        }

        objectsLeftText.text = "Active sounds: " + gameManager.currentSoundsActive;
    }

    public void ChangeAttachedRoom(Transform newPlanet)
    {
        planet = newPlanet;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != null)
        {
            objectInReach = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        objectInReach = null;
    }

    private void TogglePanel()
    {
        if (blackPanel.activeInHierarchy)
        {
            blackPanel.SetActive(false);
        }
        else
        {
            blackPanel.SetActive(true);
        }
    }

    private void AttemptDestroyObjectInReach()
    {
        if (objectInReach != null)
        {
            gameManager.MoveGameobjectToInactive(objectInReach.gameObject);
            //gameManager.removeSoundSource();
            //Destroy(objectInReach.gameObject);
            objectInReach = null;
            objectsTurnedOff++;

            objectsLeftText.text = "Active sounds: " + gameManager.currentSoundsActive;
        }
    }
}
