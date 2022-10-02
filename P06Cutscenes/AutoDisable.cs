using UnityEngine;

namespace P06Cutscenes
{
    public class AutoDisable : MonoBehaviour
    {
        private int c = 5;
        private bool inArea;

        public GameObject target;
        public AudioClip respawnBGM;

        private AudioSource mainSource;

        private bool triggerEx = true;
        private bool triggerStay;

        private void Start()
        {
            mainSource = GameObject.Find("Stage").GetComponent<AudioSource>();
        }

        private void Update()
        {
            if (inArea)
            {
                return;
            }

            if (c > 0)
            {
                c--;
            } else if (c == 0)
            {
                c = -1;
                target.SetActive(false);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                inArea = true;
                target.SetActive(true);

                c = 5;

                triggerEx = false;

                if (mainSource.time < 1)
                {
                    mainSource.clip = respawnBGM;
                    mainSource.Play();
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "Player")
            {
                inArea = false;
                triggerEx = true;
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.tag == "Player" && triggerEx == true)
            {
                triggerEx = false;
                inArea = true;
            }
        }
    }
}
