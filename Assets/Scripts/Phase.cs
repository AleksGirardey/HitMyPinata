using System;

public struct Phase {
    public EPhase phase;
    public float timeBeforeNext;

    public Phase(EPhase phase, float timeBeforeNext) {
        this.phase = phase;
        this.timeBeforeNext = timeBeforeNext;
    }
    
    public EPhase Next() {
        if (phase == EPhase.Break || phase == EPhase.Start) return EPhase.ColorAssignment;

        EPhase[] phases = (EPhase[]) Enum.GetValues(typeof(EPhase));

        int j = Array.IndexOf(phases, phase) + 1;
        
        return phases[j];
    }
}