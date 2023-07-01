using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CatsCalculator
{
    public partial class MainWindow : Window
    {
        Number NumberMain = new Number(0,0);
        Number NumberSecond = new Number(0, 0);
        Number NumberMemory = new Number(0, 0);
        Operation currentOperation = null;

        // событие нажатия на кнопку клавиатуры
        void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Divide: SetCurrentOperation(new OperDivide()); break;
                case Key.Multiply: SetCurrentOperation(new OperMultiply()); break;
                case Key.Subtract: SetCurrentOperation(new OperMinus()); break;
                case Key.Add: SetCurrentOperation(new OperPlus()); break;

                case Key.D0: AddDigitToEnd(0); break;
                case Key.D1: AddDigitToEnd(1); break;
                case Key.D2: AddDigitToEnd(2); break;
                case Key.D3: AddDigitToEnd(3); break;
                case Key.D4: AddDigitToEnd(4); break;
                case Key.D5: AddDigitToEnd(5); break;
                case Key.D6: AddDigitToEnd(6); break;
                case Key.D7: AddDigitToEnd(7); break;
                case Key.D8: AddDigitToEnd(8); break;
                case Key.D9: AddDigitToEnd(9); break;

                case Key.NumPad0: AddDigitToEnd(0); break;
                case Key.NumPad1: AddDigitToEnd(1); break;
                case Key.NumPad2: AddDigitToEnd(2); break;
                case Key.NumPad3: AddDigitToEnd(3); break;
                case Key.NumPad4: AddDigitToEnd(4); break;
                case Key.NumPad5: AddDigitToEnd(5); break;
                case Key.NumPad6: AddDigitToEnd(6); break;
                case Key.NumPad7: AddDigitToEnd(7); break;
                case Key.NumPad8: AddDigitToEnd(8); break;
                case Key.NumPad9: AddDigitToEnd(9); break;

                case Key.Delete: ClearAll(); break;
                case Key.Back: RemoveDigitFromEnd(); break;
                case Key.Enter: Execute(); break;
                case Key.OemComma: AppendPoint(); break;
                default: break;
            }
        }
        public void AppendPoint()
        {
            NumberMain.AppendPoint();
            UpdateMainNumber(NumberMain);
        }
        public void AddDigitToEnd(int digit)
        {
            try
            {
                NumberMain.AddDigitToEnd(digit);
                UpdateMainNumber(NumberMain);
            }
            catch
            {

            }
        }
        public void RemoveDigitFromEnd()
        {
            NumberMain.RemoveDigitFromEnd();
            UpdateMainNumber(NumberMain);
        }
        public void ClearAll()
        {
            NumberMain.Normal = 0;
            NumberMain.PointIndex = 0;
            NumberSecond.Normal = 0;
            NumberSecond.PointIndex = 0;
            currentOperation = null;
            UpdateScreen();
        }
        public void ClearMain()
        {
            NumberMain.Normal = 0;
            NumberMain.PointIndex = 0;
            UpdateMainNumber(NumberMain);
        }
        public void UpdateMainNumber(Number number)
        {
            string temp = number.Normal.ToString();
            Label_Main.Content = temp;
        }
        public void UpdateMainNumber(string str)
        {
            Label_Main.Content = str;
        }
        public void UpdateScreen(string str = null)
        {
            Label_Main.Content = NumberMain.Normal;
            string tempString = NumberSecond.Normal.ToString();
            if (currentOperation != null) tempString += " " + currentOperation.Display.ToString();
            Label_Process.Content = tempString;
        }
        private void SetCurrentOperation(Operation operation)
        {
            currentOperation = operation;
            if (NumberSecond.Normal == 0)
            {
                NumberSecond.Normal = NumberMain.Normal;
                NumberMain.Normal = 0;
                NumberMain.PointIndex = 0;
            }
            UpdateScreen();
        }
        private void Execute()
        {
            if (currentOperation != null)
            {
                try
                {
                    NumberMain = currentOperation.Compute(NumberSecond, NumberMain);
                    NumberSecond.Normal = NumberMain.Normal;
                    UpdateScreen();
                    NumberMain.PointIndex = 0;
                    NumberMain.Normal = 0;
                }
                catch
                {
                    Label_Main.Content = "ERROR";
                }
            }
        }
        public MainWindow()
        {
            InitializeComponent();
            foreach (Button button in MainButtonGrid.Children)
            {
                button.Style = (Style)FindResource("ButtonStyle_Main");
                button.Margin = new Thickness(4);
            }

            foreach (Button button in MainButtonGrid.Children)
            {
                if (button.Name.Contains("Button_Number"))
                {
                    button.Click += (sender, e) =>
                    {
                        AddDigitToEnd(int.Parse(button.Content.ToString()));
                    };
                }
            }

            Button_Backspace.Click += (sender, e) => { RemoveDigitFromEnd(); };
            Button_CE.Click += (sender, e) => { ClearMain(); };
            Button_C.Click += (sender, e) =>  { ClearAll(); };

            Button_SignInvert.Click += (sender, e) =>
            {
                NumberMain = new OperSignInvert().Compute(NumberMain);
                UpdateMainNumber(NumberMain);
            };
            Button_SquareRoot.Click += (sender, e) =>
            {
                NumberMain = new OperSQRT().Compute(NumberMain);
                UpdateMainNumber(NumberMain);
            };
            Button_OneByX.Click += (sender, e) =>
            {
                NumberMain = new OperOneByX().Compute(NumberMain);
                UpdateMainNumber(NumberMain);
            };

            Button_Plus.Click += (sender, e) => SetCurrentOperation(new OperPlus());
            Button_Minus.Click += (sender, e) => SetCurrentOperation(new OperMinus());
            Button_Multiply.Click += (sender, e) => SetCurrentOperation(new OperMultiply());
            Button_Divide.Click += (sender, e) => SetCurrentOperation(new OperDivide());
            Button_Percent.Click += (sender, e) => SetCurrentOperation(new OperTakePercent());

            Button_Point.Click += (sender, e) => AppendPoint();

            Button_Equals.Click += (sender, e) =>
            {
                Execute();
            };

            Button_MemoryClear.Click += (sender, e) =>
            {
                NumberMemory.Normal = 0;
                NumberMemory.PointIndex = 0;
            };
            Button_MemoryRead.Click += (sender, e) =>
            {
                NumberMain.Normal = NumberMemory.Normal;
                UpdateMainNumber(NumberMain);
            };
            Button_MemorySet.Click += (sender, e) =>
            {
                NumberMemory.Normal = NumberMain.Normal;
            };
            Button_MemoryPlus.Click += (sender, e) =>
            {
                NumberMemory = new OperPlus().Compute(NumberMemory, NumberMain);
            };
            Button_MemoryMinus.Click += (sender, e) =>
            {
                NumberMemory = new OperMinus().Compute(NumberMemory, NumberMain);
            };

            UpdateScreen();
        }

    }
}
