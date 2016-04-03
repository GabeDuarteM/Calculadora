using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using Prism.Commands;
using Prism.Mvvm;

namespace Calculadora.ViewModels
{
    public class ViewModelBase : BindableBase, INotifyDataErrorInfo
    {
        public ViewModelBase()
        {
            IsValidaErrosNoPropertyChanged = false;
        }

        protected override bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            var retorno = base.SetProperty<T>(ref storage, value, propertyName);

            if (IsValidaErrosNoPropertyChanged)
            {
                ValidarPropriedade(propertyName, value);
            }
            // comenta
            return retorno;
        }

        #region INotifyDataErrorInfo

        public bool IsValidaErrosNoPropertyChanged { get; set; }

        private readonly IDictionary<string, IList<string>> _errors = new Dictionary<string, IList<string>>();

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        protected void OnErrorsChanged(string propertyName)
        {
            var handled = ErrorsChanged;
            if (handled != null)
                handled(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public IEnumerable GetErrors(string propertyName)
        {
            if (_errors.ContainsKey(propertyName))
            {
                IList<string> propertyErrors = _errors[propertyName];
                foreach (string propertyError in propertyErrors)
                {
                    yield return propertyError;
                }
            }
            yield break;
        }

        public bool HasErrors
        {
            get { return _errors.Count > 0; }
        }

        protected void ValidarPropriedade(string propertyName, object value)
        {
            ViewModelBase objectToValidate = this;
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            bool isValid = Validator.TryValidateProperty(
                value,
                new ValidationContext(objectToValidate, null, null)
                {
                    MemberName = propertyName
                },
                results);

            if (isValid)
                RemoveErrorsForProperty(propertyName);
            else
                AddErrorsForProperty(propertyName, results);

            OnErrorsChanged(propertyName);
        }

        private void AddErrorsForProperty(string propertyName, IEnumerable<System.ComponentModel.DataAnnotations.ValidationResult> validationResults)
        {
            RemoveErrorsForProperty(propertyName);
            _errors.Add(propertyName, validationResults.Select(vr => vr.ErrorMessage).ToList());
        }

        private void RemoveErrorsForProperty(string propertyName)
        {
            if (_errors.ContainsKey(propertyName))
                _errors.Remove(propertyName);
        }

        public bool ValidarObjeto()
        {
            ViewModelBase objectToValidate = this;
            _errors.Clear();
            Type objectType = objectToValidate.GetType();
            PropertyInfo[] properties = objectType.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                if (property.GetCustomAttributes(typeof(ValidationAttribute), true).Any())
                {
                    object value = property.GetValue(objectToValidate, null);
                    ValidarPropriedade(property.Name, value);
                }
            }

            return !HasErrors;
        }

        #endregion INotifyDataErrorInfo
    }
}