using UnityEngine;

public class DönmeBG : MonoBehaviour
{
    
    [Header("ARKAPLAN HIZ VS")]
        public float rotateSpeed = 30f; 
        public bool randomStart = true; 
        void Start()
        {
            if (randomStart)
            {
                float randomAngle = Random.Range(0f, 360f);
                transform.localRotation = Quaternion.Euler(0, 0, randomAngle);
            }
        }
    
        void Update()
        { 
            transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);
            
            
        }
        
}


