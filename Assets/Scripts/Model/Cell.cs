using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Model
{
    public class Cell
    {
        public Vector2  Position { get; set; }
        public CellType Type { get; set; }
    }
}
