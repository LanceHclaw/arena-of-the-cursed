using System;
using UnityEngine;

public class StatusEffects : MonoBehaviour
{
    [Flags]
    public enum Conditions
    {
        None = 0,
        Crippled = 1,
        Stunned = 2,
        Quickened = 4
    }
}