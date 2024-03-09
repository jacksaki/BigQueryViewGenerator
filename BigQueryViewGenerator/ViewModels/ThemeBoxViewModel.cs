using MaterialDesignThemes.Wpf;
using R3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigQueryViewGenerator.ViewModels
{
    public class ThemeBoxViewModel : MenuViewModelBase
    {
        public ThemeBoxViewModel(MainWindowViewModel parent) : base(parent)
        {
            var paletteHelper = new PaletteHelper();
            ITheme theme = paletteHelper.GetTheme();

            this.IsDarkTheme = new BindableReactiveProperty<bool>(theme.GetBaseTheme() == BaseTheme.Dark);
            this.IsColorAdjusted = new BindableReactiveProperty<bool>();
            this.DesiredContrastRatio = new BindableReactiveProperty<float>(4.5f);
            this.ContrastValue = new BindableReactiveProperty<Contrast>();
            this.ColorSelectionValue = new BindableReactiveProperty<ColorSelection>();

            if (theme is Theme internalTheme)
            {
                this.IsColorAdjusted.Value = internalTheme.ColorAdjustment is not null;

                var colorAdjustment = internalTheme.ColorAdjustment ?? new ColorAdjustment();
                this.DesiredContrastRatio.Value = colorAdjustment.DesiredContrastRatio;
                this.ContrastValue.Value = colorAdjustment.Contrast;
                this.ColorSelectionValue.Value = colorAdjustment.Colors;
            }

            if (paletteHelper.GetThemeManager() is { } themeManager)
            {
                themeManager.ThemeChanged += (_, e) =>
                {
                    this.IsDarkTheme.Value = e.NewTheme?.GetBaseTheme() == BaseTheme.Dark;
                };
            }

            this.IsDarkTheme.Subscribe(x =>
            {
                ModifyTheme(theme => theme.SetBaseTheme(x ? Theme.Dark : Theme.Light));
            });
            this.IsColorAdjusted.Subscribe(x =>
            {
                ModifyTheme(theme =>
                {
                    if (theme is Theme internalTheme)
                    {
                        internalTheme.ColorAdjustment = x
                            ? new ColorAdjustment
                            {
                                DesiredContrastRatio = this.DesiredContrastRatio.Value,
                                Contrast = this.ContrastValue.Value,
                                Colors = this.ColorSelectionValue.Value,
                            }
                            : null;
                    }
                });
            });
            this.DesiredContrastRatio.Subscribe(x =>
            {
                ModifyTheme(theme =>
                {
                    if (theme is Theme internalTheme && internalTheme.ColorAdjustment != null)
                        internalTheme.ColorAdjustment.DesiredContrastRatio = x;
                });
            });
            this.ContrastValue.Subscribe(x =>
            {
                ModifyTheme(theme =>
                {
                    if (theme is Theme internalTheme && internalTheme.ColorAdjustment != null)
                        internalTheme.ColorAdjustment.Contrast = x;
                });
            });
            this.ColorSelectionValue.Subscribe(x =>
            {
                ModifyTheme(theme =>
                {
                    if (theme is Theme internalTheme && internalTheme.ColorAdjustment != null)
                        internalTheme.ColorAdjustment.Colors = x;
                });
            });
        }
        public BindableReactiveProperty<bool> IsDarkTheme { get; }
        public BindableReactiveProperty<bool> IsColorAdjusted { get; }
        public BindableReactiveProperty<float> DesiredContrastRatio { get; }
        public BindableReactiveProperty<Contrast> ContrastValue { get; }
        public BindableReactiveProperty<ColorSelection> ColorSelectionValue { get; }
        public IEnumerable<Contrast> ContrastValues => Enum.GetValues(typeof(Contrast)).Cast<Contrast>();
        public IEnumerable<ColorSelection> ColorSelectionValues => Enum.GetValues(typeof(ColorSelection)).Cast<ColorSelection>();

        private static void ModifyTheme(Action<ITheme> modificationAction)
        {
            var paletteHelper = new PaletteHelper();
            ITheme theme = paletteHelper.GetTheme();

            modificationAction?.Invoke(theme);

            paletteHelper.SetTheme(theme);
        }
    }
}
