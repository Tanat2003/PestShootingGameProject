using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mission_ObjectCarToDeliver : MonoBehaviour
{
    public static event Action OnCarDelivery;

    public void InvokeOnCarDelivery() => OnCarDelivery?.Invoke();


}
