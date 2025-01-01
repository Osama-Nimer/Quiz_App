using Data_Layer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Data_Layer.QuestionsData;
using static Data_Layer.QuizzesData;
using static Data_Layer.QuizzesData.QuizDTO;


namespace Business_Layer
{
    public class Quiz
    {
        public int QuizID { get; set; }
        public int UserID { get; set; }
        public User UserInfo { get; set; } 
        public int Score { get; set; }
        public enMode Mode = enMode.AddNew;
        public QuizzesData.QuizDTO QDTO
        {
            get { return (new QuizzesData.QuizDTO(this.QuizID,this.UserID, this.Score)); }
        }
        public enum enMode
        {
            AddNew = 0,
            Update = 1
        }

        public Quiz (QuizzesData.QuizDTO QDTO, enMode cMdde = enMode.AddNew)
        {
            this.QuizID = QDTO.QuizID;
            this.UserID = QDTO.UserID ;
            this.Score = QDTO.Score;
            Mode = cMdde;
        }

        private Quiz(int QuizID,int UserID ,int Score)
        {
            this.QuizID = QuizID;
            this.UserID = UserID;
            this.Score = Score;
            this.UserInfo = User.FindUserbyUserID(this.UserID);
            Mode = enMode.Update;
        }

        public static List<QuizzesData.QuizDTO> GetAllQuizzes()
        {
            return QuizzesData.GetAllQuizzes();
        }

        private bool _AddNewQuiz()
        {
            this.QuizID= QuizzesData.AddNewQuiz(QDTO);
            return (QuizID!= -1);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewQuiz())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;
            }
            return false;
        }

        public static Quiz FindQuizbyQuizID(int QuizID)
        {

            QuizDTO Quiz = QuizzesData.GetQuizByID(QuizID);

            if (Quiz != null)
                return new Quiz(Quiz.QuizID, Quiz.UserID,Quiz.Score);
            else
                return null;

        }

        public static bool DeleteQuiz(QuizDTO Quiz)
        {
            return QuizzesData.DeleteQuiz(Quiz);
        }
    }
}
