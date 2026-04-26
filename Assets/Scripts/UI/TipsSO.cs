using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    [CreateAssetMenu(fileName = "New Tips", menuName = "SO/Tips")]
    public class TipsSO : ScriptableObject
    {
        public List<string> TipsList;
    }
}
