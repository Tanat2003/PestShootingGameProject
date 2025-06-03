using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mission_ObjectHuntTarget : MonoBehaviour
{
    public static event Action OnTargetKill;

    public void InvokeOnTargetKill() =>  OnTargetKill?.Invoke();


}
