using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Threading;
using DataLayer;
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

        public MainViewModel()
        {
            DataLayer.IBallRepository repo = new DataLayer.MemoryBallRepository();
            simulation = new BallLogic(repo, 400, 400);
            StartSimulationCommand = new RelayCommand(StartSimulation);
            AddBallCommand = new RelayCommand(AddBall);
            RemoveBallCommand = new RelayCommand(RemoveBall);

        }

        private void StartSimulation()
        {
            simulation.CreateBall();
            // Tworzenie kul
            simulation.CreateBall();
            Balls.Clear();

            // Dodawanie kul do kolekcji BallModel
            foreach (var b in simulation.GetCurrentBallStates())
            {
                Balls.Add(new BallModel { X = b.x, Y = b.y });
            }

            timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(50) };
            timer.Tick += (s, e) => UpdateBalls();
            timer.Start();
            System.Diagnostics.Debug.WriteLine("Symulacja uruchomiona!");
        }

        private void UpdateBalls()
        {
            simulation.UpdateBallPositions();
            var updated = simulation.GetCurrentBallStates();
            //Console.WriteLine("Aktualizacja pozycji");
            for (int i = 0; i < updated.Count(); i++)
            {
                Balls[i].X = updated.ElementAt(i).x;
                Balls[i].Y = updated.ElementAt(i).y;
                //System.Diagnostics.Debug.WriteLine($"Kulka {i} - Nowa pozycja: X = {Balls[i].X}, Y = {Balls[i].Y}");
            }
        }
        private void AddBall()
        {
            simulation.CreateBall();
            var newBall = simulation.GetCurrentBallStates().Last();
            Balls.Add(new BallModel { X = newBall.x, Y = newBall.y });
        }

        private void RemoveBall()
        {
            if (Balls.Count > 0)
            {
                simulation.DeleteBall();
                Balls.RemoveAt(Balls.Count - 1);
            }
        }

            
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
