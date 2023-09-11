using UnityEngine;
using UnityEngine.UI;

namespace UniverseLib.UI.Styles
{
    public interface IReadOnlyUISkin : IDeepCopyable<UISkin>
    {
        public string                  Name                { get; }
        public TextComponentStyle      Text                { get; }
        public LayoutGroupStyle        LayoutGroup         { get; }
        public ReadOnlyFrameStyle      Frame               { get; }
        public ReadOnlyButtonStyle     Button              { get; }
        public ReadOnlyToggleStyle     Toggle              { get; }
        public ReadOnlyFrameStyle      Label               { get; }
        public ReadOnlyInputFieldStyle InputField          { get; }
        public ReadOnlyInputFieldStyle ScrollInputField    { get; }
        public ReadOnlyWindowStyle     Window              { get; }
        public ReadOnlySliderStyle     HorizontalSlider    { get; }
        public ReadOnlySliderStyle     VerticalSlider      { get; }
        public ReadOnlyScrollbarStyle  HorizontalScrollbar { get; }
        public ReadOnlyScrollbarStyle  VerticalScrollbar   { get; }
        public ReadOnlyFrameStyle      ScrollView          { get; }
        public ReadOnlyDropdownStyle   Dropdown            { get; }
    }

    [System.Serializable]
    public class UISkin : IReadOnlyUISkin, IConvertibleToReadOnly<ReadOnlyUISkin>
    {
        public string Name;

        /// <summary>
        /// The default text style.
        /// </summary>
        public TextComponentStyle Text               ;
        public LayoutGroupStyle   LayoutGroup        ;
        public FrameStyle         Frame              ;
        public ButtonStyle        Button             ;
        public ToggleStyle        Toggle             ;
        public FrameStyle         Label              ;
        public InputFieldStyle    InputField         ;
        public InputFieldStyle    ScrollInputField   ;
        public WindowStyle        Window             ;
        public SliderStyle        HorizontalSlider   ;
        public SliderStyle        VerticalSlider     ;
        public ScrollbarStyle     HorizontalScrollbar;
        public ScrollbarStyle     VerticalScrollbar  ;
        public FrameStyle         ScrollView         ;
        public DropdownStyle      Dropdown           ;

        
        string                  IReadOnlyUISkin.Name                => Name;
        TextComponentStyle      IReadOnlyUISkin.Text                => Text;
        LayoutGroupStyle        IReadOnlyUISkin.LayoutGroup         => LayoutGroup;
        ReadOnlyFrameStyle      IReadOnlyUISkin.Frame               => Frame              ?.AsReadOnly();
        ReadOnlyButtonStyle     IReadOnlyUISkin.Button              => Button             ?.AsReadOnly();
        ReadOnlyToggleStyle     IReadOnlyUISkin.Toggle              => Toggle             ?.AsReadOnly();
        ReadOnlyFrameStyle      IReadOnlyUISkin.Label               => Label              ?.AsReadOnly();
        ReadOnlyInputFieldStyle IReadOnlyUISkin.InputField          => InputField         ?.AsReadOnly();
        ReadOnlyInputFieldStyle IReadOnlyUISkin.ScrollInputField    => ScrollInputField   ?.AsReadOnly();
        ReadOnlyWindowStyle     IReadOnlyUISkin.Window              => Window             ?.AsReadOnly();
        ReadOnlySliderStyle     IReadOnlyUISkin.HorizontalSlider    => HorizontalSlider   ?.AsReadOnly();
        ReadOnlySliderStyle     IReadOnlyUISkin.VerticalSlider      => VerticalSlider     ?.AsReadOnly();
        ReadOnlyScrollbarStyle  IReadOnlyUISkin.HorizontalScrollbar => HorizontalScrollbar?.AsReadOnly();
        ReadOnlyScrollbarStyle  IReadOnlyUISkin.VerticalScrollbar   => VerticalScrollbar  ?.AsReadOnly();
        ReadOnlyFrameStyle      IReadOnlyUISkin.ScrollView          => ScrollView         ?.AsReadOnly();
        ReadOnlyDropdownStyle   IReadOnlyUISkin.Dropdown            => Dropdown           ?.AsReadOnly();

        public UISkin() { }


        /// <summary>
        /// Returns a deep copy of the skin and its styles.
        /// </summary>
        private UISkin(UISkin toCopy)
        {
            Name                = toCopy.Name + "Copy";
            Text                = toCopy.Text;
            LayoutGroup         = toCopy.LayoutGroup;
            Frame               = toCopy.Frame              ?.DeepCopy();
            Button              = toCopy.Button             ?.DeepCopy();
            Toggle              = toCopy.Toggle             ?.DeepCopy();
            Label               = toCopy.Label              ?.DeepCopy();
            InputField          = toCopy.InputField         ?.DeepCopy();
            ScrollInputField    = toCopy.ScrollInputField   ?.DeepCopy();
            Window              = toCopy.Window             ?.DeepCopy();
            HorizontalSlider    = toCopy.HorizontalSlider   ?.DeepCopy();
            VerticalSlider      = toCopy.VerticalSlider     ?.DeepCopy();
            HorizontalScrollbar = toCopy.HorizontalScrollbar?.DeepCopy();
            VerticalScrollbar   = toCopy.VerticalScrollbar  ?.DeepCopy();
            ScrollView          = toCopy.ScrollView         ?.DeepCopy();
            Dropdown            = toCopy.Dropdown           ?.DeepCopy();
        }
        

        public static readonly ReadOnlyUISkin Default = new UISkin()
        {
            Name = "DefaultUISkin",
            Frame               = new() { Name = nameof(Frame              ), },
            Button              = new() { Name = nameof(Button             ), },
            Toggle              = new() 
            { 
                Name = nameof(Toggle),
                Text = new() { Alignment = TextAnchor.MiddleLeft },
                Checkmark = new() { Color = new(0.2f, 0.4f, 0.28f) },
                CheckboxSize = new(20, 20),
                CheckboxPadding = new Vector4(2, 2, 2, 2),
            },
            Label               = new() { Name = nameof(Label              ), Text = new() { Alignment = TextAnchor.MiddleLeft }, UseBackground = false, },
            InputField          = new() { Name = nameof(InputField         ), Text = new() { Alignment = TextAnchor.MiddleLeft }, },
            ScrollInputField    = new() { Name = nameof(ScrollInputField   ), },
            Window              = new() { Name = nameof(Window             ), },
            HorizontalSlider    = new() { Name = nameof(HorizontalSlider   ), },
            VerticalSlider      = new() { Name = nameof(VerticalSlider     ), },
            HorizontalScrollbar = new() { Name = nameof(HorizontalScrollbar), },
            VerticalScrollbar   = new() { Name = nameof(VerticalScrollbar  ), },
            ScrollView          = new() { Name = nameof(ScrollView         ), },
            Dropdown            = new() { Name = nameof(Dropdown           ), Text = new() { Alignment = TextAnchor.MiddleLeft }, },
        }.AsReadOnly();


        /// <summary>
        /// Returns a deep copy of the skin and its styles.
        /// </summary>
        public UISkin DeepCopy()
        {
            return new UISkin(this);
        }


        /// <summary>
        /// Returns a shallow copy of the skin.
        /// </summary>
        public UISkin ShallowCopy()
        {
            return new UISkin() 
            {
                Name = Name + "Copy",
                Text = Text,
                LayoutGroup = LayoutGroup,
                Frame               = Frame              ,
                Button              = Button             ,
                Toggle              = Toggle             ,
                Label               = Label              ,
                InputField          = InputField         ,
                ScrollInputField    = ScrollInputField   ,
                Window              = Window             ,
                HorizontalSlider    = HorizontalSlider   ,
                VerticalSlider      = VerticalSlider     ,
                HorizontalScrollbar = HorizontalScrollbar,
                VerticalScrollbar   = VerticalScrollbar  ,
                ScrollView          = ScrollView         ,
                Dropdown            = Dropdown           ,
            };
        }

        private ReadOnlyUISkin _readonlySkin = null;
        public ReadOnlyUISkin AsReadOnly()
        {
            _readonlySkin ??= new ReadOnlyUISkin(this);
            return _readonlySkin;
        }
    }


    public class ReadOnlyUISkin : IReadOnlyUISkin
    {
        private readonly UISkin skin;

        public ReadOnlyUISkin(UISkin skin)
        {
            this.skin = skin;
        }

        public override int GetHashCode()
        {
            return skin.GetHashCode();
        }

        public string Name => ((IReadOnlyUISkin)skin).Name;

        public TextComponentStyle Text => ((IReadOnlyUISkin)skin).Text;

        public LayoutGroupStyle LayoutGroup => ((IReadOnlyUISkin)skin).LayoutGroup;

        public ReadOnlyFrameStyle Frame => ((IReadOnlyUISkin)skin).Frame;

        public ReadOnlyButtonStyle Button => ((IReadOnlyUISkin)skin).Button;

        public ReadOnlyToggleStyle Toggle => ((IReadOnlyUISkin)skin).Toggle;

        public ReadOnlyFrameStyle Label => ((IReadOnlyUISkin)skin).Label;

        public ReadOnlyInputFieldStyle InputField => ((IReadOnlyUISkin)skin).InputField;

        public ReadOnlyInputFieldStyle ScrollInputField => ((IReadOnlyUISkin)skin).ScrollInputField;

        public ReadOnlyWindowStyle Window => ((IReadOnlyUISkin)skin).Window;

        public ReadOnlySliderStyle HorizontalSlider => ((IReadOnlyUISkin)skin).HorizontalSlider;

        public ReadOnlySliderStyle VerticalSlider => ((IReadOnlyUISkin)skin).VerticalSlider;

        public ReadOnlyScrollbarStyle HorizontalScrollbar => ((IReadOnlyUISkin)skin).HorizontalScrollbar;

        public ReadOnlyScrollbarStyle VerticalScrollbar => ((IReadOnlyUISkin)skin).VerticalScrollbar;

        public ReadOnlyFrameStyle ScrollView => ((IReadOnlyUISkin)skin).ScrollView;

        public ReadOnlyDropdownStyle Dropdown => ((IReadOnlyUISkin)skin).Dropdown;


        public UISkin DeepCopy()
        {
            return ((IDeepCopyable<UISkin>)skin).DeepCopy();
        }
    }
}
