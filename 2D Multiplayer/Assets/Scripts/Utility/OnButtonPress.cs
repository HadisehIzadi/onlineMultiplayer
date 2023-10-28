using UnityEngine;
using System;

public enum ButtonActions : byte
{
    lobby_ready,
    lobby_not_ready
}


public class OnButtonPress : MonoBehaviour
{
    public static Action<ButtonActions> a_OnButtonPress;

    [SerializeField]
    private ButtonActions _buttonAction;

    public void OnPress()
    {
        a_OnButtonPress?.Invoke(_buttonAction);
    }
}
