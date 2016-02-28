using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;

namespace Calculadora.ViewModels
{
    public class RegraDeTresViewModel : ViewModelBase
    {
        private string _sValor1;

        [Required(ErrorMessage = "Preencha este campo."), RegularExpression(@"^((?:\d{1,3}[,])*(?:\d+[.])?\d+)$|^((?:\d{1,3}[.])*(?:\d+[,])?\d+)$", ErrorMessage = "Informe um número válido.")]
        public string sValor1
        {
            get { return _sValor1; }
            set { SetProperty(ref _sValor1, value); }
        }

        private string _sValor2;

        [Required(ErrorMessage = "Preencha este campo."), RegularExpression(@"^((?:\d{1,3}[,])*(?:\d+[.])?\d+)$|^((?:\d{1,3}[.])*(?:\d+[,])?\d+)$", ErrorMessage = "Informe um número válido.")]
        public string sValor2
        {
            get { return _sValor2; }
            set { SetProperty(ref _sValor2, value); }
        }

        private string _sValor3;

        [Required(ErrorMessage = "Preencha este campo."), RegularExpression(@"^((?:\d{1,3}[,])*(?:\d+[.])?\d+)$|^((?:\d{1,3}[.])*(?:\d+[,])?\d+)$", ErrorMessage = "Informe um número válido.")]
        public string sValor3
        {
            get { return _sValor3; }
            set { SetProperty(ref _sValor3, value); }
        }

        private string _sResultado;

        public string sResultado
        {
            get { return _sResultado; }
            set { SetProperty(ref _sResultado, value); }
        }

        public DelegateCommand CommandCalcular { get; private set; }

        public DelegateCommand CommandLimpar { get; private set; }

        public RegraDeTresViewModel()
        {
            Limpar();
            CommandCalcular = new DelegateCommand(Calcular);
            CommandLimpar = new DelegateCommand(Limpar);
            IsValidaErrosNoPropertyChanged = true;
        }

        private void Calcular()
        {
            if (ValidarObjeto())
            {
                decimal dValor1 = 0;
                decimal dValor2 = 0;
                decimal dValor3 = 0;
                decimal dResultado = 0;

                decimal.TryParse(sValor1, out dValor1);
                decimal.TryParse(sValor2, out dValor2);
                decimal.TryParse(sValor3, out dValor3);

                try
                {
                    dResultado = dValor2 * dValor3;
                    dResultado = dResultado / dValor1;
                    sResultado = dResultado.ToString();
                }
                catch
                {
                    sResultado = "NaN";
                }
            }
        }

        private void Limpar()
        {
            sValor1 = "0";
            sValor2 = "0";
            sValor3 = "0";
            sResultado = "X";
            ValidarObjeto();
        }
    }
}