﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberField : MonoBehaviour
{
    private Board board;

    private int x1, y1, value;

    public int X { get { return x1; } }
    public int Y { get { return y1; } }

    string identifier;

    public Text number;

    public void SetValues(int _x1, int _y1, int _value, string _identifier, Board _board)
    {
        x1 = _x1;
        y1 = _y1;
        value = _value;
        identifier = _identifier;
        board = _board;

        number.text = (value != 0) ? value.ToString() : "";

        if(value != 0)
        {
            GetComponentInParent<Button>().interactable = false;
        }
        else
        {
            number.color = Color.blue;
        }
    }

    public void ButtonClick()
    {
        InputField.instance.ActivateInputField(this);
    }

    public void ReceiveInput(int newValue)
    {
        value = newValue;
        number.text = (value != 0) ? value.ToString() : "";

        board.SetInputInRiddleGrid(x1, y1, value);
    }

    public void SetHint(int _value)
    {
        value = _value;
        number.text = value.ToString();
        number.color = Color.red;
        GetComponentInParent<Button>().interactable = false;
    }
}
