using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

namespace TriviaGame
{
    public class Player
    {
        public Action<int> OnScoreChanged;
        public Action<string> OnNameChanged;
        public Action<Color> OnColorChanged;
        public Action<bool> OnActivePlayerChanged;

        private string name;
        private Color _color;
        private int _score;

        public string Name {
            get
            {
                return name;
            }
            set
            {
                name = value;
                OnNameChanged?.Invoke(name);
            }
        }

        public Color Color
        {
            get
            {
                return _color;
            }
            set
            {
                _color = value;
                OnColorChanged?.Invoke(_color);
            }
        }

        public int Score
        {
            get
            {
                return _score;
            }
            set
            {
                _score = value;
                OnScoreChanged?.Invoke(_score);
            }
        }

        public bool IsActivePlayer
        {
            get
            {
                return isActivePlayer;
            }
            set
            {
                isActivePlayer = value;
                OnActivePlayerChanged?.Invoke(isActivePlayer);
            }
        }

        private bool isActivePlayer;

        public Player(string name)
        {
            this.Name = name;
        }
    }
}