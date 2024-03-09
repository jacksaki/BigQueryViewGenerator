using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.Diagnostics;
using System.ComponentModel.DataAnnotations;
using BigQueryViewGenerator.ViewModels;
using System.Text.Json;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using R3;
using System.ComponentModel;

namespace BigQueryViewGenerator.Models
{
    public static class Extensions
    {
        public static Observable<PropertyChangedEventArgs> ObservePropertyChanged(this INotifyPropertyChanged self, CancellationToken cancellationToken = default)
            => Observable.FromEvent<PropertyChangedEventHandler, PropertyChangedEventArgs>(static h => (s, e) => h(e), h => self.PropertyChanged += h, h => self.PropertyChanged -= h, cancellationToken);
        public static bool Between(this int value,int from,int to)
        {
            return value >= from && value <= to;
        }
        public static int GetCharCount(this string value, char searchChar)
        {
            if (string.IsNullOrEmpty(value))
            {
                return 0;
            }
            return value.Where(x => x == searchChar).Count();
        }
    }
}