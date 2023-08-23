using Adirev.Service;
using Adirev.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Adirev.Model
{
    class ScheduleWindowModel
    {
        #region Properties
        public ObservableCollection<ScheduleItemControl> ScheduleItems { get; set; }
        #endregion

        #region Constructor
        public ScheduleWindowModel()
        {
            ScheduleItems = new ObservableCollection<ScheduleItemControl>();
            LoadScheduleItem();
        }
        #endregion

        #region Public Methods
        public void AddScheduleItem()
        {
            ScheduleItemControl scheduleItemControl = new ScheduleItemControl();
            scheduleItemControl.VisibilityForm = Visibility.Visible;
            ScheduleItems.Add(scheduleItemControl);
        }
        public void Save()
        {
            if (ScheduleItems.Where(x => x.IsEnabled && x.IsValid == false).ToList().Count > 0)
            { MessageBox.Show("Correct incorrect/required data", "Info"); }
            else
            {
                List<ScheduleItem> listScheduleItem = ScheduleItems.Where(x => x.IsEnabled && x.IsValid).Select(scheduleItem => new ScheduleItem(scheduleItem.ServerDatabase.Login, scheduleItem.ServerDatabase.Password)
                {
                    Name = scheduleItem.Name,
                    System = scheduleItem.SystemDatabase,
                    Server = scheduleItem.Server,
                    Database = scheduleItem.EntityDataBaseSelected,
                    Path = scheduleItem.Path,
                    IsCheckedHour = scheduleItem.IsCheckedHour,
                    IsCheckedDay = scheduleItem.IsCheckedDay,
                    IsCheckedWeek = scheduleItem.IsCheckedWeek,
                    Time = scheduleItem.Time,
                    Day = scheduleItem.Day,
                    IsActive = scheduleItem.IsActive,
                    LastExecutionTime = scheduleItem.LastExecutionTime,
                    RealLastExecutionTime = scheduleItem.RealLastExecutionTime
                }).ToList();

                if (ScheduleManager.Save(listScheduleItem))
                { MessageBox.Show("Schedule saved", "Info"); }
                else
                { MessageBox.Show("Schedule not saved", "Error"); }
            }
        }
        #endregion

        #region Private Methods
        private void LoadScheduleItem()
        {
            List<ScheduleItem> list = ScheduleManager.GetScheduleItems();

            foreach (var item in list)
            {
                ScheduleItemControl scheduleItemControl = new ScheduleItemControl();
                scheduleItemControl.Name = item.Name;
                scheduleItemControl.ServerDatabase.Login = item.GetLogin();
                scheduleItemControl.ServerDatabase.Password = item.GetPassword();
                scheduleItemControl.ServerDatabase.System = item.System;
                scheduleItemControl.SystemDatabase = item.System;
                scheduleItemControl.Server = item.Server;
                scheduleItemControl.ServerDatabase.ServerDatabase = item.Server;
                scheduleItemControl.LoadDatabases(null, null);
                scheduleItemControl.EntityDataBaseSelected = item.Database;
                scheduleItemControl.Path = item.Path;
                scheduleItemControl.IsCheckedHour = item.IsCheckedHour;
                scheduleItemControl.IsCheckedDay = item.IsCheckedDay;
                scheduleItemControl.IsCheckedWeek = item.IsCheckedWeek;
                scheduleItemControl.Time = item.Time;
                scheduleItemControl.Day = item.Day;
                scheduleItemControl.IsActive = item.IsActive;
                scheduleItemControl.LastExecutionTime = item.LastExecutionTime;
                scheduleItemControl.RealLastExecutionTime = item.RealLastExecutionTime;

                scheduleItemControl.RefreshTitle();

                ScheduleItems.Add(scheduleItemControl);
            }
        }
        #endregion
    }
}
