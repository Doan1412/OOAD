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
                    user.Id = reader.GetInt32(reader.GetOrdinal("userId"));
                    user.Name = reader.GetString(reader.GetOrdinal("username"));
                    user.Email = reader.GetString(reader.GetOrdinal("email"));
                    list.Add(user);
                }
                connector.closeConnection();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                connector.closeConnection();
                reader.Close();
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
                    user.Id = reader.GetInt32(reader.GetOrdinal("userId"));
                    user.Name=reader.GetString(reader.GetOrdinal("username"));
                    user.Email=reader.GetString(reader.GetOrdinal("email"));
                    list.Add(user);
                }
                connector.closeConnection();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                connector.closeConnection();
                reader.Close();
            }
            return list;
        }
    }
}
