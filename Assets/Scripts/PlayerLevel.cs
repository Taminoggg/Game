using UnityEngine;

public class PlayerLevel : MonoBehaviour
{
    public float _currLevel = 1;
    private float _xpNeeded = 10;
    private static float _currXp;
    public float display;

    public static void AddXP(float amount)
    {
        _currXp += amount;
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.T)){
            _currXp += 10;
        }

        display = _currXp;
        if (_xpNeeded > _currXp) return;
        PauseMenu.SetState(State.SelectItem);
        _currXp -= _xpNeeded;
        _currLevel++;
        _xpNeeded += 10;
    }
}
