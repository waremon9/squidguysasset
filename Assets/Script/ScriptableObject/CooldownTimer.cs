using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

[CreateAssetMenu(menuName = "Timer")]
public class CooldownTimer : ScriptableObject
{
    [SerializeField][Tooltip("Dur�e en ms")] private float _cooldownDurationVote ;
    [SerializeField][Tooltip("Dur�e en ms")] private float _cooldownDurationAction ;
    [SerializeField][Tooltip("Dur�e en ms")] private float _cooldownDurationWin ;

    private float usedCoolDown;

    [SerializeField] private TimerEvent timerEvent;

    public string phase;

    public void SetCD(GState state)
    {
        switch (state)
        {
            case GState.Join:
                break;
            case GState.Vote:
                usedCoolDown = _cooldownDurationVote;
                phase = "Vote :";
                StartCooldown();
                break;

            case GState.Action:
                usedCoolDown = _cooldownDurationAction;
                phase = "Action";
                break;

            case GState.Win:
                usedCoolDown = _cooldownDurationWin;
                phase = "Game restart in :";
                StartCooldown();
                break;
            default:
                break;

        }
        

    }
    public float GetCD()

    {
        //if gamestate
        return usedCoolDown;
    }

    

    public float GetTimeLeft()
    {
        //if gamestate
        
        return (usedCoolDown - _timer.ElapsedMilliseconds);
    }
    public void StartCooldown()
    {
        _timer.Restart();
        
    }

    public void StopCooldown()
    {
        
        _timer.Stop();
        timerEvent.Raise(GameManager.Instance.gameState.gState);

    }

    public bool IsCooldownDone()
    {
        //if gamestate
        return _timer.ElapsedMilliseconds > usedCoolDown;
        
    }

    private Stopwatch _timer = new Stopwatch();
}
