using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connector_Tier
{
    public class ConnectorFactory
    {
        private string strConn = @"SERVER=LAPTOP-1JU8QPGU; Database= OOAD;User id ='kid'; pwd ='365471'";
        private SqlCommand cmd = null;
        private SqlDataReader reader = null;
        private SqlConnection conn = null;
        public ConnectorFactory()
        {
            conn = new SqlConnection(strConn);
        }

        public void openConnection()
        {
            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public void closeConnection()
        {
            try
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public SqlDataReader getData(string table)
        {
            try
            {
                string sql = "select * from " + table;
                cmd = new SqlCommand(sql);
                cmd.Connection = conn;
                this.openConnection();
                reader = cmd.ExecuteReader();

            }
            catch (Exception e)
            {
                throw e;
            }
            return reader;
        }
        public SqlDataReader getListById(string proc,string param,int id)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand(proc, conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue(param, id);
                    cmd.Connection=conn;
                    this.openConnection();
                    reader= cmd.ExecuteReader();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return reader;
        }
        public SqlDataReader getListAppByIdDate(int id, DateTime date)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("getAppByUserAndDate", conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Userid", id);
                    cmd.Parameters.AddWithValue("@Appdate", date);
                    cmd.Connection = conn;
                    this.openConnection();
                    reader = cmd.ExecuteReader();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return reader;
        }
        public SqlDataReader getListReminderByIdDate(int id, DateTime date)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("getReminderByUserIdAndDate", conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Userid", id);
                    cmd.Parameters.AddWithValue("@date", date);
                    cmd.Connection = conn;
                    this.openConnection();
                    reader = cmd.ExecuteReader();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return reader;
        }
        public int addAppointment (Appointment appointment)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("AddAppointment", conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@StartTime", appointment.StartTime);
                    cmd.Parameters.AddWithValue("@EndTime", appointment.EndTime);
                    cmd.Parameters.AddWithValue("@title", appointment.Title);
                    cmd.Parameters.AddWithValue("@Description", appointment.Description);
                    cmd.Parameters.AddWithValue("@HostID", appointment.HostId);
                    cmd.Parameters.AddWithValue("@location", appointment.Location);
                    SqlParameter appointmentIdParam = new SqlParameter("@appointmentId", SqlDbType.Int);
                    appointmentIdParam.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(appointmentIdParam);
                    this.openConnection();
                    cmd.ExecuteNonQuery();
                    int appointmentId = (int)appointmentIdParam.Value;
                    return appointmentId;
                }
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        public void UserToAppointment(string proc,int userId, int appointmentId)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand(proc, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@appointmentId",appointmentId);
                    cmd.Parameters.AddWithValue("@userId", userId);
                    cmd.Connection = conn;
                    this.openConnection();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public void updateStatus(int userId, int appointmentId, Boolean status)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("updateStatus", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@userId", userId);
                    cmd.Parameters.AddWithValue("@appointmentId", appointmentId);
                    cmd.Parameters.AddWithValue("@status", status);
                    cmd.Connection = conn;
                    this.openConnection();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public SqlDataReader getListAppByIdDateStatus(int id, DateTime date, bool status)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("getListAppByUserAndDateAndStatus", conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Userid", id);
                    cmd.Parameters.AddWithValue("@status", status);
                    cmd.Parameters.AddWithValue("@Appdate", date);
                    cmd.Connection = conn;
                    this.openConnection();
                    reader = cmd.ExecuteReader();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return reader;
        }

        public void updateAppointment(Appointment appointment)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("updateAppointment", conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@appointmentId", appointment.Id);
                    cmd.Parameters.AddWithValue("@StartTime", appointment.StartTime);
                    cmd.Parameters.AddWithValue("@EndTime", appointment.EndTime);
                    cmd.Parameters.AddWithValue("@title", appointment.Title);
                    cmd.Parameters.AddWithValue("@Description", appointment.Description);
                    cmd.Parameters.AddWithValue("@HostID", appointment.HostId);
                    cmd.Parameters.AddWithValue("@location", appointment.Location);
                    this.openConnection();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public int addReminder( MyReminder reminder)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("addReminder", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@userId", reminder.userId);
                    cmd.Parameters.AddWithValue("@appointmentId", reminder.appointment.Id);
                    cmd.Parameters.AddWithValue("@remindTime", reminder.remindTime);
                    SqlParameter remindIdParam = new SqlParameter("@remindId", SqlDbType.Int);
                    remindIdParam.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(remindIdParam);
                    this.openConnection();
                    cmd.ExecuteNonQuery();
                    int appointmentId = (int)remindIdParam.Value;
                    return appointmentId;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }
}
