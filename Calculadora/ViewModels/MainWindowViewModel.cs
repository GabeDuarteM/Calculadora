using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using Calculadora.Views;
using Prism.Commands;
using Prism.Mvvm;

namespace Calculadora.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _sVisor;

        public string sVisor
        {
            get { return _sVisor; }
            set { SetProperty(ref _sVisor, value); }
        }

        private string _sVisorHistorico;

        public string sVisorHistorico
        {
            get { return _sVisorHistorico; }
            set { SetProperty(ref _sVisorHistorico, value); }
        }

        public string sSeparadorDecimal { get { return CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator; } }

        public ICommand CommandNumero { get; set; }

        public ICommand CommandRegraDeTres { get; set; }

        public MainWindowViewModel()
        {
            sVisorHistorico = "";
            sVisor = "0";
            CommandNumero = new DelegateCommand<string>(AdicionarStringNoVisor);
            CommandRegraDeTres = new DelegateCommand(new Action(() => new RegraDeTres().Show()));
        }

        private void AdicionarStringNoVisor(string sString)
        {
            if (Regex.IsMatch(sString, $@"[\d{CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator}]"))
            {
                sVisor += sString;

                sVisor = string.Format("{0:0}", sVisor);
            }
        }
    }
}