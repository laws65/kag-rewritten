using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KAG.Menu
{
    using KAG.Misc;

    public class Toast : Singleton<Toast>
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

        public void ShowError(string text)
        {
            message.text = "Error: " + text;

            anim.Stop();
            anim.Play();
        }
    }
}
