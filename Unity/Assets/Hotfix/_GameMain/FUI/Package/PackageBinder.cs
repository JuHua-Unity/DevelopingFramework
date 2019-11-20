/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;

namespace Hotfix.Package
{
    public class PackageBinder
    {
        public static void BindAll()
        {
            UIObjectFactory.SetPackageItemExtension(UI_UI.URL, () => { return (GComponent)System.Activator.CreateInstance(typeof(UI_UI)); });
        }
    }
}