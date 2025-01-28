using UnityEngine;

[System.Serializable]
public class MeteorContext : StateContext
{
    internal float BounceCooldown = 0f;

    public override void ResetContext()
    {
        base.ResetContext();
        BounceCooldown = 0f;
    }

    public void BounceTick()
    {
        BounceCooldown -= Time.deltaTime;

        if (BounceCooldown <= 0f)
        {
            BounceCooldown = 0f;
        }
    }
}
