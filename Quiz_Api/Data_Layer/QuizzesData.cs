using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Layer
{
    public class QuizzesData
    {
        public class QuizDTO
        {


            public QuizDTO(int quizId , int userId , int score)
            {
                this.QuizID = quizId;
                this.UserID = userId;
                this.Score = score;
            }

            public int QuizID{ get; set; }

            public int UserID{ get; set; }

            public int Score{ get; set; }

        }
    
        public static List<QuizDTO> GetAllQuizzes()
        {
            var list = new List<QuizDTO>();
            SqlConnection conn = new SqlConnection(Connetion.connectionString);
            string Query = "SELECT * FROM Quizzes";
            SqlCommand cmd = new SqlCommand(Query,conn);
            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(
                        new QuizDTO(
                            (int)reader["QuizID"],
                            (int)reader["UserID"],
                            (int)reader["Score"]
                            )
                        );
                }
                reader.Close();
            }
            catch (Exception)
            {

                throw;
            }
            finally { conn.Close(); }
            return list;
        }

        public static QuizDTO GetQuizByID(int quizID)
        {
            QuizDTO Quiz = new QuizDTO(-1, -1, -1);
            SqlConnection connection = new SqlConnection(Connetion.connectionString);
            string Query = "SELECT * FROM Quizzes WHERE QuizID = @QuizID; ";
            SqlCommand command = new SqlCommand(Query,connection);
            command.Parameters.AddWithValue("@QuizID", quizID);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    Quiz.QuizID = (int)reader["QuizID"];
                    Quiz.UserID = (int)reader["UserID"];
                    Quiz.Score = (int)reader["Score"];
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally { connection.Close(); }
            return Quiz ; 
        }

        public static int AddNewQuiz(QuizDTO quizDTO)
        {
            int ID = -1;
            SqlConnection connection = new SqlConnection(Connetion.connectionString);
            string Query = @"INSERT INTO Quizzes(UserID,Score)
                                VALUES(@UserID,@Score);
                                SELECT SCOPE_IDENTITY();";
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@UserID", quizDTO.UserID);  
            command.Parameters.AddWithValue("@Score", quizDTO.Score);  
            try
            {
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    ID = insertedID;
                }
                else
                    ID = -1;
            }
            catch (Exception)
            {
                throw;
            }
            finally { connection.Close(); }
            return ID;
        }

        public static bool DeleteQuiz(QuizDTO quiz)
        {
            int rows = 0;
            SqlConnection connection = new SqlConnection(Connetion.connectionString);
            string Query = "DELETE FROM Quizzes WHERE QuizID = @QuizID";
            SqlCommand cmd = new SqlCommand(Query, connection);
            cmd.Parameters.AddWithValue("QuizID",quiz.QuizID);
            try
            {
                connection.Open();
                rows = cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

                throw;
            }
            finally { connection.Close(); }
            return (rows>0);
        }

    }
}
