using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ShowLog
{
    internal class LogGUI : MonoBehaviour
    {
        public static LogGUI Instance { get; private set; }

        // ReSharper disable once UnusedMember.Local
        private void Awake()
        {
            DontDestroyOnLoad(this);
            Instance = this;
            ShowGUI_Awake();
            Application.logMessageReceived += GotLog;
        }

        // ReSharper disable once UnusedMember.Local
        private void OnDestroy()
        {
            Application.logMessageReceived -= GotLog;
            Instance = null;
        }

        // ReSharper disable once UnusedMember.Local
        private void Update()
        {
            ShowGUI_Update();
        }

        // ReSharper disable once UnusedMember.Local
        private void OnGUI()
        {
            if (Camera.allCamerasCount < 1)
            {
                return;
            }

            ShowUI();
        }

        #region GUI显示

        private bool show;

        private void ShowUI()
        {
            if (this.show)
            {
                OpenStateShow();
            }
            else
            {
                CloseStateShow();
            }
        }

        private const float screenWidth = 900;
        private const float updateInterval = 0.25f;
        private const int requiredFrames = 10;
        private bool firstTime;
        private float lastUpdate;
        private int frames;

        private float p = 1f;
        private int FontSize => (int) (GUI.skin.font.fontSize * this.p);

        private void ShowGUI_Awake()
        {
            this.firstTime = true;
            this.frames = 0;
            this.p = Screen.width / screenWidth;
            OpenStateShowGUISettings();
            CloseStateShowGUISettings();
        }

        private void ShowGUI_Update()
        {
            this.gcTotalMemory = GC.GetTotalMemory(false);

            if (this.firstTime)
            {
                this.firstTime = false;
                this.lastUpdate = Time.realtimeSinceStartup;
                this.frames = 0;
                return;
            }

            this.frames++;
            var dt = Time.realtimeSinceStartup - this.lastUpdate;
            if (dt > updateInterval && this.frames > requiredFrames)
            {
                this.fps = this.frames / dt;
                this.lastUpdate = Time.realtimeSinceStartup;
                this.frames = 0;
            }

            this.dianliang = SystemInfo.batteryLevel;
        }

        #region 开启状态

        private Rect openWindowRect;
        private GUIStyle openWindowStyle;
        private GUIStyle openLabelStyle;
        private GUIStyle openButtonStyle;
        private Vector2 middleScrollViewPos;
        private Vector2 bottomScrollViewPos;
        private bool showLogType_Init = true;
        private int selectIndex;

        private void OpenStateShow()
        {
            if (this.showLogType_Init)
            {
                this.showLogType_Init = false;
                OpenStateShowInit_Assert();
                OpenStateShowInit_Error();
                OpenStateShowInit_Exception();
                OpenStateShowInit_Log();
                OpenStateShowInit_Warning();
            }

            if (this.openWindowStyle == null)
            {
                this.openWindowStyle = new GUIStyle(GUI.skin.window)
                {
                    fontSize = this.FontSize,
                    alignment = TextAnchor.UpperCenter
                };
            }

            if (this.openLabelStyle == null)
            {
                this.openLabelStyle = new GUIStyle(GUI.skin.label)
                {
                    fontSize = this.FontSize,
                    alignment = TextAnchor.MiddleCenter
                };
            }

            if (this.openButtonStyle == null)
            {
                this.openButtonStyle = new GUIStyle(GUI.skin.button)
                {
                    fontSize = this.FontSize,
                    alignment = TextAnchor.MiddleCenter
                };
            }

            this.openWindowRect = GUI.Window(1, this.openWindowRect, OpenStateShowWindow, "Detail Logs", this.openWindowStyle);
        }

        private void OpenStateShowInit_Assert()
        {
            var info = this.showLogType[LogType.Assert];

            var top = new GUIStyle(GUI.skin.toggle);
            info.GUIStyle_Top = top;
            top.fontSize = this.FontSize;
            top.alignment = TextAnchor.MiddleCenter;
            top.fixedWidth = 54 * this.p;

            var condition = new GUIStyle(GUI.skin.toggle);
            info.GUIStyle_Middle_Condition = condition;
            condition.fontSize = this.FontSize;
            condition.alignment = TextAnchor.MiddleLeft;

            var frame = new GUIStyle(GUI.skin.label);
            info.GUIStyle_Middle_Frame = frame;
            frame.fontSize = this.FontSize;
            frame.alignment = TextAnchor.MiddleCenter;
            frame.fixedWidth = 20 * this.p;

            var time = new GUIStyle(GUI.skin.label);
            info.GUIStyle_Middle_Time = time;
            time.fontSize = this.FontSize;
            time.alignment = TextAnchor.MiddleCenter;
            time.fixedWidth = 80 * this.p;
        }

        private void OpenStateShowInit_Error()
        {
            var info = this.showLogType[LogType.Error];

            var top = new GUIStyle(GUI.skin.toggle);
            info.GUIStyle_Top = top;
            top.fontSize = this.FontSize;
            top.alignment = TextAnchor.MiddleCenter;
            top.fixedWidth = 48 * this.p;

            var condition = new GUIStyle(GUI.skin.toggle);
            info.GUIStyle_Middle_Condition = condition;
            condition.fontSize = this.FontSize;
            condition.alignment = TextAnchor.MiddleLeft;

            var frame = new GUIStyle(GUI.skin.label);
            info.GUIStyle_Middle_Frame = frame;
            frame.fontSize = this.FontSize;
            frame.alignment = TextAnchor.MiddleCenter;
            frame.fixedWidth = 20 * this.p;

            var time = new GUIStyle(GUI.skin.label);
            info.GUIStyle_Middle_Time = time;
            time.fontSize = this.FontSize;
            time.alignment = TextAnchor.MiddleCenter;
            time.fixedWidth = 80 * this.p;
        }

        private void OpenStateShowInit_Exception()
        {
            var info = this.showLogType[LogType.Exception];

            var top = new GUIStyle(GUI.skin.toggle);
            info.GUIStyle_Top = top;
            top.fontSize = this.FontSize;
            top.alignment = TextAnchor.MiddleCenter;
            top.fixedWidth = 72 * this.p;

            var condition = new GUIStyle(GUI.skin.toggle);
            info.GUIStyle_Middle_Condition = condition;
            condition.fontSize = this.FontSize;
            condition.alignment = TextAnchor.MiddleLeft;

            var frame = new GUIStyle(GUI.skin.label);
            info.GUIStyle_Middle_Frame = frame;
            frame.fontSize = this.FontSize;
            frame.alignment = TextAnchor.MiddleCenter;
            frame.fixedWidth = 20 * this.p;

            var time = new GUIStyle(GUI.skin.label);
            info.GUIStyle_Middle_Time = time;
            time.fontSize = this.FontSize;
            time.alignment = TextAnchor.MiddleCenter;
            time.fixedWidth = 80 * this.p;
        }

        private void OpenStateShowInit_Log()
        {
            var info = this.showLogType[LogType.Log];

            var top = new GUIStyle(GUI.skin.toggle);
            info.GUIStyle_Top = top;
            top.fontSize = this.FontSize;
            top.alignment = TextAnchor.MiddleCenter;
            top.fixedWidth = 36 * this.p;

            var condition = new GUIStyle(GUI.skin.toggle);
            info.GUIStyle_Middle_Condition = condition;
            condition.fontSize = this.FontSize;
            condition.alignment = TextAnchor.MiddleLeft;

            var frame = new GUIStyle(GUI.skin.label);
            info.GUIStyle_Middle_Frame = frame;
            frame.fontSize = this.FontSize;
            frame.alignment = TextAnchor.MiddleCenter;
            frame.fixedWidth = 20 * this.p;

            var time = new GUIStyle(GUI.skin.label);
            info.GUIStyle_Middle_Time = time;
            time.fontSize = this.FontSize;
            time.alignment = TextAnchor.MiddleCenter;
            time.fixedWidth = 80 * this.p;
        }

        private void OpenStateShowInit_Warning()
        {
            var info = this.showLogType[LogType.Warning];

            var top = new GUIStyle(GUI.skin.toggle);
            info.GUIStyle_Top = top;
            top.fontSize = this.FontSize;
            top.alignment = TextAnchor.MiddleCenter;
            top.fixedWidth = 60 * this.p;

            var condition = new GUIStyle(GUI.skin.toggle);
            info.GUIStyle_Middle_Condition = condition;
            condition.fontSize = this.FontSize;
            condition.alignment = TextAnchor.MiddleLeft;

            var frame = new GUIStyle(GUI.skin.label);
            info.GUIStyle_Middle_Frame = frame;
            frame.fontSize = this.FontSize;
            frame.alignment = TextAnchor.MiddleCenter;
            frame.fixedWidth = 20 * this.p;

            var time = new GUIStyle(GUI.skin.label);
            info.GUIStyle_Middle_Time = time;
            time.fontSize = this.FontSize;
            time.alignment = TextAnchor.MiddleCenter;
            time.fixedWidth = 80 * this.p;
        }

        private void OpenStateShowWindow(int id)
        {
            OpenStateGUI_Top();
            GUILayout.Space(5 * this.p);
            OpenStateGUI_Middle();
            GUILayout.Space(10 * this.p);
            OpenStateGUI_Bottom();
        }

        private void OpenStateGUI_Bottom()
        {
            this.bottomScrollViewPos = GUILayout.BeginScrollView(this.bottomScrollViewPos);

            if (this.selectIndex >= 0 && this.selectIndex < this.logs.Count)
            {
                var a = this.logs[this.selectIndex];
                GUILayout.Label(a.Stacktrace);
            }

            GUILayout.EndScrollView();
        }

        private void OpenStateGUI_Middle()
        {
            this.middleScrollViewPos = GUILayout.BeginScrollView(this.middleScrollViewPos);

            for (var i = 0; i < this.logs.Count; i++)
            {
                var a = this.logs[i];
                var b = this.showLogType[a.Type];

                GUILayout.BeginHorizontal();
                if (GUILayout.Toggle(this.selectIndex == i, a.Condition, b.GUIStyle_Middle_Condition))
                {
                    this.selectIndex = i;
                }

                GUILayout.Label($"{a.Frame}", b.GUIStyle_Middle_Frame);
                GUILayout.Label($"{a.Time}", b.GUIStyle_Middle_Time);
                GUILayout.EndHorizontal();
            }

            GUILayout.EndScrollView();
        }

        private void OpenStateGUI_Top()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("");

            GUILayout.Label("*", this.openLabelStyle, GUILayout.Width(8 * this.p));
            var a = this.showLogType[LogType.Assert];
            a.Show = GUILayout.Toggle(a.Show, a.Name, a.GUIStyle_Top);
            GUILayout.Label("*", this.openLabelStyle, GUILayout.Width(8 * this.p));
            a = this.showLogType[LogType.Error];
            a.Show = GUILayout.Toggle(a.Show, a.Name, a.GUIStyle_Top);
            GUILayout.Label("*", this.openLabelStyle, GUILayout.Width(8 * this.p));
            a = this.showLogType[LogType.Exception];
            a.Show = GUILayout.Toggle(a.Show, a.Name, a.GUIStyle_Top);
            GUILayout.Label("*", this.openLabelStyle, GUILayout.Width(8 * this.p));
            a = this.showLogType[LogType.Log];
            a.Show = GUILayout.Toggle(a.Show, a.Name, a.GUIStyle_Top);
            GUILayout.Label("*", this.openLabelStyle, GUILayout.Width(8 * this.p));
            a = this.showLogType[LogType.Warning];
            a.Show = GUILayout.Toggle(a.Show, a.Name, a.GUIStyle_Top);
            GUILayout.Label("*", this.openLabelStyle, GUILayout.Width(8 * this.p));

            if (GUILayout.Button("X", this.openButtonStyle, GUILayout.Width(30 * this.p)))
            {
                this.show = false;
            }

            GUILayout.Label("");
            GUILayout.EndHorizontal();
        }

        private void OpenStateShowGUISettings()
        {
            this.selectIndex = -1;
            this.openWindowRect = new Rect(5, 5, Screen.width - 10, Screen.height - 10);
            this.middleScrollViewPos = Vector2.zero;
            this.bottomScrollViewPos = Vector2.zero;

            for (var i = 0; i < 100; i++)
            {
                this.logs.Add(new Log
                {
                    Time = i,
                    Frame = i,
                    Condition = $"ConditionConditionConditionConditionConditionConditionConditionConditionConditionConditionConditionConditionConditionConditionConditionConditionConditionCondition{i}",
                    Stacktrace = $"Stacktrace{i}\r\nStacktrace\r\nStacktrace\r\nStacktrace\r\nStacktrace\r\nStacktrace\r\nStacktrace\r\nStacktrace\r\nStacktrace",
                    Type = (LogType) Random.Range(0, 5)
                });
            }
        }

        #endregion

        #region 关闭状态

        private float fps;
        private long gcTotalMemory;
        private float dianliang;

        private Rect closeWindowRect;
        private Rect closeDragWindowRect;
        private Rect fpsRect;
        private Rect gcTotalMemoryRect;
        private Rect dianliangRect;
        private Rect openBtnRect;

        private GUIStyle closeLabelStyle;
        private GUIStyle closeWindowStyle;
        private GUIStyle closeButtonStyle;

        private void CloseStateShow()
        {
            if (this.closeLabelStyle == null)
            {
                this.closeLabelStyle = new GUIStyle(GUI.skin.label)
                {
                    fontSize = this.FontSize,
                    alignment = TextAnchor.MiddleCenter
                };
            }

            if (this.closeButtonStyle == null)
            {
                this.closeButtonStyle = new GUIStyle(GUI.skin.button)
                {
                    fontSize = this.FontSize,
                    alignment = TextAnchor.MiddleCenter
                };
            }

            if (this.closeWindowStyle == null)
            {
                this.closeWindowStyle = new GUIStyle(GUI.skin.window)
                {
                    fontSize = this.FontSize,
                    alignment = TextAnchor.UpperCenter
                };
            }

            this.closeWindowRect = GUI.Window(0, this.closeWindowRect, CloseStateShowWindow, "ShowLog", this.closeWindowStyle);
        }

        private void CloseStateShowWindow(int windowID)
        {
            GUI.DragWindow(this.closeDragWindowRect);

            GUI.Label(this.fpsRect, $"帧率:{this.fps:0.000}", this.closeLabelStyle);
            GUI.Label(this.gcTotalMemoryRect, GetMemoryStr(), this.closeLabelStyle);
            GUI.Label(this.dianliangRect, GetDianLiangStr(), this.closeLabelStyle);
            if (GUI.Button(this.openBtnRect, "Open", this.closeButtonStyle))
            {
                this.show = true;
            }
        }

        private void CloseStateShowGUISettings()
        {
            this.y = 0;
            this.fpsRect = new Rect(this.Get_X, this.Get_Y, this.Get_W, this.Get_H);
            this.gcTotalMemoryRect = new Rect(this.Get_X, this.Get_Y, this.Get_W, this.Get_H);
            this.dianliangRect = new Rect(this.Get_X, this.Get_Y, this.Get_W, this.Get_H);
            this.openBtnRect = new Rect(this.Get_X, this.Get_Y, this.Get_W, this.Get_H);

            //最后设置
            this.closeWindowRect = new Rect(0, 0, this.Get_W_Total, this.Get_H_Total);
            this.closeDragWindowRect = new Rect(0, 0, this.Get_W_Total, this.Get_H_Drag);
        }

        private int y;
        private float Get_W => 100 * this.p;
        private float Get_W_Total => 110 * this.p;
        private float Get_H => 20 * this.p;
        private float Get_H_Total => (25 + 20 * this.y) * this.p;
        private float Get_H_Drag => (25 + 20 * (this.y - 1)) * this.p;
        private float Get_X => 5 * this.p;

        private float Get_Y
        {
            get
            {
                this.y++;
                return this.y * 20 * this.p;
            }
        }

        private string GetDianLiangStr()
        {
            if (this.dianliang < 0)
            {
                return "电量:100%";
            }

            var d = Mathf.FloorToInt(this.dianliang * 100);
            return $"电量:{d}%";
        }

        private string GetMemoryStr()
        {
            string str;
            if (this.gcTotalMemory < 1024)
            {
                str = $"{this.gcTotalMemory:0.000}b";
            }
            else if (this.gcTotalMemory < 1024 * 1024)
            {
                str = $"{this.gcTotalMemory / 1024f:0.000}kb";
            }
            else
            {
                str = $"{this.gcTotalMemory / 1024f / 1024f:0.000}mb";
            }

            return $"GC:{str}";
        }

        #endregion

        #endregion

        #region 数据

        private readonly List<Log> logs = new List<Log>();

        private void GotLog(string condition, string stacktrace, LogType type)
        {
            var a = Time.realtimeSinceStartup;
            var b = Time.frameCount;
            this.logs.Add(new Log
            {
                Time = a,
                Frame = b,
                Condition = condition,
                Stacktrace = stacktrace,
                Type = type
            });
        }

        private readonly Dictionary<LogType, LogTypeInfo> showLogType = new Dictionary<LogType, LogTypeInfo>
        {
            {LogType.Assert, new LogTypeInfo("Assert")},
            {LogType.Error, new LogTypeInfo("Error")},
            {LogType.Exception, new LogTypeInfo("Exception")},
            {LogType.Log, new LogTypeInfo("Log")},
            {LogType.Warning, new LogTypeInfo("Warning")}
        };

        private struct Log
        {
            public int Frame;
            public float Time;
            public string Condition;
            public string Stacktrace;
            public LogType Type;
        }

        private class LogTypeInfo
        {
            public string Name { get; }
            public bool Show { get; set; }

            public GUIStyle GUIStyle_Top { get; set; }
            public GUIStyle GUIStyle_Middle_Condition { get; set; }
            public GUIStyle GUIStyle_Middle_Time { get; set; }
            public GUIStyle GUIStyle_Middle_Frame { get; set; }

            public LogTypeInfo(string name)
            {
                this.Name = name;
                this.Show = true;
            }
        }

        #endregion
    }
}