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
        gameObject.SetActive(false);
    }

    public void ActivateInputField(NumberField field)
    {
        gameObject.SetActive(true);
        lastField = field;
    }

    public void ClickedInput(int number)
    {
        lastField.ReceiveInput(number);

        gameObject.SetActive(false);
    }
}
