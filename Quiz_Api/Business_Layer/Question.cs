using Data_Layer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Data_Layer.QuestionsData;
using static Data_Layer.UsersData;

namespace Business_Layer
{
    public class Question
    {
        public int QuestionID { get; set; }
        public string QuestionText { get; set; }
        public enMode Mode = enMode.AddNew;
        public QuestionDTO MyProperty { get; set; }
        public QuestionDTO QDTO
        {
            get { return (new QuestionDTO(this.QuestionID, this.QuestionText)); }
        }
        public enum enMode
        {
            AddNew = 0,
            Update = 1
        }

        public Question(QuestionDTO QDTO, enMode cMdde = enMode.AddNew)
        {
            this.QuestionID = QDTO.QuestionID;
            this.QuestionText = QDTO.QuestionText;
            Mode = cMdde;
        }

        private Question(int QuizID, string Subject)
        {
            this.QuestionID = QuizID;
            this.QuestionText = Subject;
            Mode = enMode.Update;
        }

        public static List<QuestionDTO> GetAllQuestions()
        {
            return QuestionsData.GetAllQuizzes();
        }


        private bool _AddNewQuiz()
        {
            this.QuestionID = QuestionsData.AddNewQuiz(QDTO);
            return (QuestionID != -1);
        }

        private bool _UpdateQuestion()
        {
            return QuestionsData.UpdateQuestion(QDTO);
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
                case enMode.Update:
                    return _UpdateQuestion();
            }
            return false;
        }


        public static Question FindQuestionbyQuestionID(int questionId)
        {

            QuestionDTO Question = QuestionsData.GetQuizByQuizID(questionId);

            if (Question != null)
                return new Question(Question.QuestionID, Question.QuestionText);
            else
                return null;

        }


        public static bool DeleteQuestion(QuestionDTO Question)
        {
            return QuestionsData.DeleteQuestion(Question);
        }
    }
}
