using Mirror;
using UnityEngine;

namespace KAG.Runtime
{
    public class Character : NetworkBehaviour
    {
        public GameObject brain;

        public void Start()
        {
            Instantiate(brain, transform);
        }
    }
}
