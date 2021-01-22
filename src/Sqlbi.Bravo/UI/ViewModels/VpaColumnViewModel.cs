﻿using Dax.ViewModel;
using Sqlbi.Bravo.UI.Framework.ViewModels;

namespace Sqlbi.Bravo.UI.ViewModels
{
    internal class VpaColumnViewModel : BaseViewModel
    {
        private readonly AnalyzeModelViewModel parent;
        private bool _isSelected;

        // Coupling this VM to the parent is the simplest way to track totals based on selection
        public VpaColumnViewModel(AnalyzeModelViewModel parent) => this.parent = parent;

        public VpaColumnViewModel(AnalyzeModelViewModel parent, VpaColumn vpaColumn)
            : this(parent)
        {
            // TODO REQUIREMENTS: change this to use .IsReferenced (or similar) once available.
            // Currently using IsHidden as a proxy so rest of functionality can be implemented.
            IsRequired = vpaColumn.IsHidden;
            ColumnName = vpaColumn.ColumnName;
            TableName = vpaColumn.Table.TableName;
            ColumnCardinality = vpaColumn.ColumnCardinality;
            TotalSize = vpaColumn.TotalSize;
            PercentageDatabase = vpaColumn.PercentageDatabase;
        }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (SetProperty(ref _isSelected, value)){
                    parent.OnPropertyChanged(nameof(AnalyzeModelViewModel.SelectedColumnCount));
                }
            }
        }

        public bool IsRequired { get; set; }

        public string ColumnName { get; set; }

        public string TableName { get; set; }

        public long ColumnCardinality { get; set; }

        public long TotalSize { get; set; }

        public double PercentageDatabase { get; set; }
    }
}
