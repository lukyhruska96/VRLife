using _3dSoundSynthesis;
using NAudio.Wave;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoIPLib;

public class MyMovementScript : MonoBehaviour
{
    [SerializeField] public float horizontalSens;
    [SerializeField] public float verticalSens;
    [SerializeField] public float walkingSpeed;
    [SerializeField] public float runningSpeed;
    [SerializeField] public float jumpSpeed;
    [SerializeField] public float soundDistance;


    // looking
    private Camera fpsCamera;
    private float xRotate = 0.0f;

    // moving
    private Animator animator;
    private float height;
    private Rigidbody rigidbody;

    // ICETest
    private GameObject speaker;
    private WaveOut speakerOutput;
    private SourceLocation sourceLocation;
    private Mp3FileReader mp3Reader = new Mp3FileReader(@"D:\OneDrive\Dokumenty\Škola\Ročníkový projekt\testing\ICETest\ICETest\test.mp3");


    // Start is called before the first frame update
    void Start()
    {

        // looking
        this.fpsCamera = FindObjectOfType<Camera>();
        this.fpsCamera.transform.eulerAngles = this.transform.eulerAngles;

        // moving
        this.animator = GetComponent<Animator>();
        this.height = GetComponent<Collider>().bounds.extents.y;
        this.rigidbody = GetComponent<Rigidbody>();

        // ICETest
        this.speaker = GameObject.Find("Speaker2");
        this.speakerOutput = new WaveOut()
        {
            DesiredLatency = 100
        };
        sourceLocation = new SourceLocation(0, 0, 1);
        VoiceStream voiceStream = new VoiceStream(mp3Reader.ToSampleProvider(), new ILowLevelVoiceEffect[] { new ICEFilter(sourceLocation) }, new IHighLevelVoiceEffect[0]);
        ISampleProvider sampleProvider = voiceStream.GetSampleProvider();
        voiceStream.Run();
        speakerOutput.PlaybackStopped += (_, __) =>
        {
            mp3Reader.Dispose();
        };
        speakerOutput.Init(sampleProvider);
        speakerOutput.Play();
    }

    // Update is called once per frame
    void Update()
    {
        Looking();
        Moving();
        SoundSourceLocation();
    }

    private void SoundSourceLocation()
    {
        Vector3 a = speaker.transform.position - transform.position;
        a.y = 0;
        Vector3 b = transform.forward;
        b.y = 0;
        sourceLocation.Azim = Vector3.SignedAngle(a, b, Vector3.up);
        sourceLocation.Atten = (soundDistance - Vector3.Distance(transform.position, speaker.transform.position)) / soundDistance;
    }

    private void Looking()
    {
        transform.Rotate(0, Input.GetAxis("Mouse X") * Time.deltaTime * horizontalSens, 0);
        xRotate -= Input.GetAxis("Mouse Y") * Time.deltaTime * verticalSens;
        xRotate = Mathf.Clamp(xRotate, -90f, 56f);
        this.fpsCamera.transform.eulerAngles = new Vector3(xRotate, transform.eulerAngles.y, 0);
    }

    private void Moving()
    {
        if(Input.GetKey(KeyCode.Space) && IsGrounded())
        {
            animator.SetBool("jump", true);
            rigidbody.AddForce(Vector3.up * jumpSpeed);
        }
        else
        {
            animator.SetBool("jump", false);
        }
        if(Input.GetKey(KeyCode.W))
        {
            animator.SetBool("backwards", false);
        }
        else if(Input.GetKey(KeyCode.S))
        {
            animator.SetBool("backwards", true);
        }
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                animator.SetBool("isRunning", true);
                animator.SetBool("isWalking", false);
                transform.Translate(0, 0, Input.GetAxis("Vertical") * Time.deltaTime * runningSpeed);
            }
            else
            {
                animator.SetBool("isRunning", false);
                animator.SetBool("isWalking", true);
                transform.Translate(0, 0, Input.GetAxis("Vertical") * Time.deltaTime * walkingSpeed);
            }
        }
        else
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("isWalking", false);
        }
    }

    public void OnApplicationQuit()
    {
        speakerOutput.Stop();
        speakerOutput.Dispose();
    }

    public void OnApplicationPause(bool pause)
    {
        if (pause)
            speakerOutput?.Pause();
        else
        {
            if (speakerOutput?.PlaybackState == PlaybackState.Paused)
                speakerOutput?.Resume();
            else
                speakerOutput?.Play();
        }
    }

    public bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, height + 0.02f);
    }
}
