using UnityEngine;

namespace TimelineHero.MapCamera
{
    public class MapCameraController : MonoBehaviour
    {
        // Mouse buttons in the same order as Unity
        public enum MouseButton { Left = 0, Right = 1, Middle = 2, None = 3 }
        
        [System.Serializable]
        // Handles common parameters for translations and rotations
        public class MouseControlConfiguration
        {
 
            public bool activate;
            public MouseButton mouseButton;
            public float sensitivity;
 
            public bool isActivated()
            {
                return activate && Input.GetMouseButton((int)mouseButton);
            }
        }
        
        private Camera mainCamera;

        // Vertical translation default configuration
        public MouseControlConfiguration verticalTranslation = 
            new MouseControlConfiguration { mouseButton = MouseButton.Left, sensitivity = 2F };
 
        // Horizontal translation default configuration
        public MouseControlConfiguration horizontalTranslation = 
            new MouseControlConfiguration { mouseButton = MouseButton.Left, sensitivity = 2F };
        
        public string mouseHorizontalAxisName = "Mouse X";
        public string mouseVerticalAxisName = "Mouse Y";
        
        private void Awake()
        {
            mainCamera = Camera.main;
        }

        private void LateUpdate()
        {
            if (verticalTranslation.isActivated())
            {
                float translateY = Input.GetAxis(mouseVerticalAxisName) * verticalTranslation.sensitivity;
                transform.Translate(0, -translateY, 0);
            }
 
            if (horizontalTranslation.isActivated())
            {
                float translateX = Input.GetAxis(mouseHorizontalAxisName) * horizontalTranslation.sensitivity;
                transform.Translate(-translateX, 0, 0);
            }
        }
    }
}