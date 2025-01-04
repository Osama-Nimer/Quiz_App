using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Data_Layer.ChoicesData;

namespace Data_Layer
{
    public class AnswersData
    {
        public class AnswerDTO
        {
            public int AnswerID { get; set; }
            public int QuizID { get; set; }
            public int QuestionID { get; set; }
            public int ChoiceID { get; set; }
            public AnswerDTO(int answerId , int quizId , int questionId , int choiceId)
            {
                this.AnswerID = answerId;
                this.QuizID = quizId;
                this.QuestionID = questionId;
                this.ChoiceID = choiceId;
            }

            
        }
            public static List<AnswerDTO> GetAllAnswer()
            {
                var list = new List<AnswerDTO>();
                SqlConnection connection = new SqlConnection(Connetion.connectionString);
                string Query = "SELECT * FROM Answers";
                SqlCommand command = new SqlCommand(Query, connection);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        list.Add(
                            new AnswerDTO(
                                (int)reader["AnswerID"],
                                (int)reader["QuizID"],
                                (int)reader["QuestionID"],
                                (int)reader["ChoiceID"]
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

            public static AnswerDTO GetAnswerByID(int ID)
            {
                AnswerDTO answer = new AnswerDTO(-1, -1, -1, -1);
                SqlConnection connection = new SqlConnection(Connetion.connectionString);
                string Query = "SELECT * FROM Answers WHERE AnswerID = @AnswerID";
                SqlCommand command = new SqlCommand(Query, connection);
                command.Parameters.AddWithValue("AnswerID", ID);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        answer.AnswerID = (int)reader["AnswerID"];
                        answer.QuestionID = (int)reader["QuestionID"];
                        answer.QuizID = (int)reader["QuizID"];
                        answer.ChoiceID = (int)reader["ChoiceID"];
                    }
                    reader.Close();
                }
                catch (Exception)
                {

                    throw;
                }
                finally { connection.Close(); }
                return answer;
            }

        public static int AddNewAnswer(AnswerDTO Answer)
        {
            int ID = -1;
            SqlConnection connection = new SqlConnection(Connetion.connectionString);
            string Query = @"INSERT INTO Answers(QuestionID,QuizID,ChoiceID)
                                VALUES(@QuestionID,@QuestionID,@ChoiceID);
                                SELECT SCOPE_IDENTITY();";
            SqlCommand cmd = new SqlCommand(Query, connection);
            cmd.Parameters.AddWithValue("@QuestionID", Answer.QuestionID);
            cmd.Parameters.AddWithValue("@ChoiceID", Answer.ChoiceID);
            cmd.Parameters.AddWithValue("@QuizID", Answer.QuizID);
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

        public static bool DeleteAnswer(AnswerDTO Answer)
        {
            int rows = 0;
            SqlConnection connection = new SqlConnection(Connetion.connectionString);
            string Query = "DELETE FROM Answers  WHERE AnswerID = @AnswerID";
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@ChoiceID", Answer.AnswerID);
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
