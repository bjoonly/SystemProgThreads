using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;


namespace SystemProgThreads
{
    class ViewModel : INotifyPropertyChanged
    {
        private bool isContinuePrimaryNumbers;
        private bool isContinueFibonacciNumbers;

        public bool IsContinuePrimaryNumbers { get { return isContinuePrimaryNumbers; } set { if (value != isContinuePrimaryNumbers) { isContinuePrimaryNumbers = value; OnPropertyChanged(); } } }
        public bool IsContinueFibonacciNumbers { get => isContinueFibonacciNumbers; set { if (value != isContinueFibonacciNumbers) { isContinueFibonacciNumbers = value; OnPropertyChanged(); } } }


        private bool isPausedPrimaryNumbers;
        private bool isPausedFibonacciNumbers;
        public bool IsPausedPrimaryNumbers { get => isPausedPrimaryNumbers; set { if (value != isPausedPrimaryNumbers) { isPausedPrimaryNumbers = value; OnPropertyChanged(); } } }
        public bool IsPausedFibonacciNumbers { get => isPausedFibonacciNumbers; set { if (value != isPausedFibonacciNumbers) { isPausedFibonacciNumbers = value; OnPropertyChanged(); } } }

        private bool isStopedPrimaryNumbers;
        private bool isStopedFibonacciNumbers;

        public bool IsStopedPrimaryNumbers { get => isStopedPrimaryNumbers; set { if (value != isStopedPrimaryNumbers) { isStopedPrimaryNumbers = value; OnPropertyChanged(); } } }
        public bool IsStopedFibonacciNumbers { get => isStopedFibonacciNumbers; set { if (value != isStopedFibonacciNumbers) { isStopedFibonacciNumbers = value; OnPropertyChanged(); } } }


        private Thread primaryNumbersGenereteThread;
        private Thread fibonacciNumbersGenereteThread;


        private ulong? upperNumber;
        public ulong? UpperNumber { get { return upperNumber; } set { if (value != upperNumber) { upperNumber = value; OnPropertyChanged(); } } }

        private ulong? bottomNumber;
        public ulong? BottomNumber { get { return bottomNumber; } set { if (value != bottomNumber) { bottomNumber = value; OnPropertyChanged(); } } }

        private ulong currentPrimaryNumber;
        private ulong n1;
        private ulong n2;
        public ViewModel()
        {
            isPausedPrimaryNumbers = false;
            isContinuePrimaryNumbers = false;

            isPausedFibonacciNumbers = false;
            isContinueFibonacciNumbers = false;

            isStopedPrimaryNumbers = true;
            isStopedFibonacciNumbers = true;

            primaryNumbersStartCommand = new DelegateCommand(StartGeneratePrimaryNumbers, () => IsStopedPrimaryNumbers == true && IsContinuePrimaryNumbers == false);
            primaryNumbersStopCommand = new DelegateCommand(StopGeneratePrimaryNumbers, () => IsPausedPrimaryNumbers == false && IsStopedPrimaryNumbers == false);
            primaryNumbersPauseCommand = new DelegateCommand(PauseGeneratPrimaryNumbers, () => IsPausedPrimaryNumbers == false && IsStopedPrimaryNumbers == false);
            primaryNumbersContinueCommand = new DelegateCommand(ContinueGeneratePrimaryNumbers, () => IsPausedPrimaryNumbers == true && IsStopedPrimaryNumbers == false);


            fibonacciNumbersStartCommand = new DelegateCommand(StartGeneratFibonacciNumbers, () => IsStopedFibonacciNumbers == true && IsContinueFibonacciNumbers == false);
            fibonacciNumbersStopCommand = new DelegateCommand(StopGenerateFibonacciNumbers, () => IsPausedFibonacciNumbers == false && IsStopedFibonacciNumbers == false);
            fibonacciNumbersPauseCommand = new DelegateCommand(PauseGenerateFibonacciNumbers, () => IsPausedFibonacciNumbers == false && IsStopedFibonacciNumbers == false);
            fibonacciNumbersContinueCommand = new DelegateCommand(ContinueGenerateFibonacciNumbers, () => IsPausedFibonacciNumbers == true && IsStopedFibonacciNumbers == false);

            PropertyChanged += (sender, args) =>
            {


                if (args.PropertyName == nameof(IsStopedPrimaryNumbers))
                {
                    primaryNumbersStartCommand.RaiseCanExecuteChanged();
                    primaryNumbersStopCommand.RaiseCanExecuteChanged();
                    primaryNumbersPauseCommand.RaiseCanExecuteChanged();
                    primaryNumbersContinueCommand.RaiseCanExecuteChanged();
                }
                else if (args.PropertyName == nameof(IsPausedPrimaryNumbers))
                {
                    primaryNumbersStopCommand.RaiseCanExecuteChanged();
                    primaryNumbersPauseCommand.RaiseCanExecuteChanged();
                    primaryNumbersContinueCommand.RaiseCanExecuteChanged();
                }
                else if (args.PropertyName == nameof(IsContinuePrimaryNumbers))
                {
                    primaryNumbersStartCommand.RaiseCanExecuteChanged();
                }

                else if (args.PropertyName == nameof(IsStopedFibonacciNumbers))
                {
                    fibonacciNumbersStartCommand.RaiseCanExecuteChanged();
                    fibonacciNumbersStopCommand.RaiseCanExecuteChanged();
                    fibonacciNumbersPauseCommand.RaiseCanExecuteChanged();
                    fibonacciNumbersContinueCommand.RaiseCanExecuteChanged();
                }
                else if (args.PropertyName == nameof(IsPausedFibonacciNumbers))
                {
                    fibonacciNumbersStopCommand.RaiseCanExecuteChanged();
                    fibonacciNumbersPauseCommand.RaiseCanExecuteChanged();
                    fibonacciNumbersContinueCommand.RaiseCanExecuteChanged();
                }
                else if (args.PropertyName == nameof(IsContinueFibonacciNumbers))
                {
                    fibonacciNumbersStartCommand.RaiseCanExecuteChanged();
                }

            };
        }


        public void StartGeneratFibonacciNumbers()
        {
            IsPausedFibonacciNumbers = false;
            IsContinueFibonacciNumbers = false;
            IsStopedFibonacciNumbers = false;
            fibonacciNumbersGenereteThread = new Thread(GenerateFibonacciNumbers);
            fibonacciNumbersGenereteThread.Start();
        }

        public void StartGeneratePrimaryNumbers()
        {
            IsPausedPrimaryNumbers = false;
            IsContinuePrimaryNumbers = false;
            IsStopedPrimaryNumbers = false;
            primaryNumbersGenereteThread = new Thread(GeneratePrimaryNumber);
            primaryNumbersGenereteThread.Start();

        }
        public void PauseGeneratPrimaryNumbers()
        {
            IsPausedPrimaryNumbers = true;
            IsContinuePrimaryNumbers = false;
            IsStopedPrimaryNumbers = false;
            primaryNumbersGenereteThread.Suspend();
        }
        public void ContinueGeneratePrimaryNumbers()
        {
            IsPausedPrimaryNumbers = false;
            IsContinuePrimaryNumbers = true;
            isStopedPrimaryNumbers = false;
            primaryNumbersGenereteThread.Resume();

        }
        public void StopGeneratePrimaryNumbers()
        {
            IsPausedPrimaryNumbers = false;
            IsContinuePrimaryNumbers = false;
            IsStopedPrimaryNumbers = true;
            primaryNumbersGenereteThread.Abort();
        }
        public void StopGenerateFibonacciNumbers()
        {
            IsPausedFibonacciNumbers = false;
            IsContinueFibonacciNumbers = false;
            IsStopedFibonacciNumbers = true;
            fibonacciNumbersGenereteThread.Abort();

        }
        public void PauseGenerateFibonacciNumbers()
        {
            IsPausedFibonacciNumbers = true;
            IsContinueFibonacciNumbers = false;
            IsStopedFibonacciNumbers = false;
            fibonacciNumbersGenereteThread.Suspend();
        }
        public void ContinueGenerateFibonacciNumbers()
        {
            IsPausedFibonacciNumbers = false;
            IsContinueFibonacciNumbers = true;
            IsStopedFibonacciNumbers = false;
            fibonacciNumbersGenereteThread.Resume();

        }
        public void GeneratePrimaryNumber()
        {
            if (!isContinuePrimaryNumbers)
            {
                if (bottomNumber == null)
                    bottomNumber = 2;
                if (bottomNumber < 0)
                    bottomNumber = 2;

                currentPrimaryNumber = (ulong)bottomNumber;
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    primaryNumbers.Clear();
                }));

            }
            int count;
            try
            {
                do
                {
                    count = 0;
                    for (ulong i = 2; i <= currentPrimaryNumber / 2; i++)
                    {
                        if (currentPrimaryNumber % i == 0)
                        {
                            count++;
                            break;
                        }
                    }
                    if (currentPrimaryNumber >= 2 && count == 0)
                    {
                        Application.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            primaryNumbers.Add(currentPrimaryNumber);
                        }));
                        Thread.Sleep(50);
                    }
                    currentPrimaryNumber=checked(currentPrimaryNumber+1);
                } while ((upperNumber != null && currentPrimaryNumber <= (ulong)upperNumber) || (upperNumber == null));
            }catch (OverflowException ex)
            {
                MessageBox.Show(ex.Message);               
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    StopGeneratePrimaryNumbers();
                }));
            }
        }


        public void GenerateFibonacciNumbers()
        {
            if (!IsContinueFibonacciNumbers)
            {
                n1 = 0;
                n2 = 1;
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    fibonacciNumbers.Clear();
                    fibonacciNumbers.Add(n1);
                    fibonacciNumbers.Add(n2);
                }));
            }
            try
            {
                do
                {
                    ulong temp = n1;
                    n1 = n2;
                    

                        n2 =checked(n2+temp);
                        Application.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            fibonacciNumbers.Add(n2);
                        }));
                        Thread.Sleep(100);
                    
                } while (n2 < ulong.MaxValue);
            }
            catch (OverflowException ex)
            {
                MessageBox.Show(ex.Message);
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    StopGenerateFibonacciNumbers();
                }));
                              
            }
            
        }



        private Command primaryNumbersStartCommand;
        private Command primaryNumbersStopCommand;
        private Command primaryNumbersPauseCommand;
        private Command primaryNumbersContinueCommand;

        private Command fibonacciNumbersStartCommand;
        private Command fibonacciNumbersStopCommand;
        private Command fibonacciNumbersPauseCommand;
        private Command fibonacciNumbersContinueCommand;



        public ICommand PrimaryNumbersStartCommand => primaryNumbersStartCommand;
        public ICommand PrimaryNumbersStopCommand => primaryNumbersStopCommand;
        public ICommand PrimaryNumbersPauseCommand => primaryNumbersPauseCommand;
        public ICommand PrimaryNumbersContinueCommand => primaryNumbersContinueCommand;

        public ICommand FibonacciNumbersStartCommand => fibonacciNumbersStartCommand;
        public ICommand FibonacciNumbersStopCommand => fibonacciNumbersStopCommand;
        public ICommand FibonacciNumbersPauseCommand => fibonacciNumbersPauseCommand;
        public ICommand FibonacciNumbersContinueCommand => fibonacciNumbersContinueCommand;



        private readonly ICollection<ulong> primaryNumbers = new ObservableCollection<ulong>();
        private readonly ICollection<ulong> fibonacciNumbers = new ObservableCollection<ulong>();

        public IEnumerable<ulong> PrimaryNumbers => primaryNumbers;
        public IEnumerable<ulong> FibonacciNumbers => fibonacciNumbers;



        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
