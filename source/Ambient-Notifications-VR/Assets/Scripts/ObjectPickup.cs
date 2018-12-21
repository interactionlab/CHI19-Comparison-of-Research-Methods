using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//-------------------------------------------------------------------------------------------------\\
// This code has been developed by Feisty Crab Studios for personal, commercial, and education use.\\
//                                                                                                 \\
// You are free to edit and redistribute this code, subject to the following:                      \\
//                                                                                                 \\
//      1. You will not sell this code or an edited version of it.                                 \\
//      2. You will not remove the copyright messages                                              \\
//      3. You will give credit to Feisty Crab Studios if used commercially                        \\
//      4. Don't be a mean sausage, nobody likes a mean sausage.                                   \\
//                                                                                                 \\
// Contact us @ feistycrabstudios.gmail.com with any questions.                                    \\
//-------------------------------------------------------------------------------------------------\\

public class ObjectPickup : MonoBehaviour { 

    public MillMotion millMotionPepper;
    public MillMotion millMotionSalt;

    private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;

    private SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObj.index); } }
    private SteamVR_TrackedObject trackedObj;

    private GameObject obj;
    private FixedJoint fJoint;

    private bool throwing;
    private Rigidbody rigidbody;

    void Start()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();

        fJoint = GetComponent<FixedJoint>();
    }

    void Update()
    {
        if (controller == null)
        {
            Debug.Log("ObjectPickup: Controller not initialized");
            return;
        }

        var device = SteamVR_Controller.Input((int)trackedObj.index);

        if (controller.GetPressDown(triggerButton))
        {
            if (obj != null && obj.name == "Saltmill")
            {
                millMotionSalt.toggleMilling();
            }
            else if (obj != null && obj.name == "Peppermill")
            {
                millMotionPepper.toggleMilling();
            }
            PickUpObj();
        }

        if (controller.GetPressUp(triggerButton))
        {
            if (obj != null && obj.name == "Saltmill")
            {
                millMotionSalt.toggleMilling();
            }
            else if (obj != null && obj.name == "Peppermill")
            {
                millMotionPepper.toggleMilling();
            }
            DropObj();
        }
    }

    void FixedUpdate()
    {
        if (throwing)
        {
            Transform origin;
            if (trackedObj.origin != null)
            {
                origin = trackedObj.origin;
            }
            else
            {
                origin = trackedObj.transform.parent;
            }

            if (origin != null)
            {
                rigidbody.velocity = origin.TransformVector(controller.velocity);
                rigidbody.angularVelocity = origin.TransformVector(controller.angularVelocity * 0.25f);
            }
            else
            {
                rigidbody.velocity = controller.velocity;
                rigidbody.angularVelocity = controller.angularVelocity * 0.25f;
            }

            rigidbody.maxAngularVelocity = rigidbody.angularVelocity.magnitude;

            throwing = false;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Pickupable"))
        {
            Debug.Log("ObjectPickup: Collider");
            obj = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        obj = null;
    }

    void PickUpObj()
    {
        if (obj != null)
        {
            fJoint.connectedBody = obj.GetComponent<Rigidbody>();

            throwing = false;
            rigidbody = null;
        }
        else
        {
            fJoint.connectedBody = null;
        }
    }

    void DropObj()
    {
        if (fJoint.connectedBody != null)
        {
            rigidbody = fJoint.connectedBody;

            fJoint.connectedBody = null;

            throwing = true;
        }
    }
}