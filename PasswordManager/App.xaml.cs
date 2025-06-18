using PasswordManager.Interfaces;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;
using System.Windows;

namespace PasswordManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private CompositionContainer container;

        protected override void OnStartup(StartupEventArgs e)
        {
            var catalog = new AssemblyCatalog(Assembly.GetExecutingAssembly());
            container = new CompositionContainer(catalog);

            var viewModel = container.GetExportedValue<IPasswordManagerViewModel>();

            var mainWindow = new MainWindow
            {
                DataContext = viewModel
            };

            mainWindow.Show();
        }
    }
}
