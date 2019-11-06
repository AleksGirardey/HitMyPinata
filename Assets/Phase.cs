public struct Phase {
    public EPhase phase;
    public float timeBeforeNext;

    public Phase(EPhase phase, float timeBeforeNext) {
        this.phase = phase;
        this.timeBeforeNext = timeBeforeNext;
    }
    
    public EPhase Next() {
        if (phase == EPhase.Break)
            return EPhase.ColorAssignment;
        return phase++;
    }
}