using FairyGUI;
using UnityEngine;

namespace Hotfix
{
    internal static class FairyGUIHelper
    {
        public static void Init()
        {
            //初始化FairyGUI
            //先初始化UIConfig
            FairyGUIUIConfigInit();
            //再初始化UIContentScaler
            FairyGUIUIContentScalerInit();
        }

        private static void FairyGUIUIContentScalerInit()
        {
            UIContentScaler scaler = Stage.inst.gameObject.GetComponent<UIContentScaler>();
            //选择根据固定屏幕大小适配(即 定义好设计尺寸)
            scaler.scaleMode = UIContentScaler.ScaleMode.ScaleWithScreenSize;
            //设计分辨率
            scaler.designResolutionX = 1920;
            scaler.designResolutionY = 1080;
            //选择根据屏幕宽高适配 宽的时候根据高适配(两边留空) 高的时候根据根据宽适配(上下留空)
            scaler.screenMatchMode = UIContentScaler.ScreenMatchMode.MatchWidthOrHeight;
            //不忽略转向
            scaler.ignoreOrientation = false;

            scaler.ApplyChange();
            GRoot.inst.ApplyContentScaleFactor();
        }

        private static void FairyGUIUIConfigInit()
        {
            UIConfig.buttonSound = UIPackage.GetItemAssetByURL(NormalUrl("PackageName", "Button")) as NAudioClip;//按钮默认声音
            UIConfig.buttonSoundVolumeScale = 1f;//按钮默认声音大小

            UIConfig.clickDragSensitivity = 2;//拖拽的触发距离(触摸而非鼠标)
            UIConfig.touchDragSensitivity = 10;//拖拽的触发距离(鼠标而非触摸)
            UIConfig.touchScrollSensitivity = 20;//滚动的触发距离(鼠标而非触摸)

            UIConfig.defaultComboBoxVisibleItemCount = 10;//ComboBox的最大默认显示个数  超过的话visible为false

            UIConfig.defaultFont = "Microsoft YaHei,SimHei";//1系统字体名字 2Resources目录下的ttf格式的字体文件名字 3多种字体用英文逗号隔开

            UIConfig.defaultScrollBarDisplay = ScrollBarDisplayType.Auto;//移动平台推荐Auto web推荐Visible
            UIConfig.defaultScrollBounceEffect = true;//开启滚动条回弹效果
            UIConfig.defaultScrollStep = 25;//滚动步长
            UIConfig.defaultScrollTouchEffect = true;//开启可拖拽滚动区域任意位置
            UIConfig.verticalScrollBar = NormalUrl("PackageName", "VerticalScrollBar");//全局垂直滚动条
            UIConfig.horizontalScrollBar = NormalUrl("PackageName", "HorizontalScrollBar");//全局水平滚动条

            UIConfig.loaderErrorSign = NormalUrl("PackageName", "LoaderErrorSign");//如果Loader装填失败了会创建此物体来代替

            UIConfig.popupMenu = NormalUrl("PackageName", "PopupMenu");//默认下拉菜单
            UIConfig.popupMenu_seperator = NormalUrl("PackageName", "PopupMenuSeperator");//默认下拉菜单的分割线
            UIConfig.tooltipsWin = NormalUrl("PackageName", "TooltipsWin");//提示框 点击屏幕会消失那种

            UIConfig.globalModalWaiting = NormalUrl("PackageName", "GlobalModalWaiting");//全局等待模态窗口
            UIConfig.modalLayerColor = new Color(0f, 0f, 0f, 0.4f);//模态窗口出现后背景变灰
            UIConfig.windowModalWaiting = NormalUrl("PackageName", "WindowModalWaiting");//全局模态窗体等待 阻挡用户操作
            UIConfig.allowSoftnessOnTopOrLeftSide = true;//点击窗口的时候窗口自动置前

            UIConfig.inputCaretSize = 1;//输入光标大小控制
            UIConfig.inputHighlightColor = new Color32(255, 223, 141, 128);//输入框文本选中高亮显示
            UIConfig.richTextRowVerticalAlign = VertAlignType.Bottom;//不清楚
            UIConfig.enhancedTextOutlineEffect = false;//开启文本描边发光效果
            UIConfig.depthSupportForPaintingMode = false;//if RenderTexture using in paiting mode has depth support.
        }

        private static string NormalUrl(string pkgName, string resName)
        {
            return $"{UIPackage.URL_PREFIX}{pkgName}/{resName}";
        }
    }
}