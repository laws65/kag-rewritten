using TMPro;
using UnityEngine;

namespace KAG.Menu
{
    public class Toast : MonoBehaviour
    {
        public TextMeshProUGUI message;
        Animation anim;

        private void Awake()
        {
            anim = GetComponent<Animation>();
        }

        public void Show(string text)
        {
            message.text = text;

            anim.Stop();
            anim.Play();
        }
    }
}
