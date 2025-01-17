﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CRUD_MVVM.ViewModels
{

    public class BaseViewModel : INotifyPropertyChanged
    {
        // Evento para notificar cambios de propiedad
        public event PropertyChangedEventHandler PropertyChanged;

        // Método para notificar que una propiedad ha cambiado
        protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value)) return false;
            backingStore = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        // Método para disparar el evento PropertyChanged
        protected virtual void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
