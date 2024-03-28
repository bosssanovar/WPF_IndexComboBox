using Reactive.Bindings;

using System.Reactive.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<ComboBoxItem<int>> ComboItems { get; } = new List<ComboBoxItem<int>>();

        public ReactivePropertySlim<int> SelectedValue { get; } = new ReactivePropertySlim<int>(1);

        public ReadOnlyReactivePropertySlim<string?> SelectedDisplay { get; }

        public MainWindow()
        {
            InitComboBoxItems();

            SelectedDisplay = SelectedValue.Select(x =>
            {
                return ComboItems.Where(x => x.Val == SelectedValue.Value)
                .Select(x => x.Text)
                .FirstOrDefault();
            }).ToReadOnlyReactivePropertySlim();

            InitializeComponent();
        }

        private void InitComboBoxItems()
        {
            Random r = new Random();

            for (int number = 1; number <= 640; number++)
            {
                int length = r.Next(4, 32);
                ComboItems.Add(new(number, GenerateText(length)));
            }
        }

        private static readonly string chars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

        /// <summary>
        /// ランダムな文字列を生成する
        /// </summary>
        /// <param name="length">生成する文字列の長さ</param>
        /// <returns>生成された文字列</returns>
        public string GenerateText(int length)
        {
            StringBuilder sb = new StringBuilder(length);
            Random r = new Random();

            for (int i = 0; i < length; i++)
            {
                int pos = r.Next(chars.Length);
                char c = chars[pos];
                sb.Append(c);
            }

            return sb.ToString();
        }
    }

    public record ComboBoxItem<T>(T Val, string Text)
    {
        public string Disp
        {
            get
            {
                return $"{Val} : {Text}";
            }
        }
    }

    public class AddressTemplateSelector : System.Windows.Controls.DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            ContentPresenter presenter = (ContentPresenter)container;

            if (presenter.TemplatedParent is ComboBox)
            {
                return (DataTemplate)presenter.FindResource("AddressComboCollapsed");
            }
            else // Templated parent is ComboBoxItem
            {
                return (DataTemplate)presenter.FindResource("AddressComboExpanded");
            }
        }
    }
}