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
        private int _score = 0;

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
                Debug.Log("Score = " + _score);
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

        public bool IsMe { get; private set; } = false;

        public string Id { get; set; }

        private bool isActivePlayer;

        public Player(string id, string name, bool isMe)
        {
            this.Id = id;
            this.Name = name;
            this.IsMe = isMe;
        }
    }
}