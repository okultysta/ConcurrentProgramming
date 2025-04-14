using DataLayer;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;

namespace View
{
    public partial class MainWindow : Window
    {
        private MainViewModel viewModel;
       
        public MainWindow()
        {
            InitializeComponent();
            SetDependencies();
            this.DataContext = viewModel;
        }

        private void BoardCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            viewModel.UpdateBoardSize(BoardCanvas.ActualWidth, BoardCanvas.ActualHeight);
        }

        private void SetDependencies()
        {
            IBallRepository repo = new MemoryBallRepository();
            IBallManager manager = new BallLogic(repo, 400, 400);
            viewModel = new MainViewModel(manager);
        }
    }
}
