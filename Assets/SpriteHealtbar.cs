using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteHealtbar : MonoBehaviour 
{
    public SpriteRenderer HealthSpriteRenderer;

    public void SetHealth(float health)
    {
        if (health < 0)
        {
            health = 0;
        }
        
        HealthSpriteRenderer.size = new Vector2(health * 10, HealthSpriteRenderer.size.y);
    }
}
