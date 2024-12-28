using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealable
{
    event Action OnHeal;
    void Heal(int amount);
}
