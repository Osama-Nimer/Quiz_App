using Microsoft.Data.SqlClient;

namespace Data_Layer
{
    public class UsersData
    {
        public class UserDTO
        {
            public UserDTO(int userID, string username,string email, string password , bool role)
            {
                this.UserID = userID;
                this.UserName = username;
                this.Email = email;
                this.Password = password;
                this.Role = role;
            }
            public int UserID { get; set; }
            public string UserName { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public bool Role { get; set; } = false;
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
                        (string)reader["Password"],
                        (bool)reader["Role"]
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
            UserDTO user = new UserDTO(-1, "","", "",false);
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
                    user.Role = (bool)reader["Role"];
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


        public static UserDTO GetUserByUserNameAndPassword(string Username, string Password)
        {
            UserDTO user = null;
            SqlConnection connection = new SqlConnection(Connetion.connectionString);
            string query = "SELECT * FROM Users WHERE Username = @Username AND Password = @Password";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Username", Username);
            command.Parameters.AddWithValue("@Password", Password);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    Console.WriteLine("User found!");
                    user = new UserDTO(
                        (int)reader["UserID"],
                        (string)reader["Username"],
                        (string)reader["Email"],
                        (string)reader["Password"],
                        (bool)reader["Role"]
                    );
                }
                else
                {
                    Console.WriteLine("No user found with the provided username and password.");
                }
                reader.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
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
            string Query = @"INSERT INTO Users(UserName,Email,Password,Role)
                                VALUES(@UserName,@Email, @Password,0);
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


        public static bool UpdateUser(UserDTO User)
        {
            int rows = 0;
            SqlConnection connection = new SqlConnection(Connetion.connectionString);
            string Query = "UPDATE Users SET Role = 1 WHERE UserID = @UserID";
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@UserID", User.UserID);
            try
            {
                connection.Open();
                rows = command.ExecuteNonQuery();
            }
            catch (Exception)
            {

                throw;
            }
            finally { connection.Close(); }
            return (rows > 0);
        }


        public static bool DeleteUser(UserDTO User)
        {
            int rows = 0;
            SqlConnection connection = new SqlConnection(Connetion.connectionString);
            string Query = "DELETE FROM Users  WHERE UserID = @UserID";
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@UserID", User.UserID);
            try
            {
                connection.Open();
                rows = command.ExecuteNonQuery();
            }
            catch (Exception)
            {

                throw;
            }
            return (rows > 0);
        }
    }
}
