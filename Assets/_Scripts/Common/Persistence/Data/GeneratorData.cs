using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GeneratorData
{
    public string Guid;
    public int Amount;

    public void NewGame()
    {
        Amount = 0;
    }

}
