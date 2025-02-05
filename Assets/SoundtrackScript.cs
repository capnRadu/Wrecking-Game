using UnityEngine;

public class SoundtrackScript : MonoBehaviour
{

    [Header("Start Trigger")]
    public static bool GameStart = false;
    public static bool GameEnd = false;

    private void Awake()
    {
        GameStart = false;
        GameEnd = false;
    }


    [Header("Stats")]
    [SerializeField]
    public double musicDuration;
    [SerializeField]
    public double goalTime;
    [SerializeField]
    bool introOver = false;
    [SerializeField]
    bool musicStart = false;
    [SerializeField]
    bool IntroStart = false;
    [SerializeField]
    bool loopReady = false;
    [SerializeField]
    bool EndingStart = false;




    public AudioClip currentClip;
    public AudioClip newClip;
    [Header("MusicSources")]
    public AudioSource pianoSource;
    public AudioSource audioSource;
    public AudioSource audioSourceTwo;
    public AudioSource Reverse;
    [Header("SFX")]
    public AudioSource PianoSlam;






    private void Start()
    {

    }



    void Update()
    {

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    GameStart = true;
        //}


        //if (Input.GetKeyDown(KeyCode.Return))
        //{
        //    GameEnd = true;
        //}


        if (GameStart && !musicStart) {
            PianoDisrupt();
        }

        if (!PianoSlam.isPlaying && musicStart && !IntroStart)
        {
            IntroStart = true;
            OnPlayMusic();
            
        }

        if (AudioSettings.dspTime > goalTime - 1 && introOver == false && loopReady)
        {
            PlayLoop();
        }

        if (GameStart && musicStart && GameEnd && !EndingStart)
        {
            audioSourceTwo.Stop();
            audioSource.Stop();
            Reverse.Play();
            EndingStart = true;

        }

    }

    

    private void PianoDisrupt()
    {

        pianoSource.Stop();
        PianoSlam.Play();
        musicStart = true;


    }


    private void OnPlayMusic()
    {
        goalTime = AudioSettings.dspTime + 0.5;
        audioSource.clip = currentClip;
        audioSource.PlayScheduled(goalTime);
        musicDuration = (double)currentClip.samples / currentClip.frequency;
        goalTime = goalTime + musicDuration;
        loopReady = true;

    }

    private void PlayLoop()
    {

            audioSourceTwo.clip = newClip;
            audioSourceTwo.PlayScheduled(goalTime);
            introOver = true;
        
    }
}
