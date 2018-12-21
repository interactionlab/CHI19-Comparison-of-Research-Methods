using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour {

    public AudioSource audio;
    public GameObject audioContoll;

    public MillMotion millMotionPepper;
    public MillMotion millMotionSalt;

    private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;
    private Valve.VR.EVRButtonId touchPad = Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad;

    private SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObj.index); } }
    private SteamVR_TrackedObject trackedObj;

    private GameObject obj = null;
    private ObjectType objectType = ObjectType.None;

    enum ObjectType
    {
        None,
        Coffee,
        Button,
        Speaker,
        Mill
    };

    // Use this for initialization
    void Start () {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (controller == null)
        {
            Debug.Log("Trigger: Controller not initialized - Trigger");
            return;
        }
        
        var device = SteamVR_Controller.Input((int)trackedObj.index);

        if (objectType == ObjectType.None)
            return;


        if (controller.GetPressDown(triggerButton))
        {
            if (objectType == ObjectType.Coffee)
            {
                TriggerParticalSystem s = obj.GetComponent<TriggerParticalSystem>();
                if (s != null)
                    s.play();
            }
            else if (objectType == ObjectType.Button)
            {
                if (obj.name == "Plus")
                {
                    Debug.Log("Trigger: Volume Up");
                    audio.volume = audio.volume + .1f;

                }
                else if (obj.name == "Minus")
                {
                    Debug.Log("Trigger: Volume Down");
                    audio.volume = audio.volume - .1f;
                }
                else if (obj.name == "PlayPause")
                {
                    if (audio.isPlaying)
                        audio.Stop();
                    else
                        audio.Play();
                }
            }
            else if (objectType == ObjectType.Speaker)
            {
                if (audioContoll.activeSelf)
                {
                    audioContoll.active = false;
                }
                else
                {
                    audioContoll.active = true;
                }
            }
        }
        if (controller.GetPressDown(touchPad) || controller.GetPressUp(touchPad))
        {
            if (objectType == ObjectType.Mill) {
                if (obj.name == "Saltmill" && millMotionSalt != null)
                {
                    millMotionSalt.toggleOpen();
                }
                else if (obj.name == "Peppermill" && millMotionPepper != null) {
                    millMotionPepper.toggleOpen();
                }
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        ObjectType objectTypeTemp = ObjectType.None;
        if (other.gameObject.name == "Coffee")
        {
            objectTypeTemp = ObjectType.Coffee;
        }
        else if (other.CompareTag("Button"))
        {
            objectTypeTemp = ObjectType.Button;
        }
        else if (other.gameObject.name == "Speaker") {
            objectTypeTemp = ObjectType.Speaker;
        }
        else if (other.gameObject.name == "Peppermill" || other.gameObject.name == "Saltmill")
        {
            objectTypeTemp = ObjectType.Mill;
        }


        if (objectTypeTemp != ObjectType.None)
        {
            Debug.Log("Trigger: " + other.name);
            objectType = objectTypeTemp;
            obj = other.gameObject;
        } else
        {
            obj = null;
            objectType = ObjectType.None;
        }
        
    }
}
