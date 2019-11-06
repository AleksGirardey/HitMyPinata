using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum EPhase {
    Start,
    ColorAssignment,
    Assimilation,
    Hitting,
    Break
}

public enum EHittingColor { Red, Blue, Green }

public class GameController : MonoBehaviour {
    private static readonly Dictionary<EPhase, Phase> Phases = new Dictionary<EPhase, Phase> {
        {EPhase.Start, new Phase(EPhase.Start, 100000000f)},
        {EPhase.ColorAssignment, new Phase(EPhase.ColorAssignment, 0f)},
        {EPhase.Assimilation, new Phase(EPhase.Assimilation, 1f)},
        {EPhase.ColorAssignment, new Phase(EPhase.Hitting, 12f)},
        {EPhase.ColorAssignment, new Phase(EPhase.Break, 3f)}
    };
    
    private static readonly Dictionary<EHittingColor, HittingColor> HittingColors = new Dictionary<EHittingColor, HittingColor> {
        {EHittingColor.Red, new HittingColor(KeyCode.R)},
        {EHittingColor.Green, new HittingColor(KeyCode.G)},
        {EHittingColor.Blue, new HittingColor(KeyCode.B)}
    };

    private Phase _phase;
    private float _elapsedTime;
    
    public Player playerOne;
    public Player playerTwo;

    private void Start() {
        _phase = Phases[EPhase.Start];
        _elapsedTime = 0f;
    }

    private void Update() {
        if (_phase.phase == EPhase.Start)
            StartUpdate();
        else
            PhaseUpdate();
    }

    private void StartUpdate() {
        
    }
    
    private void PhaseUpdate() {
        switch (_phase.phase) {
            case EPhase.ColorAssignment: {
                // Color Assignment
                List<EHittingColor> colors = new List<EHittingColor>();
                foreach (EHittingColor color in Enum.GetValues(typeof(EHittingColor))) {
                    colors.Add(color);
                }

                float random = Random.Range(0f, colors.Count);
                playerOne.AssignedKey = HittingColors[colors[(int) random]].KeyCode;
                colors.RemoveAt((int) random);
            
                random = Random.Range(0f, colors.Count);
                playerOne.AssignedKey = HittingColors[colors[(int) random]].KeyCode;
                break;
            }
            case EPhase.Assimilation:
                break;
            case EPhase.Hitting: {
                // Incrementing player points on key trigger
                if (Input.GetKeyUp(playerOne.AssignedKey)) ++playerOne.Points;
                if (Input.GetKeyUp(playerTwo.AssignedKey)) ++playerTwo.Points;
                break;
            }
            case EPhase.Break:
                break;
            default:
                break;
        }

        // Phase Transition
        if (_elapsedTime > _phase.timeBeforeNext) {
            _elapsedTime -= _phase.timeBeforeNext;
            _phase = Phases[_phase.Next()];
        }
    }
}
