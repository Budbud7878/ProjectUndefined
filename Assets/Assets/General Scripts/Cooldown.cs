using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class Cooldown
{
    #region VARIABLES
    [SerializeField] private float CoolDownTime;
    private float _nextOn;

    public bool IsCoolingDown => Time.time < _nextOn;
    public void StartCoolDown() => _nextOn = Time.time + CoolDownTime;

    #endregion
}

