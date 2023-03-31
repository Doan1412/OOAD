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
                DataTable dt = new DataTable();
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
                DataTable dt = new DataTable();
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
        public int addAppointment (Appointment appointment)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("AddAppointment", conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@StartTime", appointment.StartTime);
                    cmd.Parameters.AddWithValue("@EndTime",appointment.EndTime);
                    cmd.Parameters.AddWithValue("@title", appointment.Title);
                    cmd.Parameters.AddWithValue("@Description", appointment.Description);
                    cmd.Connection = conn;
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
        public void addUserToAppointment(int userId, int appointmentId)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("InsertAppointmentUser", conn))
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
    }
}
