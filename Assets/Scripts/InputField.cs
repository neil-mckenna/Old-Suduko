using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputField : MonoBehaviour
{
    public static InputField instance;
    NumberField lastField;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        this.gameObject.SetActive(false);

    }

    public void ActivateInputField(NumberField _lastField)
    {
        gameObject.SetActive(true);
        lastField = _lastField;

    }

    public void ClickedInput(int number)
    {
        lastField.RecieveInput(number);

        // Deactivate panel
        gameObject.SetActive(false);

    }





}
