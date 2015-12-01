using UnityEngine;
using System.Collections;

public class HealthText : MonoBehaviour
{
  public Health healthComponent;

  public GUIBarScript HealthBar;
  public TextMesh HealthHoverText;
  void Awake()
  {
    if (healthComponent)
      healthComponent.OnSetHealth += SetText;
  }

  void SetText(float health)
  {
    var DisplayVal = "<color=red> Health: " + health.ToString("0") + "/" + healthComponent.maxHealth + "</color>";

    if (HealthHoverText != null) HealthHoverText.text = DisplayVal;
    if (HealthBar != null) HealthBar.Value = health / healthComponent.maxHealth;
    //GetComponent<TextMesh>().text = "<color=red> Health: " + health.ToString("0") + "/" + healthComponent.maxHealth + "</color>";
  }
}
