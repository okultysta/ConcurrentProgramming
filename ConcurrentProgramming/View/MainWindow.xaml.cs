using System.Windows;

namespace View
{
    public partial class MainWindow : Window
    {
        private MainViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();
            viewModel = new MainViewModel();
            this.DataContext = viewModel;
        }

        private void BoardCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            viewModel.UpdateBoardSize(BoardCanvas.ActualWidth, BoardCanvas.ActualHeight);
        }
    }
}
