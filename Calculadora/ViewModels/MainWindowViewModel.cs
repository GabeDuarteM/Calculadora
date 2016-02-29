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
        private decimal _dValor1;

        public decimal dValor1
        {
            get { return _dValor1; }
            set { SetProperty(ref _dValor1, value); }
        }

        private decimal _dValor2;

        public decimal dValor2
        {
            get { return _dValor2; }
            set { SetProperty(ref _dValor2, value); }
        }

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

        public DelegateCommand<string> CommandNumero { get; private set; }

        public DelegateCommand CommandRegraDeTres { get; private set; }

        public DelegateCommand CommandTrocarSinal { get; private set; }

        public DelegateCommand CommandC { get; private set; }

        public DelegateCommand CommandCE { get; private set; }

        public DelegateCommand CommandBackspace { get; private set; }

        public DelegateCommand CommandDivisao { get; private set; }

        public DelegateCommand CommandRaiz { get; private set; }

        public DelegateCommand CommandMultiplicacao { get; private set; }

        public DelegateCommand CommandPorcentagem { get; private set; }

        public DelegateCommand CommandSubtracao { get; private set; }

        public DelegateCommand CommandSoma { get; private set; }

        public DelegateCommand CommandIgual { get; private set; }

        public MainWindowViewModel()
        {
            BotaoC();
            CommandNumero = new DelegateCommand<string>(AdicionarStringNoVisor);
            CommandRegraDeTres = new DelegateCommand(new Action(() => new RegraDeTres().Show()));
            CommandTrocarSinal = new DelegateCommand(BotaoTrocarSinal);
            CommandC = new DelegateCommand(BotaoC);
            CommandCE = new DelegateCommand(BotaoCE);
            CommandBackspace = new DelegateCommand(BotaoBackspace);
            CommandDivisao = new DelegateCommand(BotaoDivisao);
            CommandRaiz = new DelegateCommand(BotaoRaiz);
            CommandMultiplicacao = new DelegateCommand(BotaoMultiplicacao);
            CommandPorcentagem = new DelegateCommand(BotaoPorcentagem);
            CommandSubtracao = new DelegateCommand(BotaoSubtracao);
            CommandSoma = new DelegateCommand(BotaoSoma);
            CommandIgual = new DelegateCommand(BotaoIgual);
        }

        private void BotaoIgual()
        {
            throw new NotImplementedException();
        }

        private void BotaoSoma()
        {
            throw new NotImplementedException();
        }

        private void BotaoSubtracao()
        {
            throw new NotImplementedException();
        }

        private void BotaoPorcentagem()
        {
            throw new NotImplementedException();
        }

        private void BotaoMultiplicacao()
        {
            throw new NotImplementedException();
        }

        private void BotaoRaiz()
        {
            throw new NotImplementedException();
        }

        private void BotaoDivisao()
        {
            throw new NotImplementedException();
        }

        private void BotaoBackspace()
        {
            if (sVisor.EndsWith(sSeparadorDecimal))
            {
                AlterarStringNoVisor(dValor1, false);
                return;
            }

            string sValor1 = dValor1.ToString();
            sValor1 = sValor1.Remove(sValor1.Length - 1);

            if (sValor1 != "" && sValor1 != CultureInfo.CurrentCulture.NumberFormat.NegativeSign)
            {
                decimal dValor;
                if (decimal.TryParse(sValor1, out dValor))
                {
                    _dValor1 = dValor; // BUG Pra funfar com o Prism. Se não colocar isso quando a string for um zero e antes disso existir um separador decimal (Ex: 10,30, sendo o último zero o sString) o valor setado pelo prism continua como o anterior (no caso, 10,3)
                    dValor1 = dValor;
                    AlterarStringNoVisor(dValor1, sValor1.EndsWith(sSeparadorDecimal));
                }
            }
            else
            {
                BotaoCE();
            }
        }

        private void BotaoCE()
        {
            sVisor = "0";
            dValor1 = 0;
        }

        private void BotaoC()
        {
            BotaoCE();
            sVisorHistorico = "";
            dValor2 = 0;
        }

        private void AdicionarStringNoVisor(string sString)
        {
            decimal dValor;

            if (decimal.TryParse(sVisor + sString, out dValor))
            {
                _dValor1 = dValor; // BUG Pra funfar com o Prism. Se não colocar isso quando a string for um zero e antes disso existir um separador decimal (Ex: 10,30, sendo o último zero o sString) o valor setado pelo prism continua como o anterior (no caso, 10,3)
                dValor1 = dValor;
                AlterarStringNoVisor(dValor1, sString.EndsWith(sSeparadorDecimal));
            }
        }

        private void AlterarStringNoVisor(decimal dNumero, bool bTerminaComSeparadorDecimal)
        {
            int nCasasDecimais = BitConverter.GetBytes(decimal.GetBits(dNumero)[3])[2]; // Pega a quantidade de casas decimais do número para informar no formato abaixo.
            sVisor = string.Format($"{{0:N{nCasasDecimais}}}", dNumero);

            if (bTerminaComSeparadorDecimal)
            {
                sVisor += sSeparadorDecimal;
            }
        }

        private void BotaoTrocarSinal()
        {
            // TODO Usar metodo de multiplicação.
            dValor1 = dValor1 * -1;
            AlterarStringNoVisor(dValor1, sVisor.EndsWith(sSeparadorDecimal));
        }
    }
}