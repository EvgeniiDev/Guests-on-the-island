using UnityEngine;

namespace Assets.Scripts.UI
{
    internal class Shop : MonoBehaviour
    {
        public Canvas Menu;

        private void Start()
        {
            Menu.gameObject.SetActive(false);
        }

        public void ChangeState()
        {
            Menu.gameObject.SetActive(!Menu.gameObject.active);
        }
    }
}
