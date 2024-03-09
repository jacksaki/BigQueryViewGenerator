using R3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigQueryViewGenerator.ViewModels
{
    public class ExceptionWindowViewModel:ViewModelBase
    {
        public void Initialize()
        {
        }

        public Exception Exception { get; }
        public bool CanContinue { get; }

        public ReactiveCommand<Unit> QuitCommand { get; }
        public ReactiveCommand<Unit> ContinueCommand { get; }

        public BindableReactiveProperty<bool?> DialogResult { get; }

        public ExceptionWindowViewModel(Exception ex, bool canContinue)
        {
            this.Exception = ex;
            this.CanContinue = canContinue;
            this.DialogResult = new BindableReactiveProperty<bool?>();
            this.QuitCommand = new ReactiveCommand<Unit>();
            this.QuitCommand.Subscribe(_ => this.DialogResult.Value = false);
            this.ContinueCommand = new ReactiveCommand<Unit>();
            this.ContinueCommand.Subscribe(_ => this.DialogResult.Value = true);
        }

        public string ErrorText
        {
            get
            {
                if (this.Exception is AggregateException)
                {
                    return string.Join("\r\n\r\n", ((AggregateException)this.Exception).InnerExceptions.Select(x => $"{x.Message}\r\n\r\n{x.StackTrace}"));
                }
                else
                {
                    return $"{this.Exception.Message}\r\n\r\n{this.Exception.StackTrace}";
                }
            }
        }
    }
}
