using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

public class ButtonVR : MonoBehaviourPun
{
    public GameObject button;
    public UnityEvent onPress;
    public UnityEvent onRelease;
    GameObject presser;
    //AudioSource sound;
    bool isPressed = false;

    Keyboard keyboard;
    GameObject buttonPressed;

    void Start()
    {
        //sound = GetComponent<AudioSource>();
        isPressed = false;
        keyboard = GetComponentInParent<Keyboard>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isPressed)
        {
            Debug.Log("Touched key");
            button.transform.localPosition = new Vector3(0, 0.003f, 0);
            presser = other.gameObject;
            onPress.Invoke();
            //sound.Play();
            if(GameState.Instance.OfflineMode)
            {
                UpdateInput();
            }
            else
            {
            photonView.RPC("UpdateInput", RpcTarget.All);
            }
            //UpdateInput();
            isPressed = true;


        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == presser)
        {
            button.transform.localPosition = new Vector3(0, 0.015f, 0);
            //onRelease.Invoke();
            isPressed = false;
        }
    }

    [PunRPC]
    public void UpdateInput()
    {
            buttonPressed = transform.parent.gameObject;

            if (buttonPressed.name.Length == 1 || buttonPressed.name == "Space")
            {
                Debug.Log("Button pressed:" + buttonPressed.name);
                keyboard.AddToInput(buttonPressed.name);
            }
            else if(buttonPressed.name == "Enter")
            {
                keyboard.Submit();
            }
            else
            {
                keyboard.Backspace();
            }
    }

}
