using UnityEngine;

public class Hook : MonoBehaviour
{
   [SerializeField] private SpriteRenderer childSpriteRenderer;
   
   [SerializeField] private Color hookColor;
   [SerializeField] private Color swingColor;

   
   public void SetSpriteColor(bool isSwinging)
   {
      childSpriteRenderer.color = isSwinging ? swingColor : hookColor;
   }
   
}
