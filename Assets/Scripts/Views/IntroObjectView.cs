using UnityEngine;
public class IntroObjectView : MonoBehaviour
{
    private PlayerStateModel _playerStateModel;

    private void Awake()
    {
        _playerStateModel = FindObjectOfType<PlayerStateModel>();
        _playerStateModel.OnStateChanged += CheckEnabled;
    }

    void CheckEnabled(PlayerState state)
    {
        foreach (Transform child in transform)
            child.gameObject.SetActive(state == PlayerState.Intro);
    }
}
