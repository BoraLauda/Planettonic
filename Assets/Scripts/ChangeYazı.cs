using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class ChangeYazı : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
       [Header("Açılacak Panel")]
        public GameObject hoverlamaPanel; 
    
      
        public Image heartImage;       
        public Sprite boşSprite;     
    
        void OnEnable()
        {
            if (hoverlamaPanel != null) hoverlamaPanel.SetActive(false);
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
           
            if (IsSlotFull())
            {
                if (hoverlamaPanel != null) hoverlamaPanel.SetActive(true);
            }
        }
    
        
        public void OnPointerExit(PointerEventData eventData)
        {
            if (hoverlamaPanel != null) hoverlamaPanel.SetActive(false);
        }
    
     
        bool IsSlotFull()
        {
            return heartImage.sprite != boşSprite;
        }
}

