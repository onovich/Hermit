using UnityEngine;

namespace Polyperfect.War
{
    public class CursorLocker : MonoBehaviour
    {
        public KeyCode LockingKey = KeyCode.Return;
        void Start()
        {
            LockCursor();
        }
        void Update()
        {
            if (Input.GetKeyDown(LockingKey))
            {
                if (Cursor.lockState == CursorLockMode.Locked)
                    UnlockCursor();
                else
                    LockCursor();
            }
        }
        public void LockCursor()=>Cursor.lockState = CursorLockMode.Locked;
        public void UnlockCursor()=>Cursor.lockState = CursorLockMode.None;

        public static Vector2 CursorPosition()
        {
            if (Cursor.lockState == CursorLockMode.Locked)
                return new Vector2(Screen.width * .5f, Screen.height * .5f);
            return Input.mousePosition;
        }
    }
}