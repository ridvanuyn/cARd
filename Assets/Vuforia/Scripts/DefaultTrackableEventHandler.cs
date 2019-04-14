/*==============================================================================
Copyright (c) 2017 PTC Inc. All Rights Reserved.

Copyright (c) 2010-2014 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.
==============================================================================*/

using UnityEngine;
using Vuforia;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
///using mainSpace;
//using CardInfo;

/// <summary>
/// A custom handler that implements the ITrackableEventHandler interface.
///
/// Changes made to this file could be overwritten when upgrading the Vuforia version.
/// When implementing custom event handler behavior, consider inheriting from this class instead.
/// </summary>
public class DefaultTrackableEventHandler : MonoBehaviour, ITrackableEventHandler
{
    #region PROTECTED_MEMBER_VARIABLES

    protected TrackableBehaviour mTrackableBehaviour;
    protected TrackableBehaviour.Status m_PreviousStatus;
    protected TrackableBehaviour.Status m_NewStatus;
    public static Dictionary<string, double> dictionary = new Dictionary<string, double>();
    public string bestChoiceTrackable = "";

    #endregion // PROTECTED_MEMBER_VARIABLES

    #region UNITY_MONOBEHAVIOUR_METHODS

    protected virtual void Start()
    {
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
    }

    protected virtual void OnDestroy()
    {
        if (mTrackableBehaviour)
            mTrackableBehaviour.UnregisterTrackableEventHandler(this);
    }

    #endregion // UNITY_MONOBEHAVIOUR_METHODS

    #region PUBLIC_METHODS

    /// <summary>
    ///     Implementation of the ITrackableEventHandler function called when the
    ///     tracking state changes.
    /// </summary>
    public void OnTrackableStateChanged(
        TrackableBehaviour.Status previousStatus,
        TrackableBehaviour.Status newStatus)
    {
        m_PreviousStatus = previousStatus;
        m_NewStatus = newStatus;

        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED ||
            newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " found");

            if (!dictionary.ContainsKey(mTrackableBehaviour.TrackableName))
            {
                dictionary.Add(mTrackableBehaviour.TrackableName + "", 0);
                Debug.Log("ekledim");
            }

            OnTrackingFound();
        }
        else if (previousStatus == TrackableBehaviour.Status.TRACKED &&
                 newStatus == TrackableBehaviour.Status.NO_POSE)
        {
            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");
            OnTrackingLost();
        }
        else
        {
            // For combo of previousStatus=UNKNOWN + newStatus=UNKNOWN|NOT_FOUND
            // Vuforia is starting, but tracking has not been lost or found yet
            // Call OnTrackingLost() to hide the augmentations
            OnTrackingLost();
        }
    }

    #endregion // PUBLIC_METHODS

    #region PROTECTED_METHODS

    protected virtual void OnTrackingFound()
    {
        KuveytApi api = KuveytApi.GetInstance();
        List<string> cardNumbers = api.getCreditCardNumbers();
        api.decideBestChoice(cardNumbers);
        

        var cardMap = new Dictionary<string, string>
        {
            { "altinKart", "4025916319964789" },
            { "saglamKart", "4025916319964780" }
        };

        if (KuveytApi.BestChoice.CardNumber.Equals(cardMap[mTrackableBehaviour.TrackableName]))
        {
            this.bestChoiceTrackable = mTrackableBehaviour.TrackableName;
        }

        var rendererComponents = GetComponentsInChildren<Renderer>(true);
        var colliderComponents = GetComponentsInChildren<Collider>(true);
        var canvasComponents = GetComponentsInChildren<Canvas>(true);

        // Enable rendering:
        foreach (var component in rendererComponents)
            component.enabled = true;
        // Enable colliders:
        foreach (var component in colliderComponents)

            component.enabled = true;

        // Enable canvas':
        foreach (var component in canvasComponents)
            component.enabled = true;


        if (mTrackableBehaviour.TrackableName == "altinKart")
        {
            CardInformation card = api.getCreditCardInformation("4025916319964789");
            dictionary[mTrackableBehaviour.TrackableName] = card.Limit;
        }
        else
        {
            CardInformation card = api.getCreditCardInformation("4025916319964780");
            dictionary[mTrackableBehaviour.TrackableName] = card.Limit;
        }

        Debug.Log("Bu arkadaşı listeye ekledim " + mTrackableBehaviour.TrackableName);
        Debug.Log(dictionary[mTrackableBehaviour.TrackableName]);

        Debug.Log("###########");
        Debug.Log(mTrackableBehaviour.TrackableName);
        Debug.Log(this.bestChoiceTrackable);

        Debug.Log("###########");

        if (mTrackableBehaviour.TrackableName.Equals(this.bestChoiceTrackable))
        {
            Debug.Log("ooo beeeest");
        }

        //    cardInfo.changeCardInfo(cardInfos.Limit);

    }


    protected virtual void OnTrackingLost()
    {
        var rendererComponents = GetComponentsInChildren<Renderer>(true);
        var colliderComponents = GetComponentsInChildren<Collider>(true);
        var canvasComponents = GetComponentsInChildren<Canvas>(true);

        // Disable rendering:
        foreach (var component in rendererComponents)
            component.enabled = false;

        // Disable colliders:
        foreach (var component in colliderComponents)
            component.enabled = false;

        // Disable canvas':
        foreach (var component in canvasComponents)
            component.enabled = false;
    }

    #endregion // PROTECTED_METHODS
}

class ReverseComparer<T> : IComparer<T>
{
    private readonly IComparer<T> original;

    public ReverseComparer(IComparer<T> original)
    {
        this.original = original;
    }

    public int Compare(T left, T right)
    {
        return original.Compare(right, left);
    }
}