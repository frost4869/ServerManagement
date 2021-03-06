﻿using ServerManagement.Model.Entity;
using ServerManagement.ViewModel;
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

namespace ServerManagement.View
{
    /// <summary>
    /// Interaction logic for Account.xaml
    /// </summary>
    public partial class Account : UserControl
    {
        public static Account Instance
        {
            get;
            private set;
        }

        public Account(AuthenticationViewModel viewModel)
        {
            InitializeComponent();
            Instance = this;
            this.DataContext = viewModel;
            accountDataGrid.AddHandler(DataGridRow.MouseDoubleClickEvent,
                                        new MouseButtonEventHandler(DataGridRow_MouseDoubleClick), true);
        }

        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (((DataGrid)sender).SelectedCells.Count > 0)
            {
                User user = ((DataGrid)sender).SelectedCells[0].Item as User;
                accountUpdateView.DataContext = new AccountViewModel(user);
            }
        }
    }
}
