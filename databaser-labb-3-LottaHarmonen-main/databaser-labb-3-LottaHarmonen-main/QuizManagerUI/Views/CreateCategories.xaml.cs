﻿using DataAccess.Services;
using QuizManagerUI.ViewModels;
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

namespace QuizManagerUI.Views
{
    /// <summary>
    /// Interaction logic for CreateCategories.xaml
    /// </summary>
    public partial class CreateCategories : UserControl
    {
        public CreateCategories()
        {
            InitializeComponent();
            DataContext = new CreateCategoriesViewViewModel(new MongoDbService());
        }
    }
}
