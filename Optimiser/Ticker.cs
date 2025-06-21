using UnityEngine;

public class Ticker : MonoBehaviour
{
    public static float TickDuration = 0.2f;
    private float Tick = 0;

    public delegate void TickCounter();
    public static TickCounter OnTick;
    private void OnEnable()
    {
        GlobalManager.GameLoop += Counter;
    }
    private void OnDisable()
    {
        GlobalManager.GameLoop -= Counter;
    }
    private void Counter()
    {
        Tick += Time.deltaTime;
        if (Tick >= TickDuration)
        {
            Tick = 0;
            OnTick?.Invoke();
        }
    }
}
