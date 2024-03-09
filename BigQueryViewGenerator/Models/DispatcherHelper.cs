using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Threading;

namespace BigQueryViewGenerator.Models
{
    public static class DispatcherHelper
    {
        private static Dispatcher _uiDispatcher;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("ReSharper", "InconsistentNaming")]
        public static Dispatcher UIDispatcher
        {
            get
            {
                var metadata = DesignerProperties.IsInDesignModeProperty?.GetMetadata(typeof(DependencyObject));
                if ((bool)(metadata?.DefaultValue ?? default(bool)))
                {
                    _uiDispatcher = Dispatcher.CurrentDispatcher;
                }
                return _uiDispatcher;
            }
            set { _uiDispatcher = value; }
        }

    }
}