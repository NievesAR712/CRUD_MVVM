﻿using CRUD_MVVM.PresentationLayer.ViewModels;

namespace CRUD_MVVM
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage(MainViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
