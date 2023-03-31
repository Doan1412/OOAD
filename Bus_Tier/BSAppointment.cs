using Connector_Tier;
using Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bus_Tier
{
    public class BSAppointment
    {
        ConnectorFactory connector =null;
        public BSAppointment()
        {
            connector = new ConnectorFactory();
        }
        public void saveAppointment()
        {

        }
        public List<Appointment> getListAppointmentByUserId(int id)
        {
            SqlDataReader reader = null;
            List<Appointment> list = new List<Appointment>();
            try
            {
                BSUser l = new BSUser();
                reader = connector.getListById("GetAppointmentByUserId", "@Userid", id);
                while (reader.Read())
                {
                    Appointment appointment = new Appointment();
                    appointment.Id = reader.GetSqlInt32(0).Value;
                    appointment.StartTime = reader.GetDateTime(1);
                    appointment.EndTime = reader.GetDateTime(2);
                    appointment.Title = reader.GetString(3);
                    appointment.Description = reader.GetString(4);
                    list.Add(appointment);
                }
                connector.closeConnection();
            }
            catch (Exception e)
            {
                throw e;
            }
            return list;
        }
        public List <Appointment> getListAppointmentByIdAndDate(int id,DateTime date)
        {
            SqlDataReader reader = null;
            List<Appointment> list = new List<Appointment>();
            try
            {
                BSUser l = new BSUser();
                reader = connector.getListAppByIdDate(id,date);
                while (reader.Read())
                {
                    Appointment appointment = new Appointment();
                    appointment.Id = reader.GetSqlInt32(0).Value;
                    appointment.StartTime = reader.GetDateTime(2);
                    appointment.EndTime = reader.GetDateTime(3);
                    appointment.Title = reader.GetString(1);
                    appointment.Description = reader.GetString(4);
                    list.Add(appointment);
                }
                connector.closeConnection();
            }
            catch (Exception e)
            {
                throw e;
            }
            return list;
        }
        public int addAppointment(Appointment appointment)
        {
            try
            {
                return connector.addAppointment(appointment);  
            }
            catch ( Exception e)
            {
                throw e;
            }
        }
    }
}
