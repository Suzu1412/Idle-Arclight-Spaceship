using UnityEngine;


public static class Vector2Extensions
{
    /// <summary>
    /// Adds to any x y values of a Vector2
    /// </summary>
    public static Vector2 Add(this Vector2 vector2, float x = 0, float y = 0)
    {
        return new Vector2(vector2.x + x, vector2.y + y);
    }

    /// <summary>
    /// Sets any x y values of a Vector2
    /// </summary>
    public static Vector2 With(this Vector2 vector2, float? x = null, float? y = null)
    {
        return new Vector2(x ?? vector2.x, y ?? vector2.y);
    }

    /// <summary>
    /// Checks if the current Vector3 is within the specified range (the distance is squared internally).
    /// </summary>
    public static bool IsWithinRange(this Vector2 from, Vector2 to, float range)
    {
        return (to - from).sqrMagnitude <= range * range;
    }

    /// <summary>
    /// Returns the float distance Magnitude
    /// </summary>
    /// <param name="current"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    public static float GetSquaredDistanceTo(this Vector2 current, Vector2 target)
    {
        return (current - target).sqrMagnitude;
    }

    /// <summary>
    /// Gets the normalized direction from one Vector2 to another.
    /// </summary>
    /// <param name="from">The starting point.</param>
    /// <param name="to">The destination point.</param>
    /// <returns>The normalized direction vector. Returns Vector2.zero if the direction is invalid.</returns>
    public static Vector2 GetDirectionTo(this Vector2 from, Vector2 to)
    {
        Vector2 direction = to - from;
        return direction.sqrMagnitude > Mathf.Epsilon ? direction.normalized : Vector2.zero;
    }

    /// <summary>
    /// Computes a random point in an annulus (a ring-shaped area) based on minimum and 
    /// maximum radius values around a central Vector2 point (origin).
    /// </summary>
    /// <param name="origin">The center Vector2 point of the annulus.</param>
    /// <param name="minRadius">Minimum radius of the annulus.</param>
    /// <param name="maxRadius">Maximum radius of the annulus.</param>
    /// <returns>A random Vector2 point within the specified annulus.</returns>
    public static Vector2 RandomPointInAnnulus(this Vector2 origin, float minRadius, float maxRadius)
    {
        float angle = Random.value * Mathf.PI * 2f;
        Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

        // Squaring and then square-rooting radii to ensure uniform distribution within the annulus
        float minRadiusSquared = minRadius * minRadius;
        float maxRadiusSquared = maxRadius * maxRadius;
        float distance = Mathf.Sqrt(Random.value * (maxRadiusSquared - minRadiusSquared) + minRadiusSquared);

        // Calculate the position vector
        Vector2 position = direction * distance;
        return origin + position;
    }
}
