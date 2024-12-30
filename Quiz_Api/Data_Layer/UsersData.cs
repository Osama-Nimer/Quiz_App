using Microsoft.Data.SqlClient;

namespace Data_Layer
{
    public class UsersData
    {
        public class UserDTO
        {
            public UserDTO(int userID, string username,string email, string password)
            {
                this.UserID = userID;
                this.UserName = username;
                this.Email = email;
                this.Password = password;
            }
            public int UserID { get; set; }
            public string UserName { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public static List<UserDTO> GetAllUsers()
        {
            var users = new List<UserDTO>();
            SqlConnection connection = new SqlConnection(Connetion.connectionString);
            string Query = @"Select * From Users";
            SqlCommand command = new SqlCommand(Query, connection);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    users.Add(new UserDTO(
                        (int)reader["UserID"],
                        (string)reader["UserName"],
                        (string)reader["Email"],
                        (string)reader["Password"]
                        ));

                };
                reader.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally { connection.Close(); }
            return users;
        }


        public static UserDTO GetUserByUserID(UserDTO User)
        {
            UserDTO user = new UserDTO(-1, "","", "");
            bool IsFound = false;
            SqlConnection connection = new SqlConnection(Connetion.connectionString);
            string Query = @"Select * From Users Where UserID = @UserID";
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@UserID", User.UserID);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    IsFound = true;
                    user.UserID = User.UserID;
                    user.UserName = (string)reader["UserName"];
                    user.Email = (string)reader["Email"];
                    user.Password = (string)reader["Password"];
                }
                else
                    IsFound = false;
                reader.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally { connection.Close(); }
            return user;
        }


        public static UserDTO GetUserByUserNameAndPassword(string UserName, string Password)
        {
            UserDTO user = null;  // Set to null initially
            SqlConnection connection = new SqlConnection(Connetion.connectionString);
            string query = "SELECT * FROM Users WHERE UserName = @UserName AND Password = @Password";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@UserName", UserName);
            command.Parameters.AddWithValue("@Password", Password);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    user = new UserDTO(
                        (int)reader["UserID"],
                        (string)reader["UserName"],
                        (string)reader["Email"],
                        (string)reader["Password"] // Get password from database
                    );
                }
                reader.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                connection.Close();
            }

            return user;
        }


        public static int AddNewUser(UserDTO User)
        {
            int userID = -1;
            SqlConnection connection = new SqlConnection(Connetion.connectionString);
            string Query = @"INSERT INTO Users(UserName,Email,Password)
                                VALUES(@UserName,@Email, @Password);
                                SELECT SCOPE_IDENTITY();";
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@UserName", User.UserName);
            command.Parameters.AddWithValue("@Email", User.Email);
            command.Parameters.AddWithValue("@Password", User.Password);

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null && int.TryParse(result.ToString(), out int InsertedID))
                    userID = InsertedID;
                else
                    userID = -1;
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
            }
            finally { connection.Close(); }

            return userID;
        }

        public static bool IsUserExist(UserDTO User)
        {
            bool IsFound = false;
            SqlConnection con = new SqlConnection(Connetion.connectionString);
            string Query = "Select Username From Users";
            SqlCommand cmd = new SqlCommand(Query, con);
            try
            {
                con.Open(); 
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read()) {
                    if ((string)reader["Username"] == User.UserName)
                    {
                        IsFound = true;
                        break;
                    }
                        
                }
                reader.Close(); 
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                con.Close();
            }
            return IsFound ;
        }
    }
}
