using Data_Layer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Data_Layer.QuizzesData;
using static Data_Layer.UsersData;

namespace Business_Layer
{
    public class Quiz
    {
        public int QuizID { get; set; }
        public string Subject { get; set; }
        public enMode Mode = enMode.AddNew;
        public QuizDTO MyProperty { get; set; }
        public QuizDTO QDTO
        {
            get { return (new QuizDTO(this.QuizID, this.Subject)); }
        }
        public enum enMode
        {
            AddNew = 0,
            Update = 1
        }

        public Quiz(QuizDTO QDTO, enMode cMdde = enMode.AddNew)
        {
            this.QuizID = QDTO.QuizID;
            this.Subject = QDTO.Subject;
            Mode = cMdde;
        }

        private Quiz(int QuizID, string Subject)
        {
            this.QuizID = QuizID;
            this.Subject = Subject;
            Mode = enMode.Update;
        }

        public static List<QuizDTO> GetAllQuizzes()
        {
            return QuizzesData.GetAllQuizzes();
        }


        private bool _AddNewQuiz()
        {
            this.QuizID = QuizzesData.AddNewQuiz(QDTO);
            return (QuizID != -1);
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


        public static Quiz FindQuizbyQuizID(QuizDTO quiz)
        {

            QuizDTO Quiz = QuizzesData.GetQuizByQuizID(quiz);

            if (Quiz != null)
                return new Quiz(Quiz.QuizID, Quiz.Subject);
            else
                return null;

        }
    }
}
