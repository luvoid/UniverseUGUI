using System;
using System.Collections.Generic;
using UnityEngine.Scripting;
using UnityEngine;

namespace UniverseLib.UGUI
{
    /// <summary>
    ///   A LuVoid.UniverseLib UGUI event.
    /// </summary>
    public sealed class UGUIEvent
    {
        internal IntPtr m_Ptr;
        private static UGUIEvent s_Current;
        private static UGUIEvent s_MasterEvent;

        private static Dictionary<string, int> __switchMap;

        static UGUIEvent()
        {
            Internal_MakeMasterEventCurrent(0);
        }

        public UGUIEvent()
        {
            displayIndex = 0;
        }

        public UGUIEvent(int displayIndex) //=> this.Init(displayIndex);
        {
            this.displayIndex = displayIndex;
        }

        public UGUIEvent(UGUIEvent other)
        {
            if (other == null)
                throw new ArgumentException("Event to copy from is null.");

            m_Ptr = other.m_Ptr;
            alt = other.alt;
            button = other.button;
            capsLock = other.capsLock;
            character = other.character;
            clickCount = other.clickCount;
            command = other.command;
            commandName = other.commandName;
            control = other.control;
            delta = other.delta;
            displayIndex = other.displayIndex;
            keyCode = other.keyCode;
            modifiers = other.modifiers;
            mousePosition = other.mousePosition;
            numeric = other.numeric;
            pressure = other.pressure;
            shift = other.shift;
            uRawType = other.uRawType;
            uType = other.uType;

        }


        internal static void CleanupRoots()
        {
            s_Current = null;
            s_MasterEvent = null;
        }

        internal static void BeginUGUIEvent(UGUIEventType type)
        {
            s_MasterEvent = new UGUIEvent()
            {
                uRawType = type,
                uType = type,
            };
            s_Current = s_MasterEvent;
        }

        internal static void EndUGUIEvent()
        {
            s_Current = null;
        }

        /// <summary>
        /// The mouse position.
        /// </summary>
        public Vector2 mousePosition { get; set; }

        /// <summary>
        /// The relative movement of the mouse compared to last event.
        /// </summary>
        public Vector2 delta { get; set; }

        [Obsolete("Use HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);", true)]
        public Ray mouseRay
        {
            get => new Ray(Vector3.up, Vector3.up);
            set { }
        }

        /// <summary>
        ///   <para>Is Shift held down? (Read Only)</para>
        /// </summary>
        public bool shift
        {
            get => (modifiers & EventModifiers.Shift) != EventModifiers.None;
            set
            {
                if (!value)
                    modifiers &= ~EventModifiers.Shift;
                else
                    modifiers |= EventModifiers.Shift;
            }
        }

        /// <summary>
        ///   <para>Is Control key held down? (Read Only)</para>
        /// </summary>
        public bool control
        {
            get => (modifiers & EventModifiers.Control) != EventModifiers.None;
            set
            {
                if (!value)
                    modifiers &= ~EventModifiers.Control;
                else
                    modifiers |= EventModifiers.Control;
            }
        }

        /// <summary>
        ///   <para>Is Alt/Option key held down? (Read Only)</para>
        /// </summary>
        public bool alt
        {
            get => (modifiers & EventModifiers.Alt) != EventModifiers.None;
            set
            {
                if (!value)
                    modifiers &= ~EventModifiers.Alt;
                else
                    modifiers |= EventModifiers.Alt;
            }
        }

        /// <summary>
        ///   <para>Is Command/Windows key held down? (Read Only)</para>
        /// </summary>
        public bool command
        {
            get => (modifiers & EventModifiers.Command) != EventModifiers.None;
            set
            {
                if (!value)
                    modifiers &= ~EventModifiers.Command;
                else
                    modifiers |= EventModifiers.Command;
            }
        }

        /// <summary>
        ///   <para>Is Caps Lock on? (Read Only)</para>
        /// </summary>
        public bool capsLock
        {
            get => (modifiers & EventModifiers.CapsLock) != EventModifiers.None;
            set
            {
                if (!value)
                    modifiers &= ~EventModifiers.CapsLock;
                else
                    modifiers |= EventModifiers.CapsLock;
            }
        }

        /// <summary>
        ///   <para>Is the current keypress on the numeric keyboard? (Read Only)</para>
        /// </summary>
        public bool numeric
        {
            get => (modifiers & EventModifiers.Numeric) != EventModifiers.None;
            set
            {
                if (!value)
                    modifiers &= ~EventModifiers.Shift;
                else
                    modifiers |= EventModifiers.Shift;
            }
        }

        /// <summary>
        ///   <para>Is the current keypress a function key? (Read Only)</para>
        /// </summary>
        public bool functionKey => (modifiers & EventModifiers.FunctionKey) != EventModifiers.None;

        /// <summary>
        ///   <para>The current event that's being processed right now.</para>
        /// </summary>
        public static UGUIEvent current
        {
            get => s_Current;
            set
            {
                s_Current = value == null ? s_MasterEvent : value;
                //Event.Internal_SetNativeEvent(Event.s_Current.m_Ptr);
            }
        }

        private static void Internal_MakeMasterEventCurrent(int displayIndex)
        {
            if (s_MasterEvent == null)
                s_MasterEvent = new UGUIEvent(displayIndex);
            s_MasterEvent.displayIndex = displayIndex;
            s_Current = s_MasterEvent;
            //Event.Internal_SetNativeEvent(Event.s_MasterEvent.m_Ptr);
        }

        /// <summary>
        ///   <para>Is this event a keyboard event? (Read Only)</para>
        /// </summary>
        public bool isKey
        {
            get
            {
                EventType type = this.type;
                return type == EventType.KeyDown || type == EventType.KeyUp;
            }
        }

        /// <summary>
        ///   <para>Is this event a mouse event? (Read Only)</para>
        /// </summary>
        public bool isMouse
        {
            get
            {
                EventType type = this.type;
                int num;
                switch (type)
                {
                    case EventType.MouseDown:
                    case EventType.MouseUp:
                    case EventType.MouseMove:
                    case EventType.MouseDrag:
                    case EventType.ContextClick:
                        num = 1;
                        break;
                    default:
                        num = 0;
                        break;
                }
                return num != 0;
            }
        }

        public bool isScrollWheel
        {
            get
            {
                EventType type = this.type;
                return type == EventType.ScrollWheel || type == EventType.ScrollWheel;
            }
        }

        /// <summary>
        ///   <para>Create a keyboard event.</para>
        /// </summary>
        /// <param name="key"></param>
        public static UGUIEvent KeyboardEvent(string key)
        {
            UGUIEvent @event = new UGUIEvent(0);
            @event.type = EventType.KeyDown;
            if (string.IsNullOrEmpty(key))
                return @event;
            int num1 = 0;
            bool flag1 = false;
            bool flag2;
            do
            {
                flag2 = true;
                if (num1 >= key.Length)
                {
                    flag1 = false;
                    break;
                }
                char ch = key[num1];
                switch (ch)
                {
                    case '#':
                        @event.modifiers |= EventModifiers.Shift;
                        ++num1;
                        break;
                    case '%':
                        @event.modifiers |= EventModifiers.Command;
                        ++num1;
                        break;
                    case '&':
                        @event.modifiers |= EventModifiers.Alt;
                        ++num1;
                        break;
                    default:
                        if (ch == '^')
                        {
                            @event.modifiers |= EventModifiers.Control;
                            ++num1;
                            break;
                        }
                        flag2 = false;
                        break;
                }
            }
            while (flag2);
            string lower = key.Substring(num1, key.Length - num1).ToLower();
            if (lower != null)
            {
                if (__switchMap == null)
                {
                    __switchMap = new Dictionary<string, int>(49)
                    {
                        {
                            "[0]",
                            0
                        },
                        {
                            "[1]",
                            1
                        },
                        {
                            "[2]",
                            2
                        },
                        {
                            "[3]",
                            3
                        },
                        {
                            "[4]",
                            4
                        },
                        {
                            "[5]",
                            5
                        },
                        {
                            "[6]",
                            6
                        },
                        {
                            "[7]",
                            7
                        },
                        {
                            "[8]",
                            8
                        },
                        {
                            "[9]",
                            9
                        },
                        {
                            "[.]",
                            10
                        },
                        {
                            "[/]",
                            11
                        },
                        {
                            "[-]",
                            12
                        },
                        {
                            "[+]",
                            13
                        },
                        {
                            "[=]",
                            14
                        },
                        {
                            "[equals]",
                            15
                        },
                        {
                            "[enter]",
                            16
                        },
                        {
                            "up",
                            17
                        },
                        {
                            "down",
                            18
                        },
                        {
                            "left",
                            19
                        },
                        {
                            "right",
                            20
                        },
                        {
                            "insert",
                            21
                        },
                        {
                            "home",
                            22
                        },
                        {
                            "end",
                            23
                        },
                        {
                            "pgup",
                            24
                        },
                        {
                            "page up",
                            25
                        },
                        {
                            "pgdown",
                            26
                        },
                        {
                            "page down",
                            27
                        },
                        {
                            "backspace",
                            28
                        },
                        {
                            "delete",
                            29
                        },
                        {
                            "tab",
                            30
                        },
                        {
                            "f1",
                            31
                        },
                        {
                            "f2",
                            32
                        },
                        {
                            "f3",
                            33
                        },
                        {
                            "f4",
                            34
                        },
                        {
                            "f5",
                            35
                        },
                        {
                            "f6",
                            36
                        },
                        {
                            "f7",
                            37
                        },
                        {
                            "f8",
                            38
                        },
                        {
                            "f9",
                            39
                        },
                        {
                            "f10",
                            40
                        },
                        {
                            "f11",
                            41
                        },
                        {
                            "f12",
                            42
                        },
                        {
                            "f13",
                            43
                        },
                        {
                            "f14",
                            44
                        },
                        {
                            "f15",
                            45
                        },
                        {
                            "[esc]",
                            46
                        },
                        {
                            "return",
                            47
                        },
                        {
                            "space",
                            48
                        }
                    };
                }
                int num2;
                if (__switchMap.TryGetValue(lower, out num2))
                {
                    switch (num2)
                    {
                        case 0:
                            @event.character = '0';
                            @event.keyCode = KeyCode.Keypad0;
                            goto label_73;
                        case 1:
                            @event.character = '1';
                            @event.keyCode = KeyCode.Keypad1;
                            goto label_73;
                        case 2:
                            @event.character = '2';
                            @event.keyCode = KeyCode.Keypad2;
                            goto label_73;
                        case 3:
                            @event.character = '3';
                            @event.keyCode = KeyCode.Keypad3;
                            goto label_73;
                        case 4:
                            @event.character = '4';
                            @event.keyCode = KeyCode.Keypad4;
                            goto label_73;
                        case 5:
                            @event.character = '5';
                            @event.keyCode = KeyCode.Keypad5;
                            goto label_73;
                        case 6:
                            @event.character = '6';
                            @event.keyCode = KeyCode.Keypad6;
                            goto label_73;
                        case 7:
                            @event.character = '7';
                            @event.keyCode = KeyCode.Keypad7;
                            goto label_73;
                        case 8:
                            @event.character = '8';
                            @event.keyCode = KeyCode.Keypad8;
                            goto label_73;
                        case 9:
                            @event.character = '9';
                            @event.keyCode = KeyCode.Keypad9;
                            goto label_73;
                        case 10:
                            @event.character = '.';
                            @event.keyCode = KeyCode.KeypadPeriod;
                            goto label_73;
                        case 11:
                            @event.character = '/';
                            @event.keyCode = KeyCode.KeypadDivide;
                            goto label_73;
                        case 12:
                            @event.character = '-';
                            @event.keyCode = KeyCode.KeypadMinus;
                            goto label_73;
                        case 13:
                            @event.character = '+';
                            @event.keyCode = KeyCode.KeypadPlus;
                            goto label_73;
                        case 14:
                            @event.character = '=';
                            @event.keyCode = KeyCode.KeypadEquals;
                            goto label_73;
                        case 15:
                            @event.character = '=';
                            @event.keyCode = KeyCode.KeypadEquals;
                            goto label_73;
                        case 16:
                            @event.character = '\n';
                            @event.keyCode = KeyCode.KeypadEnter;
                            goto label_73;
                        case 17:
                            @event.keyCode = KeyCode.UpArrow;
                            @event.modifiers |= EventModifiers.FunctionKey;
                            goto label_73;
                        case 18:
                            @event.keyCode = KeyCode.DownArrow;
                            @event.modifiers |= EventModifiers.FunctionKey;
                            goto label_73;
                        case 19:
                            @event.keyCode = KeyCode.LeftArrow;
                            @event.modifiers |= EventModifiers.FunctionKey;
                            goto label_73;
                        case 20:
                            @event.keyCode = KeyCode.RightArrow;
                            @event.modifiers |= EventModifiers.FunctionKey;
                            goto label_73;
                        case 21:
                            @event.keyCode = KeyCode.Insert;
                            @event.modifiers |= EventModifiers.FunctionKey;
                            goto label_73;
                        case 22:
                            @event.keyCode = KeyCode.Home;
                            @event.modifiers |= EventModifiers.FunctionKey;
                            goto label_73;
                        case 23:
                            @event.keyCode = KeyCode.End;
                            @event.modifiers |= EventModifiers.FunctionKey;
                            goto label_73;
                        case 24:
                            @event.keyCode = KeyCode.PageDown;
                            @event.modifiers |= EventModifiers.FunctionKey;
                            goto label_73;
                        case 25:
                            @event.keyCode = KeyCode.PageUp;
                            @event.modifiers |= EventModifiers.FunctionKey;
                            goto label_73;
                        case 26:
                            @event.keyCode = KeyCode.PageUp;
                            @event.modifiers |= EventModifiers.FunctionKey;
                            goto label_73;
                        case 27:
                            @event.keyCode = KeyCode.PageDown;
                            @event.modifiers |= EventModifiers.FunctionKey;
                            goto label_73;
                        case 28:
                            @event.keyCode = KeyCode.Backspace;
                            @event.modifiers |= EventModifiers.FunctionKey;
                            goto label_73;
                        case 29:
                            @event.keyCode = KeyCode.Delete;
                            @event.modifiers |= EventModifiers.FunctionKey;
                            goto label_73;
                        case 30:
                            @event.keyCode = KeyCode.Tab;
                            goto label_73;
                        case 31:
                            @event.keyCode = KeyCode.F1;
                            @event.modifiers |= EventModifiers.FunctionKey;
                            goto label_73;
                        case 32:
                            @event.keyCode = KeyCode.F2;
                            @event.modifiers |= EventModifiers.FunctionKey;
                            goto label_73;
                        case 33:
                            @event.keyCode = KeyCode.F3;
                            @event.modifiers |= EventModifiers.FunctionKey;
                            goto label_73;
                        case 34:
                            @event.keyCode = KeyCode.F4;
                            @event.modifiers |= EventModifiers.FunctionKey;
                            goto label_73;
                        case 35:
                            @event.keyCode = KeyCode.F5;
                            @event.modifiers |= EventModifiers.FunctionKey;
                            goto label_73;
                        case 36:
                            @event.keyCode = KeyCode.F6;
                            @event.modifiers |= EventModifiers.FunctionKey;
                            goto label_73;
                        case 37:
                            @event.keyCode = KeyCode.F7;
                            @event.modifiers |= EventModifiers.FunctionKey;
                            goto label_73;
                        case 38:
                            @event.keyCode = KeyCode.F8;
                            @event.modifiers |= EventModifiers.FunctionKey;
                            goto label_73;
                        case 39:
                            @event.keyCode = KeyCode.F9;
                            @event.modifiers |= EventModifiers.FunctionKey;
                            goto label_73;
                        case 40:
                            @event.keyCode = KeyCode.F10;
                            @event.modifiers |= EventModifiers.FunctionKey;
                            goto label_73;
                        case 41:
                            @event.keyCode = KeyCode.F11;
                            @event.modifiers |= EventModifiers.FunctionKey;
                            goto label_73;
                        case 42:
                            @event.keyCode = KeyCode.F12;
                            @event.modifiers |= EventModifiers.FunctionKey;
                            goto label_73;
                        case 43:
                            @event.keyCode = KeyCode.F13;
                            @event.modifiers |= EventModifiers.FunctionKey;
                            goto label_73;
                        case 44:
                            @event.keyCode = KeyCode.F14;
                            @event.modifiers |= EventModifiers.FunctionKey;
                            goto label_73;
                        case 45:
                            @event.keyCode = KeyCode.F15;
                            @event.modifiers |= EventModifiers.FunctionKey;
                            goto label_73;
                        case 46:
                            @event.keyCode = KeyCode.Escape;
                            goto label_73;
                        case 47:
                            @event.character = '\n';
                            @event.keyCode = KeyCode.Return;
                            @event.modifiers &= ~EventModifiers.FunctionKey;
                            goto label_73;
                        case 48:
                            @event.keyCode = KeyCode.Space;
                            @event.character = ' ';
                            @event.modifiers &= ~EventModifiers.FunctionKey;
                            goto label_73;
                    }
                }
            }
            if (lower.Length != 1)
            {
                try
                {
                    @event.keyCode = (KeyCode)Enum.Parse(typeof(KeyCode), lower, true);
                }
                catch (ArgumentException ex)
                {
                    Debug.LogError(UnityString.Format("Unable to find key name that matches '{0}'", lower));
                }
            }
            else
            {
                @event.character = lower.ToLower()[0];
                @event.keyCode = (KeyCode)@event.character;
                if (@event.modifiers != EventModifiers.None)
                    @event.character = char.MinValue;
            }
        label_73:
            return @event;
        }

        public override int GetHashCode()
        {
            int num = 1;
            if (isKey)
                num = (ushort)keyCode;
            if (isMouse)
                num = mousePosition.GetHashCode();
            return (int)((EventModifiers)(num * 37) | modifiers);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != GetType())
                return false;
            UGUIEvent @event = (UGUIEvent)obj;
            if (type != @event.type || (modifiers & ~EventModifiers.CapsLock) != (@event.modifiers & ~EventModifiers.CapsLock))
                return false;
            if (isKey)
                return keyCode == @event.keyCode;
            return isMouse && mousePosition == @event.mousePosition;
        }

        public override string ToString()
        {
            if (isKey)
                return character == char.MinValue ? UnityString.Format("Event:{0}   Character:\\0   Modifiers:{1}   KeyCode:{2}", type, modifiers, keyCode) : "Event:" + type + "   Character:" + (int)character + "   Modifiers:" + modifiers + "   KeyCode:" + keyCode;
            if (isMouse)
                return UnityString.Format("Event: {0}   Position: {1} Modifiers: {2}", type, mousePosition, modifiers);
            if (type != EventType.ExecuteCommand && type != EventType.ValidateCommand)
                return "" + type;
            return UnityString.Format("Event: {0}  \"{1}\"", type, commandName);
        }

        /// <summary>
        ///   <para>Use this event.</para>
        /// </summary>
        public void Use()
        {
            if (type == EventType.Repaint || type == EventType.Layout)
                Debug.LogWarning(UnityString.Format("Event.Use() should not be called for events of type {0}", type));

            type = EventType.Used;
        }

        private static EventType AsGUIEventType(UGUIEventType type)
        {
            switch (type)
            {
                case UGUIEventType.InitialLayout:
                    return EventType.Layout;
                case UGUIEventType.InitialRepaint:
                    return EventType.Repaint;
                default:
                    return (EventType)(int)type;
            }
        }

        public EventType rawType => AsGUIEventType(uRawType);

        /// <summary>
        ///   The type of event.
        /// </summary>
        public EventType type { get => AsGUIEventType(uRawType); set => uType = (UGUIEventType)(int)value; }

        public UGUIEventType uRawType { get; private set; }

        /// <summary>
        ///   The type of event.
        /// </summary>
        public UGUIEventType uType { get; set; }

        /// <summary>
        ///   Get a filtered event type for a given control ID.
        /// </summary>
        /// <param name="controlID">The ID of the control you are querying from.</param>
        public EventType GetTypeForControl(int controlID) => throw new NotImplementedException();

        /// <summary>
        ///   <para>Which mouse button was pressed.</para>
        /// </summary>
        public int button { get; set; }

        /// <summary>
        ///   <para>Which modifier keys are held down.</para>
        /// </summary>
        public EventModifiers modifiers { get; set; }

        public float pressure { get; set; }

        /// <summary>
        ///   <para>How many consecutive mouse clicks have we received.</para>
        /// </summary>
        public int clickCount { get; set; }

        /// <summary>
        ///   <para>The character typed.</para>
        /// </summary>
        public char character { get; set; }

        /// <summary>
        ///   <para>The name of an ExecuteCommand or ValidateCommand Event.</para>
        /// </summary>
        public string commandName { get; set; }

        /// <summary>
        ///   <para>The raw key code for keyboard events.</para>
        /// </summary>
        public KeyCode keyCode { get; set; }

        /// <summary>
        ///   <para>Index of display that the event belongs to.</para>
        /// </summary>
        public int displayIndex { get; set; } = 0;

        /// <summary>
        ///   <para>Get the next queued [Event] from the event system.</para>
        /// </summary>
        /// <param name="outEvent">Next Event.</param>
        public static bool PopEvent(UGUIEvent outEvent) => throw new NotImplementedException();

        /// <summary>
        ///   <para>Returns the current number of events that are stored in the event queue.</para>
        /// </summary>
        /// <returns>
        ///   <para>Current number of events currently in the event queue.</para>
        /// </returns>
        public static int GetEventCount() => throw new NotImplementedException();
    }
}
