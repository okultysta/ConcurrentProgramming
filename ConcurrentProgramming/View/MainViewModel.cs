using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Threading;

namespace View
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly IBallManager simulation;
        public ObservableCollection<BallModel> Balls { get; set; } = new();

        public ICommand StartSimulationCommand { get; }
        public ICommand AddBallCommand { get; }
        public ICommand RemoveBallCommand { get; }

        private DispatcherTimer timer;
        private double boardWidth;
        private double boardHeight;

        private int waitingAddCount = 0;
        private int waitingRemoveCount = 0;

        public MainViewModel() { }

        public MainViewModel(IBallManager manager)
        {
            simulation = manager;
            StartSimulationCommand = new RelayCommand(StartSimulation);
            AddBallCommand = new RelayCommand(QueueAddBall);
            RemoveBallCommand = new RelayCommand(QueueRemoveBall);
        }

        public void UpdateBoardSize(double newWidth, double newHeight)
        {
            boardWidth = newWidth;
            boardHeight = newHeight;
            simulation.updateFrameSize(boardWidth, boardHeight);
        }

        private void StartSimulation()
        {
            simulation.CreateBall();
            simulation.CreateBall();
            Balls.Clear();

            foreach (var b in simulation.GetCurrentBallStates())
            {
                Balls.Add(new BallModel { X = b.x, Y = b.y, Radius = b.radius });
            }

            timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(20) };
            timer.Tick += (s, e) => UpdateBalls();
            timer.Start();
        }

        private void UpdateBalls()
        {
            
            for (int i = 0; i < waitingAddCount; i++)
            {
                simulation.CreateBall();
                var newBall = simulation.GetCurrentBallStates().Last();
                Balls.Add(new BallModel { X = newBall.x, Y = newBall.y, Radius = newBall.radius });
            }
            waitingAddCount = 0;

            for (int i = 0; i < waitingRemoveCount && Balls.Count > 0; i++)
            {
                simulation.DeleteBall();
                Balls.RemoveAt(Balls.Count - 1);
            }
            waitingRemoveCount = 0;

            // Aktualizacja pozycji kulek
            simulation.UpdateBallPositions();
            var updated = simulation.GetCurrentBallStates();

            for (int i = 0; i < updated.Count(); i++)
            {
                var ball = Balls[i];
                var newBallState = updated.ElementAt(i);
                ball.X = newBallState.x;
                ball.Y = newBallState.y;
            }
        }

        private void QueueAddBall()
        {
            waitingAddCount++;
        }

        private void QueueRemoveBall()
        {
            waitingRemoveCount++;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
