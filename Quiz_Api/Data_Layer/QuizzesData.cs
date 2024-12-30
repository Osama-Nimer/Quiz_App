using Microsoft.Data.SqlClient;

namespace Data_Layer
{
    public class QuizzesData
    {
        public class QuizDTO
        {
            public QuizDTO(int QuizID, string Subject)
            {
                this.QuizID = QuizID;
                this.Subject = Subject;
            }
            public int QuizID { get; set; }
            public string Subject { get; set; }
        }

        public static List<QuizDTO> GetAllQuizzes()
        {
            var quizzes = new List<QuizDTO>();
            SqlConnection connection = new SqlConnection(Connetion.connectionString);
            string Query = @"Select * From Quizzes";
            SqlCommand command = new SqlCommand(Query, connection);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    quizzes.Add(new QuizDTO(
                        (int)reader["QuizID"],
                        (string)reader["Subject"]
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



        public static QuizDTO GetQuizByQuizID(QuizDTO Quiz)
        {
            QuizDTO quiz = new QuizDTO(-1, "");
            bool IsFound = false;
            SqlConnection connection = new SqlConnection(Connetion.connectionString);
            string Query = @"Select * From Quizzes Where QuizID = @QuizID";
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@QuizID", Quiz.QuizID);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    IsFound = true;
                    quiz.QuizID= Quiz.QuizID;
                    quiz.Subject= (string)reader["Subject"];
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
            return quiz;
        }


        public static int AddNewQuiz(QuizDTO Quiz)
        {
            int quizID = -1;
            SqlConnection connection = new SqlConnection(Connetion.connectionString);
            string Query = @"INSERT INTO Quizzes(Subject)
                                VALUES(@Subject);
                                SELECT SCOPE_IDENTITY();";
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@Subject", Quiz.Subject);

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


    }
}
