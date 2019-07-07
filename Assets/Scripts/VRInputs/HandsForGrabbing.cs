using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class HandsForGrabbing : MonoBehaviour
{

    public SteamVR_Action_Boolean m_GrabAction = null;

    private SteamVR_Behaviour_Pose m_Pose = null;
    private FixedJoint m_Joint = null;

    private Interactable m_CurrentInteractable = null;
    private List<Interactable> m_ContactInteractable = new List<Interactable>();

    private void Awake()
    {
        m_Pose = GetComponent<SteamVR_Behaviour_Pose>();
        m_Joint = GetComponent<FixedJoint>();
    }

    // Update is called once per frame
    void Update()
    {
        // down
        if (m_GrabAction.GetStateDown(m_Pose.inputSource))
        {
            Debug.Log("lksjhdf");
            Pickup();
        }

        // Up

        if (m_GrabAction.GetStateUp(m_Pose.inputSource))
        {
            Debug.Log("3987465389745");
            Drop();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Key01"))
            return;
        m_ContactInteractable.Add(other.gameObject.GetComponent<Interactable>());
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Key01"))
            return;
        m_ContactInteractable.Remove(other.gameObject.GetComponent<Interactable>());
    }

    public void Pickup()
    {
        // get nearest interactable
        m_CurrentInteractable = GetNearestInteractable();


        // null check
        if (!m_CurrentInteractable)
            return;

        // already held check
        if (m_CurrentInteractable.m_ActiveHand)
            m_CurrentInteractable.m_ActiveHand.Drop();

        // position
        m_CurrentInteractable.transform.position = transform.position;

        //attach
        Rigidbody targetBody = m_CurrentInteractable.GetComponent<Rigidbody>();
        m_Joint.connectedBody = targetBody;

        // Set active Hand
        m_CurrentInteractable.m_ActiveHand = this;
    }

    public void Drop()
    {
        // Null check
        if (!m_CurrentInteractable)
            return;

        // apply Velocuty
        Rigidbody targetBody = m_CurrentInteractable.GetComponent<Rigidbody>();
        targetBody.velocity = m_Pose.GetVelocity();
        targetBody.angularVelocity = m_Pose.GetAngularVelocity();

        // detach
        m_Joint.connectedBody = null;

        // clear
        m_CurrentInteractable.m_ActiveHand = null;
        m_CurrentInteractable = null;
    }

    private Interactable GetNearestInteractable()
    {
        Interactable nearest = null;
        float minDistance = float.MaxValue;
        float distance = 0.0f;

        foreach(Interactable interactable in m_ContactInteractable)
        {
            distance = (interactable.transform.position - transform.position).sqrMagnitude;
            if (distance < minDistance)
            {
                minDistance = distance;
                nearest = interactable;
            }
        }

        return nearest;
    }
}
