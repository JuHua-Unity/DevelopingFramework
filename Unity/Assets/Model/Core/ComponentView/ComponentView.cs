#if UNITY_EDITOR
namespace Model
{
    public class ComponentView : UnityEngine.MonoBehaviour
    {
        public object Component { get; set; } = null;
    }
}
#endif