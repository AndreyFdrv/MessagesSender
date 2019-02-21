﻿using System;
using System.Windows;
using System.Threading;
using System.IO.Pipes;

namespace MessagesSender.Server
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string Status
        {
            get { return lblStatus.Content.ToString(); }
            set { Dispatcher.Invoke(new Action(() => { lblStatus.Content = value; })); }
        }
        public MainWindow()
        {
            InitializeComponent();
            var server = new Server();
            new Thread(server.Start).Start(this);
        }
    }
}