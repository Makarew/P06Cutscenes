using System.Collections.Generic;
using UnityEngine;

namespace P06Cutscenes
{
    public class Cutscene : MonoBehaviour
    {
        [Header("Cutscene Playables")]
        public Animator[] animators;
        public AnimationClip[] clips;

        public AudioClip[] sounds;
        public float[] soundStartTimes;
        private List<bool> isPlaying;
        private List<AudioSource> sources;

        public GameObject camera;
        //private PlayerCamera playerCamera;

        //public GameObject[] objectSwap;

        [Header("Audio Control")]
        [Tooltip("0 = Stop BGM And Restart After Cutscene; 1 = Volume Is 0; 2 = Fade Out; 3 = Don't Stop")]
        public int audioSimmerMode;
        private float bgmVal;

        [Tooltip("Add an audio clip to change the BGM after the cutscene, leave empty to keep the BGM the same")]
        public AudioClip changeBGM;
        [Tooltip("Plays 'Change BGM' when enabled; Set Audio Simmer Mode to 3")]
        public bool playAtStart;

        [Header("Post cutscene setup")]
        [Tooltip("If a transform is added, the current character will be teleported there after the cutscene")]
        public Transform tpPoint;

        [Tooltip("Enable objects when the cutscene is over")]
        public GameObject[] toDisable;
        [Tooltip("Disable objects when the cutscene is over")]
        public GameObject[] toEnable;

        [Tooltip("Play another cutscene after this cutscene ends; Good for cutscenes spanning multiple areas")]
        public Cutscene nextCutscene;

        //[Tooltip("Change to mach speed after cutscene")]
        public enum newCharacter
        {
            keep,
            sonic_new,
            sonic_fast,
            snow_board,
            princess,
            shadow,
            silver,
            tails,
            knuckles,
            rouge,
            omega
        }

        public newCharacter changeCharacter;

        private float timer;
        private float maxTime;
        private bool started;
        private bool animReset;

        [HideInInspector]
        public GameObject player;
        private AudioSource mainSource;

        private List<GameObject> uiObjects;
        //private GameManager gameManager;
        private float igtime;

        private void Start()
        {
            sources = new List<AudioSource>();
            isPlaying = new List<bool>();

            for (int i = 0; i < clips.Length; i++)
            {
                if  (clips[i].length > maxTime)
                {
                    maxTime = clips[i].length;
                }
            }

            mainSource = GameObject.Find("Stage").GetComponent<AudioSource>();

            uiObjects = GetObjectsOnLayer(5, "Finish");

            //gameManager = FindObjectOfType<GameManager>();
        }

        public void StartCutscene()
        {
            for (int i = 0; i < animators.Length; i++)
            {
                animators[i].SetBool("start", true);
            }

            for (int i = 0; i < sounds.Length; i++)
            {
                GameObject newsource = Instantiate(new GameObject(), transform);
                sources.Add(newsource.AddComponent<AudioSource>());
                sources[i].playOnAwake = false;
                sources[i].loop = false;
                sources[i].clip = sounds[i];
                isPlaying.Add(false);
            }

            player.SetActive(false);

            bgmVal = mainSource.volume;

            switch (audioSimmerMode)
            {
                case 0:
                    mainSource.Stop();
                    break;
                case 1:
                    mainSource.volume = 0;
                    break;
                case 2:
                    break;
                case 3:
                    break;
            }

            if (playAtStart)
            {
                mainSource.clip = changeBGM;
                mainSource.Play();
            }

            for (int i = 0; i < uiObjects.Count; i++)
            {
                uiObjects[i].SetActive(false);
            }

            //igtime = gameManager._PlayerData.time;

            //playerCamera = FindObjectOfType<PlayerCamera>();
            //playerCamera.GetComponent<StateMachine>().enabled = false;
            //playerCamera.enabled = false;

            //playerCamera.transform.SetParent(camera.transform);
            //playerCamera.transform.localPosition = Vector3.zero;
            //playerCamera.transform.localRotation = new Quaternion();

            started = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                player = other.gameObject;

                StartCutscene();
            }
        }

        private void Update()
        {
            if (!started)
            {
                return;
            }

            for (int i = 0; i < sources.Count; i++)
            {
                if (timer > soundStartTimes[i] && !isPlaying[i])
                {
                    sources[i].Play();
                    isPlaying[i] = true;
                }
            }

            if (audioSimmerMode == 2 && mainSource.volume > 0)
            {
                mainSource.volume -= Time.deltaTime;
                if (mainSource.volume < 0) {
                    mainSource.volume = 0;
                    mainSource.Stop();
                }
            }

            if (!animReset && timer > 0.1f)
            {
                for (int i = 0; i < animators.Length; i++)
                {
                    animators[i].SetBool("start", false);
                }
                animReset = true;
            }

            timer += Time.deltaTime;

            if (timer > maxTime)
            {
                player.SetActive(true);
                if(audioSimmerMode == 0)
                    mainSource.Play();

                mainSource.volume = bgmVal;

                for (int i = 0; i < uiObjects.Count; i++)
                {
                    uiObjects[i].SetActive(true);
                }

                if(tpPoint != null)
                {
                    player.transform.position = tpPoint.position;
                    player.transform.rotation = tpPoint.rotation;
                }

                if (changeBGM != null)
                    mainSource.clip = changeBGM;

                switch (audioSimmerMode)
                {
                    case 3:
                        break;
                    default:
                        mainSource.Play();
                        break;
                }

                for (int i = 0; i < toDisable.Length; i++)
                {
                    toDisable[i].SetActive(false);
                }
                for (int i = 0; i < toEnable.Length; i++)
                {
                    toEnable[i].SetActive(true);
                }

                //playerCamera.GetComponent<StateMachine>().enabled = true;
                //playerCamera.enabled = true;

                //playerCamera.transform.SetParent(null);

                TakerFix();

                switch (changeCharacter)
                {
                    case newCharacter.keep:
                        break;
                    case newCharacter.sonic_new:
                        ChangeCharacter("sonic_ new", 0);
                        break;
                    case newCharacter.sonic_fast:
                        ChangeCharacter("sonic_fast", 0);
                        break;
                    case newCharacter.snow_board:
                        ChangeCharacter("snow_board", 0);
                        break;
                    case newCharacter.princess:
                        ChangeCharacter("princess", 4);
                        break;
                    case newCharacter.shadow:
                        ChangeCharacter("shadow", 5);
                        break;
                    case newCharacter.silver:
                        ChangeCharacter("silver", 1);
                        break;
                    case newCharacter.tails:
                        ChangeCharacter("tails", 2);
                        break;
                    case newCharacter.knuckles:
                        ChangeCharacter("knuckles", 3);
                        break;
                    case newCharacter.rouge:
                        ChangeCharacter("rouge", 7);
                        break;
                    case newCharacter.omega:
                        ChangeCharacter("omega", 6);
                        break;
                }

                if (nextCutscene != null)
                {
                    nextCutscene.player = player;

                    nextCutscene.StartCutscene();
                } //else 
                    //gameManager._PlayerData.time = igtime;

                Destroy(gameObject);
            }
        }

        private void TakerFix()
        {
            cTaker[] takers = FindObjectsOfType<cTaker>();
            for (int i = 0; i < takers.Length; i++)
            {
                takers[i].transform.localPosition = Vector3.zero;
            }
        }

        private void ChangeCharacter(string name, int id)
        {
            PlayerBase pb = FindObjectOfType<PlayerBase>();

            PlayerBase newchar = (Instantiate(Resources.Load("DefaultPrefabs/Player/" + name), pb.transform.position, pb.transform.rotation) as GameObject).GetComponent<PlayerBase>();
            newchar.SetPlayer(id, name);
            newchar.StartPlayer(false);
            Destroy(pb.gameObject);
        }

        private List<GameObject> GetObjectsOnLayer(int layer, string tag)
        {
            GameObject[] objects = FindObjectsOfType<GameObject>();
            List<GameObject> list = new List<GameObject>();
            for (int i = 0; i < objects.Length; i++)
            {
                if(objects[i].layer == layer && objects[i].tag != tag)
                {
                    list.Add(objects[i]);
                }
            }

            return list;
        }
    }
}
