using Microsoft.Data.SqlClient;
using static Data_Layer.UsersData;

namespace Data_Layer
{
    public class QuestionsData
    {
        public class QuestionDTO
        {
            public QuestionDTO(int QuestionID, string QuestionText)
            {
                this.QuestionID = QuestionID;
                this.QuestionText = QuestionText;
            }
            public int QuestionID { get; set; }
            public string QuestionText { get; set; }
        }

        public static List<QuestionDTO> GetAllQuizzes()
        {
            var quizzes = new List<QuestionDTO>();
            SqlConnection connection = new SqlConnection(Connetion.connectionString);
            string Query = @"Select * From Questions";
            SqlCommand command = new SqlCommand(Query, connection);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    quizzes.Add(new QuestionDTO(
                        (int)reader["QuestionID"],
                        (string)reader["QuestionText"]
                        ));

                };
                reader.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally { connection.Close(); }
            return quizzes;
        }



        public static QuestionDTO GetQuizByQuizID(QuestionDTO question)
        {
            QuestionDTO Question = new QuestionDTO(-1, "");
            bool IsFound = false;
            SqlConnection connection = new SqlConnection(Connetion.connectionString);
            string Query = @"Select * From Questions Where QuestionID = @QuestionID";
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@QuestionID", question.QuestionID);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    IsFound = true;
                    Question.QuestionID= question.QuestionID;
                    Question.QuestionText= (string)reader["QuestionText"];
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
            return Question;
        }


        public static int AddNewQuiz(QuestionDTO Question)
        {
            int quizID = -1;
            SqlConnection connection = new SqlConnection(Connetion.connectionString);
            string Query = @"INSERT INTO Questions(QuestionText)
                                VALUES(@QuestionText);
                                SELECT SCOPE_IDENTITY();";
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@QuestionText", Question.QuestionText);

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null && int.TryParse(result.ToString(), out int InsertedID))
                    quizID = InsertedID;
                else
                    quizID = -1;
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
            }
            finally { connection.Close(); }

            return quizID;
        }

        public static bool UpdateQuestion(QuestionDTO Question)
        {
            int rows = 0;
            SqlConnection connection = new SqlConnection(Connetion.connectionString);
            string Query = "UPDATE Questions SET QuestionText = @QuestionText WHERE QuestionID = @QuestionID;";
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@QuestionID", Question.QuestionID);
            command.Parameters.AddWithValue("@QuestionText", Question.QuestionText);
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


        public static bool DeleteQuestion(QuestionDTO Question)
        {
            int rows = 0;
            SqlConnection connection = new SqlConnection(Connetion.connectionString);
            string Query = "DELETE FROM Questions  WHERE QuestionID = @QuestionID";
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@QuestionID", Question.QuestionID);
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
