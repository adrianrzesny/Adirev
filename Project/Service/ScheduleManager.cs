using Adirev.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Adirev.Service
{
    class ScheduleManager
    {
        #region Objects
        private Timer scheduleManagerTimer;
        #endregion

        #region Properties

        #endregion

        #region Constructor
        public ScheduleManager()
        {
            scheduleManagerTimer = new Timer(ScheduleManagerTask, null, TimeSpan.Zero, TimeSpan.FromSeconds(15));
        }
        #endregion

        #region Public Methods
        public static bool Save(List<ScheduleItem> listScheduleItem)
        {
            bool result = true;
            try
            {
                string path = @$"{ApplicationGlobalSettings.PathSettingsApplication}\Schedules.{ApplicationGlobalSettings.Extension}";
                FileManager.WriteToBinaryFile<List<ScheduleItem>>(path, listScheduleItem);
            }
            catch (Exception ex)
            {
                Logger.SaveError(ex.Message, ex.InnerException?.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName + " -> " + System.Reflection.MethodBase.GetCurrentMethod());
                result = false;
            }

            return result;
        }

        public static List<ScheduleItem> GetScheduleItems()
        {
            string path = @$"{ApplicationGlobalSettings.PathSettingsApplication}\Schedules.{ApplicationGlobalSettings.Extension}";
            List<ScheduleItem> list = FileManager.ReadFromBinaryFile<List<ScheduleItem>>(path);

            return list;
        }

        #endregion

        #region Private Methods
        private void ScheduleManagerTask(Object state)
        {
            List<ScheduleItem> list = GetScheduleItems();

            foreach (var item in list.Where(x => x.IsActive))
            {
                DateTime curentTime = DateTime.Now;

                int scheduleHour = DateTime.Parse($" {item.Day} {item.Time}").Hour;
                int scheduleMinute = DateTime.Parse($" {item.Day} {item.Time}").Minute;
                int scheduleDayOfWeek = (int)DateTime.Parse($" {item.Day} {item.Time}").DayOfWeek;
                int scheduleHourCurrent = curentTime.Hour;
                int scheduleMinuteCurrent = curentTime.Minute;
                int scheduleDayOfWeekCurrent = (int)curentTime.DayOfWeek;

                if (item.IsCheckedHour && (((curentTime - item.LastExecutionTime).TotalHours >= 1) || (scheduleHour == scheduleHourCurrent && scheduleMinute == scheduleMinuteCurrent)))
                { ExecuteScheduleItemPlan(item); }
                else if (item.IsCheckedDay && (((curentTime - item.LastExecutionTime).TotalHours >= 24) || (scheduleHour == scheduleHourCurrent && scheduleMinute == scheduleMinuteCurrent)))
                { ExecuteScheduleItemPlan(item); }
                else if (item.IsCheckedWeek && (((curentTime - item.LastExecutionTime).TotalHours >= 168) || (scheduleHour == scheduleHourCurrent && scheduleMinute == scheduleMinuteCurrent && scheduleDayOfWeek == scheduleDayOfWeekCurrent)))
                { ExecuteScheduleItemPlan(item); }
            }

            Save(list);
        }

        private void ExecuteScheduleItemPlan(ScheduleItem scheduleItem)
        {
            if (SaveScriptsDatabase(scheduleItem))
            {
                DateTime curentTime = DateTime.Now;
                scheduleItem.LastExecutionTime = new DateTime(curentTime.Year, curentTime.Month, curentTime.Day, curentTime.Hour, DateTime.Parse($" {scheduleItem.Day} {scheduleItem.Time}").Minute, 0);
                scheduleItem.RealLastExecutionTime = curentTime;
            }
            else
            { scheduleItem.LastExecutionTime = scheduleItem.LastExecutionTime.AddHours(1); }

        }

        private bool SaveScriptsDatabase(ScheduleItem scheduleItem)
        {
            bool resultSave = true;

            if (ServerManager.Ping(scheduleItem.Server))
            {
                resultSave = SaveScript(scheduleItem, DatabaseManager.TypeScript.FN) == DatabaseManager.Status.LOGIN_FAIlED ? false : resultSave;
                resultSave = SaveScript(scheduleItem, DatabaseManager.TypeScript.P) == DatabaseManager.Status.LOGIN_FAIlED ? false : resultSave;
                resultSave = SaveScript(scheduleItem, DatabaseManager.TypeScript.U) == DatabaseManager.Status.LOGIN_FAIlED ? false : resultSave;
                resultSave = SaveScript(scheduleItem, DatabaseManager.TypeScript.TR) == DatabaseManager.Status.LOGIN_FAIlED ? false : resultSave;
                resultSave = SaveScript(scheduleItem, DatabaseManager.TypeScript.V) == DatabaseManager.Status.LOGIN_FAIlED ? false : resultSave;
            }
            else
            { resultSave = false; }

            return resultSave;
        }

        private DatabaseManager.Status SaveScript(ScheduleItem scheduleItem, DatabaseManager.TypeScript type)
        {
            DatabaseManager db = new DatabaseManager() { System = scheduleItem.System, Server = scheduleItem.Server, DatabaseEntity = scheduleItem.Database, Login = scheduleItem.GetLogin(), Password = scheduleItem.GetPassword() };
            List<DatabaseItem> list = db.GetItemsContents(type, DatabaseManager.OpcionExport.ALL, null);

            string path = scheduleItem.Path;
            path += $@"\{FileManager.DeleteInvalidFileNameChars(db.DatabaseEntity)}";
            FileManager.CreateDirectory(scheduleItem.Path, FileManager.DeleteInvalidFileNameChars(db.DatabaseEntity));
            FileManager.CreateDirectory(path, DatabaseManager.GetNameTypeScript(type));

            foreach (var item in list)
            { FileManager.SaveFileSQL(path, item.Name, item.Contents, type); }

            return db.StatusServer;
        }
        #endregion
    }
}
