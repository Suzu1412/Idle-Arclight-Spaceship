using UnityEngine;

public class FighterContext : StateContext
{
    private float _minDelayBeforeAttack = 1f;
    private float _maxDelayBeforeAttack = 5f;
    private float _delayBeforeAttack;

    public override void ResetContext()
    {
        base.ResetContext();
        ResetShotTimer();
    }

    public override void Tick()
    {
        base.Tick();
        _delayBeforeAttack -= Time.deltaTime;
    }

    public void ResetShotTimer()
    {
        _delayBeforeAttack = Random.Range(_minDelayBeforeAttack, _maxDelayBeforeAttack);
    }

    internal bool CanAttack() => _delayBeforeAttack <= 0;
}

