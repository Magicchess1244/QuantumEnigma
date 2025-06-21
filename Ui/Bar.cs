using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
    public float FillAmount;
    public float TotalFillAmount;

    private Image image;
    private void Start()
    {
        image = GetComponent<Image>();
    }
    private void Update()
    {
        image.fillAmount = FillAmount / TotalFillAmount;
    }
    private void Fill(float Amount)    
    {
        FillAmount += Amount;
    }
}
