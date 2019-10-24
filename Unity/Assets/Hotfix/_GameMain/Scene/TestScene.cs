using System.Collections.Generic;

namespace Hotfix
{
    internal class TestScene : Component
    {
        public int[] array = new int[1] { 1 };
        public List<int> list = new List<int>() { 1, 1, 1, 1, 1, 2 };
        public UnityEngine.AnimationCurve animationCurve;
        public UnityEngine.GameObject GameObject1 = GameRoot;
    }
}