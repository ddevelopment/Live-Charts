﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using LiveCharts;
using LiveCharts.Annotations;

namespace ChartsTest.Line_Examples
{
    /// <summary>
    /// Interaction logic for LazyDataLine.xaml
    /// </summary>
    public partial class LazyDataLine
    {
        public LazyDataLine()
        {
            InitializeComponent();
            Models = new ObservableCollection<TestViewModel>();
            DataContext = this;

            Models.Add(new TestViewModel());
            Models.Add(new TestViewModel());
        }

        public ObservableCollection<TestViewModel> Models { get; set; }

        private void AddElementOnClick(object sender, RoutedEventArgs e)
        {
            Models.Add(new TestViewModel());
        }

        private void GetDataOnClick(object sender, RoutedEventArgs e)
        {
            if (sender == null) return;
            var fe = sender as FrameworkElement;
            if (fe == null) return;
            var model = fe.DataContext as TestViewModel;
            if (model == null) return;
            model.GetNewData();
        }

        private void AddSeriesOnClick(object sender, RoutedEventArgs e)
        {
            if (sender == null) return;
            var fe = sender as FrameworkElement;
            if (fe == null) return;
            var model = fe.DataContext as TestViewModel;
            if (model == null) return;
            model.AddSeries();
        }

        private void RemoveLastSeriesOnClick(object sender, RoutedEventArgs e)
        {
            if (sender == null) return;
            var fe = sender as FrameworkElement;
            if (fe == null) return;
            var model = fe.DataContext as TestViewModel;
            if (model == null) return;
            model.RemoveSeries();
        }
    }


    public class TestViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Series> _series;
        private string _name;
        private static Random _random = new Random();

        public TestViewModel()
        {
            Labels = new List<string>();
            Series = new ObservableCollection<Series>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        //Charts Update When you add/remove a serie from Series collection or when you add/remove a value inside each serie.
        public ObservableCollection<Series> Series
        {
            get { return _series; }
            set
            {
                _series = value;
                OnPropertyChanged();
            }
        }

        //labels do not fire any update, it is not necesary to use Observable collection.
        public List<string> Labels { get; set; }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public void GetNewData()
        {
            Name = GetRandomName();
            Labels.Clear();
            var randomStartDay = _random.Next(1, 10);
            for (int i = 0; i < 10; i++) Labels.Add("Day " + (randomStartDay +i));
            foreach (var series in Series.ToArray()) Series.Remove(series);
            Series.Add(new LineSeries
            {
                Title = "Serie 1",
                PrimaryValues = BuildRandomValues(),
                PointRadius = 0
            });
            Series.Add(new LineSeries
            {
                Title = "Serie 2",
                PrimaryValues = BuildRandomValues(),
                PointRadius = 0
            });
        }

        public void AddSeries()
        {
            Series.Add(new LineSeries
            {
                Title = "Series " + Series.Count + 1,
                PrimaryValues = BuildRandomValues(),
                PointRadius = 0
            });
        }

        public void RemoveSeries()
        {
            if (Series.Count < 1) return;
            Series.RemoveAt(0);
        }

        private static ObservableCollection<double> BuildRandomValues()
        {
            var serie = new ObservableCollection<double>();
            for (int i = 0; i < 10; i++) serie.Add(_random.Next(-10,10));
            return serie;
        }

        private static string GetRandomName()
        {
            var names = new[] {"Pablo", "Vicent", "Andy", "Salvador", "Leonardo", "Jackson", "Claude", "Miguel", "Henri", "Frida", "Donatello" };
            return names[_random.Next(0, names.Length - 1)];
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}