using UnityEngine;

[HelpURL("https://coffeebraingames.wordpress.com/2014/10/20/a-generic-duration-timer-class/")]
public class DurationTimer
{
    private float polledTime;
    private float durationTime;

    /**
     * Constructor with a specified duration time
     */
    public DurationTimer(float durationTime)
    {
        Reset(durationTime);
    }

    /**
     * Updates the timer
     */
    public void UpdateTime()
    {
        this.polledTime += Time.deltaTime;
    }

    /**
     * Resets the timer
     */
    public void Reset()
    {
        this.polledTime = 0;
    }

    /**
     * Resets the timer and assigns a new duration time
     */
    public void Reset(float durationTime)
    {
        Reset();
        this.durationTime = durationTime;
    }

    /**
     * Returns whether or not the timed duration has elapsed
     * Use for End Timer
     */
    public bool HasElapsed()
    {
        return FloatComparison.TolerantGreaterThanOrEquals(this.polledTime, this.durationTime);
    }

    /**
     * Returns the ratio of polled time to duration time. Returned value is 0 to 1 only
     * Use for Interpolation (Lerp) 
     */
    public float GetRatio()
    {
        if (FloatComparison.TolerantLesserThanOrEquals(this.durationTime, 0))
        {
            // bad duration time value
            // if countdownTime is zero, ratio will be infinity (divide by zero)
            // we just return 1.0 here for safety
            return 1.0f;
        }

        float ratio = this.polledTime / this.durationTime;
        return Mathf.Clamp(ratio, 0, 1);
    }

    /**
     * Returns the polled time since it started
     */
    public float GetPolledTime()
    {
        return this.polledTime;
    }

    /**
     * Forces the timer to end
     */
    public void EndTimer()
    {
        this.polledTime = this.durationTime;
    }

    /**
     * Returns the durationTime
     */
    public float GetDurationTime()
    {
        return this.durationTime;
    }
}