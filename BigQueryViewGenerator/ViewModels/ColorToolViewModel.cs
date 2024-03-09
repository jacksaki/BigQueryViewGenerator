using System.Windows.Media;
using BigQueryViewGenerator.Models;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using R3;

namespace BigQueryViewGenerator.ViewModels
{
    internal class ColorToolViewModel : ViewModelBase
    {
        private readonly PaletteHelper _paletteHelper = new PaletteHelper();
        public ColorToolViewModel()
        {
            ToggleBaseCommand = new ReactiveCommand<bool>();
            ToggleBaseCommand.Subscribe(x => ApplyBase(x));
            this.ChangeHueCommand = new ReactiveCommand<Color>();
            this.ChangeHueCommand.Subscribe(x => ChangeHue(x));
            this.ChangeCustomHueCommand = new ReactiveCommand<Color>();
            this.ChangeCustomHueCommand.Subscribe(x => ChangeCustomColor(x));
            this.ChangeToPrimaryCommand = new ReactiveCommand<Unit>();
            this.ChangeToPrimaryCommand.Subscribe(_=>ChangeScheme(ColorScheme.Primary));
            this.ChangeToSecondaryCommand = new ReactiveCommand<Unit>();
            this.ChangeToSecondaryCommand .Subscribe(_=>ChangeScheme(ColorScheme.Secondary));
            this.ChangeToPrimaryForegroundCommand = new ReactiveCommand<Unit>();
            this.ChangeToPrimaryForegroundCommand .Subscribe(_=>ChangeScheme(ColorScheme.PrimaryForeground));
            this.ChangeToSecondaryForegroundCommand = new ReactiveCommand<Unit>();
            this.ChangeToSecondaryForegroundCommand .Subscribe(_=>ChangeScheme(ColorScheme.SecondaryForeground));

            ITheme theme = _paletteHelper.GetTheme();
            _primaryColor = theme.PrimaryMid.Color;
            _secondaryColor = theme.SecondaryMid.Color;

            this.SelectedColor = new BindableReactiveProperty<Color?>(_primaryColor);
            this.SelectedColor.Subscribe(x =>
            {
                var currentSchemeColor = ActiveScheme.Value switch
                {
                    ColorScheme.Primary => _primaryColor,
                    ColorScheme.Secondary => _secondaryColor,
                    ColorScheme.PrimaryForeground => _primaryForegroundColor,
                    ColorScheme.SecondaryForeground => _secondaryForegroundColor,
                    _ => throw new NotSupportedException($"{ActiveScheme} is not a handled ColorScheme.. Ye daft programmer!")
                };

                if (x != currentSchemeColor && x is Color color)
                {
                    ChangeCustomColor(color);
                }
            });
        }

        public BindableReactiveProperty<ColorScheme> ActiveScheme { get; }

        public BindableReactiveProperty<Color?> SelectedColor { get; }

        public IEnumerable<ISwatch> Swatches { get; } = SwatchHelper.Swatches;

        public ReactiveCommand<Color> ChangeCustomHueCommand { get; }

        public ReactiveCommand<Color> ChangeHueCommand { get; }
        public ReactiveCommand<Unit> ChangeToPrimaryCommand { get; }
        public ReactiveCommand<Unit> ChangeToSecondaryCommand { get; }
        public ReactiveCommand<Unit> ChangeToPrimaryForegroundCommand { get; }
        public ReactiveCommand<Unit> ChangeToSecondaryForegroundCommand { get; }

        public ReactiveCommand<bool> ToggleBaseCommand { get; }

        private void ApplyBase(bool isDark)
        {
            ITheme theme = _paletteHelper.GetTheme();
            IBaseTheme baseTheme = isDark ? new MaterialDesignDarkTheme() : new MaterialDesignLightTheme();
            theme.SetBaseTheme(baseTheme);
            _paletteHelper.SetTheme(theme);
        }


        private void ChangeCustomColor(Color color)
        {
            if (ActiveScheme.Value == ColorScheme.Primary)
            {
                _paletteHelper.ChangePrimaryColor(color);
                _primaryColor = color;
            }
            else if (ActiveScheme.Value == ColorScheme.Secondary)
            {
                _paletteHelper.ChangeSecondaryColor(color);
                _secondaryColor = color;
            }
            else if (ActiveScheme.Value == ColorScheme.PrimaryForeground)
            {
                SetPrimaryForegroundToSingleColor(color);
                _primaryForegroundColor = color;
            }
            else if (ActiveScheme.Value == ColorScheme.SecondaryForeground)
            {
                SetSecondaryForegroundToSingleColor(color);
                _secondaryForegroundColor = color;
            }
        }

        private void ChangeScheme(ColorScheme scheme)
        {
            ActiveScheme.Value = scheme;
            if (ActiveScheme.Value == ColorScheme.Primary)
            {
                SelectedColor.Value = _primaryColor;
            }
            else if (ActiveScheme.Value == ColorScheme.Secondary)
            {
                SelectedColor.Value = _secondaryColor;
            }
            else if (ActiveScheme.Value == ColorScheme.PrimaryForeground)
            {
                SelectedColor.Value = _primaryForegroundColor;
            }
            else if (ActiveScheme.Value == ColorScheme.SecondaryForeground)
            {
                SelectedColor.Value = _secondaryForegroundColor;
            }
        }

        private Color? _primaryColor;

        private Color? _secondaryColor;

        private Color? _primaryForegroundColor;

        private Color? _secondaryForegroundColor;

        private void ChangeHue(Color color)
        {
            SelectedColor.Value = color;
            if (ActiveScheme.Value == ColorScheme.Primary)
            {
                _paletteHelper.ChangePrimaryColor(color);
                _primaryColor = color;
                _primaryForegroundColor = _paletteHelper.GetTheme().PrimaryMid.GetForegroundColor();
            }
            else if (ActiveScheme.Value == ColorScheme.Secondary)
            {
                _paletteHelper.ChangeSecondaryColor(color);
                _secondaryColor = color;
                _secondaryForegroundColor = _paletteHelper.GetTheme().SecondaryMid.GetForegroundColor();
            }
            else if (ActiveScheme.Value == ColorScheme.PrimaryForeground)
            {
                SetPrimaryForegroundToSingleColor(color);
                _primaryForegroundColor = color;
            }
            else if (ActiveScheme.Value == ColorScheme.SecondaryForeground)
            {
                SetSecondaryForegroundToSingleColor(color);
                _secondaryForegroundColor = color;
            }
        }

        private void SetPrimaryForegroundToSingleColor(Color color)
        {
            ITheme theme = _paletteHelper.GetTheme();

            theme.PrimaryLight = new ColorPair(theme.PrimaryLight.Color, color);
            theme.PrimaryMid = new ColorPair(theme.PrimaryMid.Color, color);
            theme.PrimaryDark = new ColorPair(theme.PrimaryDark.Color, color);

            _paletteHelper.SetTheme(theme);
        }

        private void SetSecondaryForegroundToSingleColor(Color color)
        {
            ITheme theme = _paletteHelper.GetTheme();

            theme.SecondaryLight = new ColorPair(theme.SecondaryLight.Color, color);
            theme.SecondaryMid = new ColorPair(theme.SecondaryMid.Color, color);
            theme.SecondaryDark = new ColorPair(theme.SecondaryDark.Color, color);

            _paletteHelper.SetTheme(theme);
        }
    }
}
