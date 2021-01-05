using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IntegerCalcVirtuplex
{
    public partial class MainWindow : Window
    {
        private string expressionString;
        private Label calculationLabel;

        public MainWindow()
        {
            InitializeComponent();
            calculationLabel = this.FindName("CalculationLabel") as Label;
        }

        private void buttonDefault_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            expressionString += btn.Content;

            UpdateLabel();
        }

        private void buttonDelete_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(expressionString))
                return;

            expressionString = expressionString.Remove(expressionString.Length - 1);
            UpdateLabel();
        }

        private void buttonEquals_Click(object sender, RoutedEventArgs e)
        {
            expressionString = Calculator.CalculateFromString(expressionString).Result;
            UpdateLabel();
        }

        private async void buttonImport_Click(object sender, RoutedEventArgs e)
        {

            string inputFilepath = RequestOpenFileDialog();
            string outputFilepath = RequestSaveFileDialog();

            if (string.IsNullOrEmpty(inputFilepath) || string.IsNullOrEmpty(outputFilepath))
                return;

            if (inputFilepath == outputFilepath)
            {
                MessageBox.Show("Input and output should be two different files");
                return;
            }

            var btn = sender as Button;
            btn.IsEnabled = false;

            var progressBar = this.FindName("progressBarImport") as ProgressBar;
            progressBar.IsIndeterminate = true;

            await Task.Run(() => Calculator.CalculateFromFile(inputFilepath, outputFilepath));

            btn.IsEnabled = true;
            progressBar.IsIndeterminate = false;
            MessageBox.Show("Calculation from file completed");

        }

        private string RequestOpenFileDialog()
        {
            OpenFileDialog inputFileDialog = new OpenFileDialog();
            inputFileDialog.Title = "Input File";
            inputFileDialog.Filter = "Text Files | *.txt";
            inputFileDialog.DefaultExt = "txt";
            if (inputFileDialog.ShowDialog() == true)
            {
                return inputFileDialog.FileName;
            }

            return string.Empty;
        }

        private string RequestSaveFileDialog()
        {
            SaveFileDialog outputFileDialog = new SaveFileDialog();
            outputFileDialog.Title = "Output File";
            outputFileDialog.Filter = "Text Files | *.txt";
            outputFileDialog.DefaultExt = "txt";
            if (outputFileDialog.ShowDialog() == true)
            {
                return outputFileDialog.FileName;
            }

            return string.Empty;
        }

        private void UpdateLabel()
        {
            calculationLabel.Content = expressionString;
        }
    }
}
