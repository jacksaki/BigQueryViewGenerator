using BigQueryViewGenerator.Models;
using BigQueryViewGenerator.ViewModels;
using System.Data;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows;

namespace BigQueryViewGenerator
{
    public partial class App : Application
    {
        private void InitCulture()
        {
            var newCulture = new CultureInfo(CultureInfo.CurrentCulture.Name);
            CultureInfo.DefaultThreadCurrentCulture = newCulture;
            CultureInfo.DefaultThreadCurrentUICulture = newCulture;
            FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(System.Windows.Markup.XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));
        }
        public AppConfig Config { get; }
        public App() : base()
        {
            this.Config = AppConfig.Load();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            this.Config.CompileAllConverters();
            DispatcherHelper.UIDispatcher = Dispatcher;
            Application.Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            InitCulture();
            var p = GetOtherProcess();
            if (p != null)
            {
                MessageBox.Show("既に起動しています。", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                Current.Shutdown(-1);
            }

            Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
            var mainWindow = new Views.MainWindow();
            mainWindow.DataContext = new MainWindowViewModel(this.Config);
            mainWindow.Closing += MainWindow_Closing;
            this.MainWindow = mainWindow;
            mainWindow.Show();
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Config.Save();
        }

        private System.Diagnostics.Process GetOtherProcess()
        {
            var processes = System.Diagnostics.Process.GetProcessesByName(System.IO.Path.GetFileName(System.Reflection.Assembly.GetExecutingAssembly().Location));
            return processes.Where(x => x.Id != System.Diagnostics.Process.GetCurrentProcess().Id).FirstOrDefault();
        }
        private void Current_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            using (var vm = new ExceptionWindowViewModel(e.Exception, true))
            {
                var window = new Views.ExceptionWindow();
                window.DataContext = vm;
                if (window.ShowDialog() == true)
                {
                    e.Handled = true;
                    return;
                }
                else
                {
                    Current.Shutdown(-1);
                    System.Environment.Exit(1);
                }
            }
        }

        //集約エラーハンドラ
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject == null)
            {
                return;
            }

            using (var vm = new ExceptionWindowViewModel((Exception)e.ExceptionObject, false))
            {
                var window = new Views.ExceptionWindow();
                window.DataContext = vm;
                window.ShowDialog();
                Current.Shutdown(-1);
                System.Environment.Exit(1);
            }
        }
    }
}
