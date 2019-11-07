using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public enum EPhase {
    Start,
    End,
    ColorAssignment,
    Assimilation,
    Hitting,
    Break
}

public enum EHittingColor { Red, Blue, Green }

public class GameController : MonoBehaviour {
    public static GameController Instance;

    [SerializeField] private int roundMax = 10;
    private int _roundCount = 0;
    
    private static readonly Dictionary<EPhase, Phase> Phases = new Dictionary<EPhase, Phase> {
        {EPhase.Start, new Phase(EPhase.Start, 100000000f)},
        {EPhase.End, new Phase(EPhase.End, 100000000f)},
        {EPhase.ColorAssignment, new Phase(EPhase.ColorAssignment, 0f)},
        {EPhase.Assimilation, new Phase(EPhase.Assimilation, 1f)},
        {EPhase.Hitting, new Phase(EPhase.Hitting, 12f)},
        {EPhase.Break, new Phase(EPhase.Break, 3f)}
    };
    
    private static readonly Dictionary<EHittingColor, HittingColor> HittingColors = new Dictionary<EHittingColor, HittingColor> {
        {EHittingColor.Red, new HittingColor(KeyCode.R, Color.red)},
        {EHittingColor.Green, new HittingColor(KeyCode.G, Color.green)},
        {EHittingColor.Blue, new HittingColor(KeyCode.B, Color.blue)}
    };

    public Phase phase;
    public float elapsedTime;
    
    public Player playerOne;
    public Player playerTwo;

    private bool _init = true;
    
    private void Start() {
        Instance = this;
        playerOne = gameObject.AddComponent<Player>();
        playerOne.transform.SetParent(transform);
        playerTwo = gameObject.AddComponent<Player>();
        playerTwo.transform.SetParent(transform);
        phase = Phases[EPhase.Start];
        elapsedTime = 0f;
        DontDestroyOnLoad(gameObject);
    }

    private void Update() {
        elapsedTime += Time.deltaTime;
        if (phase.phase == EPhase.Start) StartUpdate();
        else PhaseUpdate();
        
        Debug.Log("Current Phase : " + phase.phase);
    }

    private void StartUpdate() {
        if (!HittingColors.Values.Any(color => Input.GetKeyUp(color.KeyCode))) return;
        phase = Phases[phase.Next()];
        elapsedTime = 0f;
        SceneManager.LoadScene("Scenes/Round");
    }
    
    private void PhaseUpdate() {
        switch (phase.phase) {
            case EPhase.ColorAssignment: {
                Random.InitState((int)Time.time);
                // Color Assignment
                List<EHittingColor> colors = Enum.GetValues(typeof(EHittingColor)).Cast<EHittingColor>().ToList();

                int random = Random.Range(0, colors.Count);
                
                playerOne.AssignedKey = HittingColors[colors[random]];
                colors.RemoveAt(random);
            
                Random.InitState((int)Time.time);
                
                random = Random.Range(0, colors.Count);

                playerTwo.AssignedKey = HittingColors[colors[random]];
                elapsedTime = 0f;
                
                phase = Phases[phase.Next()];
                break;
            }
            case EPhase.Assimilation:
                break;
            case EPhase.Hitting: {
                if (_init) {
                    elapsedTime = 0f;
                    _init = false;
                }

                // Incrementing player points on key trigger
                if (Input.GetKeyUp(playerOne.AssignedKey.KeyCode)) ++playerOne.Points;
                if (Input.GetKeyUp(playerTwo.AssignedKey.KeyCode)) ++playerTwo.Points;
                break;
            }
            case EPhase.Break:
                if (!_init) {
                    _init = true;
                    _roundCount++;
                }
                break;
            default:
                break;
        }

        // Phase Transition
        if (elapsedTime > phase.timeBeforeNext) {
            elapsedTime -= phase.timeBeforeNext;
            phase = Phases[phase.Next()];
            if (phase.phase == EPhase.ColorAssignment && _roundCount >= roundMax)
                SceneManager.LoadScene("Scenes/End");
        }
    }
}
