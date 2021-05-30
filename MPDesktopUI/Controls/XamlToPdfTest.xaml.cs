using MPData;
using System.Windows.Controls;

namespace MPDesktopUI
{
    /// <summary>
    /// Interaction logic for XamlToPdfTest.xaml
    /// </summary>
    public partial class XamlToPdfTest : UserControl
    {
        public XamlToPdfTest(MainViewModel mainViewModel)
        {
            InitializeComponent();

            this.DataContext = mainViewModel;
        }
    }
}
