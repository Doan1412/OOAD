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
    public class BSUser
    {
        ConnectorFactory connector = null;
        public BSUser()
        {
            connector = new ConnectorFactory();
        }
        public List<User> getUser()
        {
            SqlDataReader reader = null;
            List<User> list = new List<User>();
            try
            {
                reader = connector.getData("person");
                while (reader.Read())
                {
                    User user = new User();
                    user.Id = reader.GetSqlInt32(0).Value;
                    user.Name = reader.GetString(1);
                    user.Email = reader.GetString(2);
                    list.Add(user);
                }
                connector.closeConnection();
            }
            catch (Exception e)
            {
                throw e;
            }
            return list;
        }
        public List<User> getListUserByAppointmentId(int id)
        {
            SqlDataReader reader = null;
            List<User> list = new List<User>();
            try
            {
                reader = connector.getListById("getUserListAppointment", "@Appointmentid", id);
                while (reader.Read())
                {
                    User user= new User();  
                    user.Id=reader.GetSqlInt32(0).Value;  
                    user.Name=reader.GetString(1);
                    user.Email=reader.GetString(2);
                    list.Add(user);
                }
                connector.closeConnection();
            }
            catch (Exception e)
            {
                throw e;
            }
            return list;
        }
        public void addUserToAppointment(int userId, int appointmentId)
        {
            try
            {
                connector.addUserToAppointment(userId, appointmentId);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
