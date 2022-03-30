#region

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Auditor.Models;
using Dapper;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

#endregion

namespace Auditor.ViewModel
{
    /// <summary>
    ///     This class contains properties that the main View can data bind to.
    ///     <para>
    ///         Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    ///     </para>
    ///     <para>
    ///         You can also use Blend to data bind with the tool's support.
    ///     </para>
    ///     <para>
    ///         See http://www.galasoft.ch/mvvm
    ///     </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private readonly DataRepository repository = new DataRepository();
        private WindowEditActivity window;

        public MainViewModel()
        {
            FilterText = "ToolName='Microsoft Visual Studio 2013'";
        }

        public List<Activity> Activities { get; set; }

        public String FilterText { get; set; }

        public List<Activity> SelectedActivities { get; set; }

        /// <summary>
        /// First activity of selected activities
        /// </summary>
        public Activity SelectedActivity { get; set; }

        public ICommand ExecuteFilter
        {
            get { return new RelayCommand(OnFilter, () => !String.IsNullOrEmpty(FilterText)); }
        }

        public ICommand ExecuteEditActivity
        {
            get { return new RelayCommand(OnEditActivity, () => SelectedActivities != null && SelectedActivities.Count > 0); }
        }

        public ICommand ExecuteFatchLogs
        {
            get { return new RelayCommand(OnFatchLogs); }
        }

        public ICommand ExecuteEditActivitySave
        {
            get { return new RelayCommand(OnEditActivitySave); }
        }

        public ICommand ExecuteFillVS2013Default
        {
            get { return new RelayCommand(OnFillVS2013Default); }
        }

        public ICommand SelectionChangedCommand
        {
            get { return new RelayCommand<IList>(OnDataGridSelectionChanged); }
        }

        public ICommand SaveCurrentSettingsCommand
        {
            get { return new RelayCommand(OnSaveCurrentSettingsCommand); }
        }

        

        #region Private Methods

        private void OnFatchLogs()
        {
            Activities = repository.GetActivities();
            RaisePropertyChanged(() => Activities);
        }

        private void OnFilter()
        {
            Activities = repository.GetActivities(FilterText);
            RaisePropertyChanged(() => Activities);
        }

        private void OnEditActivity()
        {
            SelectedActivity = SelectedActivities[0];
            RaisePropertyChanged(() => SelectedActivity);

            window = new WindowEditActivity();
            window.ShowDialog();
        }

        private void OnEditActivitySave()
        {
            //Change all activities to same as first activity
            foreach (Activity selectedActivity in SelectedActivities)
            {
                selectedActivity.ActionType = SelectedActivity.ActionType;
                selectedActivity.ObjectName = SelectedActivity.ObjectName;
                selectedActivity.ToolName = SelectedActivity.ToolName;
                selectedActivity.ProjectID = SelectedActivity.ProjectID;
                selectedActivity.OldActionType = SelectedActivity.OldActionType;
                selectedActivity.Version = SelectedActivity.Version;
                selectedActivity.ExeName = SelectedActivity.ExeName;
                selectedActivity.ArtifactType = SelectedActivity.ArtifactType;
                selectedActivity.AppID = SelectedActivity.AppID;
                selectedActivity.ObjID = SelectedActivity.ObjID;

                repository.SaveActivity(selectedActivity);    
            }
            window.Close();
        }

        private void OnDataGridSelectionChanged(IList SelectedItems)
        {
            SelectedActivities = new List<Activity>(SelectedItems.Count);
            foreach (Activity activity in SelectedItems)
            {
                SelectedActivities.Add(activity);
            }

        }

        private void OnFillVS2013Default()
        {
            //SelectedActivity.ActionType = 3004;
            //SelectedActivity.ObjectName = "Works.NET";
            //SelectedActivity.ToolName = "Microsoft Visual Studio 2013";
            //SelectedActivity.ProjectID = 611;
            //SelectedActivity.OldActionType = 2005;
            //SelectedActivity.Version = "12.0.30501.0";
            //SelectedActivity.ExeName = "DEVENV.EXE";
            //SelectedActivity.ArtifactType = 2;
            //SelectedActivity.AppID = 50894;
            //SelectedActivity.ObjID = 1926;
            SelectedActivity.ActionType = Properties.Settings.Default.ActionType;
            SelectedActivity.ObjectName = Properties.Settings.Default.ObjectName;
            SelectedActivity.ToolName = Properties.Settings.Default.ToolName;
            SelectedActivity.ProjectID = Properties.Settings.Default.ProjectID;
            SelectedActivity.OldActionType = Properties.Settings.Default.OldActionType;
            SelectedActivity.Version = Properties.Settings.Default.Version;
            SelectedActivity.ExeName = Properties.Settings.Default.ExeName;
            SelectedActivity.ArtifactType = Properties.Settings.Default.ArtifactType;
            SelectedActivity.AppID = Properties.Settings.Default.AppID;
            SelectedActivity.ObjID = Properties.Settings.Default.ObjID;
            RaisePropertyChanged(() => this.SelectedActivity);

        }

        private void OnSaveCurrentSettingsCommand()
        {
            if (SelectedActivities != null)
            {
                Properties.Settings.Default.ActionType = SelectedActivity.ActionType;
                Properties.Settings.Default.ObjectName = SelectedActivity.ObjectName;
                Properties.Settings.Default.ToolName = SelectedActivity.ToolName;
                Properties.Settings.Default.ProjectID = SelectedActivity.ProjectID;
                Properties.Settings.Default.OldActionType = SelectedActivity.OldActionType;
                Properties.Settings.Default.Version = SelectedActivity.Version;
                Properties.Settings.Default.ExeName = SelectedActivity.ExeName;
                Properties.Settings.Default.ArtifactType = SelectedActivity.ArtifactType;
                Properties.Settings.Default.AppID = SelectedActivity.AppID;
                Properties.Settings.Default.ObjID = SelectedActivity.ObjID;
                Properties.Settings.Default.Save();
                MessageBox.Show("Setting saved");
            }
        }

        #endregion
    }
}