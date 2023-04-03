using Connector_Tier;
using Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bus_Tier
{
    internal class BSReminder
    {
        ConnectorFactory connector = null;

        public BSReminder(ConnectorFactory connector)
        {
            this.connector = connector;
        }
        public List<MyReminder> getListReminderByUserId(int userId, DateTime date)
        {
            List<MyReminder> list = new List<MyReminder>();
            try
            {
                SqlDataReader reader = connector.getListReminderByIdDate(userId, date);
                while (reader.Read())
                {
                    MyReminder reminder = new MyReminder()
                    {
                        reminderId = reader.GetInt32(reader.GetOrdinal("id")),
                        remindTime = reader.GetDateTime(reader.GetOrdinal("remindTime")),
                        userId = reader.GetInt32(reader.GetOrdinal("userId")),
                    };
                    Appointment appointment = new Appointment
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("appointmentId")),
                        StartTime = reader.GetDateTime(reader.GetOrdinal("startTime")),
                        EndTime = reader.GetDateTime(reader.GetOrdinal("endTime")),
                        Title = reader.GetString(reader.GetOrdinal("title")),
                        Description = reader.GetString(reader.GetOrdinal("description")),
                        HostId = reader.GetInt32(reader.GetOrdinal("HostID")),
                        Location = reader.GetString(reader.GetOrdinal("location")),
                    };
                    reminder.appointment = appointment;
                    list.Add(reminder);
                }
                connector.closeConnection();
                reader.Close();
                return list;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void addReminder(MyReminder reminder)
        {
            try
            {
                connector.addReminder(reminder);
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                connector.closeConnection();
            }
        }
    }
}
