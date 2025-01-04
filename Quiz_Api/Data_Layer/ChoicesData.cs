using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Data_Layer.QuestionsData;

namespace Data_Layer
{
    public class ChoicesData
    {
        public class ChoiceDTO {

            public ChoiceDTO(int choiceID , int questionID ,string choiceText ,bool isCorrect)
            {
                this.ChoiceID = choiceID;   
                this.QuestionID = questionID;
                this.ChoiceText = choiceText;
                this.isCorrect = isCorrect;
            }

            public int ChoiceID { get; set; }
            public int QuestionID { get; set; }
            public string ChoiceText { get; set; }
            public bool isCorrect { get; set; }
        }



        public static List<ChoiceDTO> GetAllChoices()
        {
            var list = new List<ChoiceDTO>();
            SqlConnection connection = new SqlConnection(Connetion.connectionString);
            string Query = "SELECT * FROM Choices";
            SqlCommand command = new SqlCommand(Query, connection);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(
                        new ChoiceDTO(
                            (int)reader["ChoiceID"],
                            (int)reader["QuestionID"],
                            (string)reader["ChoiceText"],
                            (bool)reader["IsCorrect"]
                            )
                        );
                    
                }
                reader.Close();
            }
            catch (Exception)
            {

                throw;
            }
            finally { connection.Close(); }
            return list;
        }

        public static ChoiceDTO GetChoiceByID(int ID)
        {
            ChoiceDTO choice = new ChoiceDTO(-1,-1,"",false);
            SqlConnection connection = new SqlConnection(Connetion.connectionString);
            string Query = "SELECT * FROM Choices WHERE ChoiceID = @ChoiceID";
            SqlCommand command = new SqlCommand (Query, connection);    
            command.Parameters.AddWithValue("ChoiceID", ID);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    choice.ChoiceID = (int)reader["ChoiceID"];
                    choice.QuestionID = (int)reader["QuestionID"];
                    choice.ChoiceText = (string)reader["ChoiceText"];
                    choice.isCorrect = (bool)reader["IsCorrect"];
                }
                reader.Close();
            }
            catch (Exception)
            {

                throw;
            }
            finally { connection.Close(); }
            return choice;
        }

        public static int AddNewChoice(ChoiceDTO choice)
        {
            int ID = -1;
            SqlConnection connection = new SqlConnection (Connetion.connectionString);
            string Query = @"INSERT INTO Choices(QuestionID,ChoiceText,IsCorrect)
                                VALUES(@QuestionID,@ChoiceText,@IsCorrect);
                                SELECT SCOPE_IDENTITY();";
            SqlCommand cmd = new SqlCommand (Query, connection);
            cmd.Parameters.AddWithValue("@QuestionID", choice.QuestionID);
            cmd.Parameters.AddWithValue("@ChoiceText", choice.ChoiceText);
            cmd.Parameters.AddWithValue("@IsCorrect", choice.isCorrect);
            try
            {
                connection.Open();
                object result = cmd.ExecuteScalar();
                if (result != null && int.TryParse(result.ToString(), out int InsertedID))
                {
                    ID = InsertedID;
                }
                else
                    ID = -1;
            }
            catch (Exception)
            {

                throw;
            }
            return ID;
        }


        public static bool UpdateChoice(ChoiceDTO choice)
        {
            int rows = 0;
            SqlConnection connetion = new SqlConnection(Connetion.connectionString);
            string Query = "UPDATE Choices SET ChoiceText = @ChoiceText WHERE ChoiceID = @ChoiceID;";
            SqlCommand command = new SqlCommand (Query, connetion);
            command.Parameters.AddWithValue("ChoiceID", choice.ChoiceID);
            command.Parameters.AddWithValue("ChoiceText", choice.ChoiceText);
            try
            {
                connetion.Open();
                rows = command.ExecuteNonQuery();
            }
            catch (Exception)
            {

                throw;
            }
            finally { connetion.Close(); }
            return (rows > 0);  
        }

        public static bool DeleteChoice(ChoiceDTO choice)
        {
            int rows = 0;
            SqlConnection connection = new SqlConnection(Connetion.connectionString);
            string Query = "DELETE FROM Choices  WHERE ChoiceID = @ChoiceID";
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@ChoiceID", choice.ChoiceID);
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
    }
}
