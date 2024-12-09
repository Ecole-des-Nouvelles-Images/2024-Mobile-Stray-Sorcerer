using UnityEngine;

namespace Maze
{
    public class Props
    {
        public enum Type
        {
            Barrel,
            BrazierColumn,
            Crates,
            Flag,
            Rubble,
        }

        [field: SerializeField] public Type PropType { get; private set; }
    }
}