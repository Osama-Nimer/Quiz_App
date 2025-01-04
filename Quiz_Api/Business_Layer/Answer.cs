using Data_Layer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Data_Layer.AnswersData;
using static Data_Layer.ChoicesData;

namespace Business_Layer
{
    public class Answer
    {
        public int AnswerID { get; set; }
        public int QuizID { get; set; }
        public int QuestionID { get; set; }
        public int ChoiceID { get; set; }
        public Quiz QuizInfo { get; set; }
        public Choice ChoiceInfo { get; set; }
        public Question QuestionInfo { get; set; }
        public enMode Mode = enMode.AddNew;
        public AnswerDTO ADTO
        {
            get { return (new AnswerDTO(this.AnswerID,this.QuizID,this.ChoiceID, this.QuestionID)); }
        }
        public enum enMode
        {
            AddNew = 0,
            Update = 1
        }

        public Answer(AnswerDTO ADTO, enMode cMdde = enMode.AddNew)
        {
            this.AnswerID= ADTO.AnswerID;
            this.QuizID = ADTO.QuizID;
            this.ChoiceID = ADTO.ChoiceID;
            this.QuestionID = ADTO.QuestionID;
            Mode = cMdde;
        }

        private Answer(int AnswerID ,int QuizID, int ChoiceID, int QuestionID)
        {
            this.AnswerID = AnswerID;
            this.QuizID= QuizID;
            this.ChoiceID = ChoiceID;
            this.QuestionID = QuestionID;
            this.QuestionInfo = Question.FindQuestionbyQuestionID(this.QuestionID);
            this.QuizInfo = Quiz.FindQuizbyQuizID(this.QuizID);
            this.ChoiceInfo = Choice.FindChoicebyChoiceID(this.ChoiceID);
            Mode = enMode.Update;
        }

        public static List<AnswerDTO> GetAllAnswers()
        {
            return AnswersData.GetAllAnswer();
        }


        private bool _AddNewAnswer()
        {
            this.AnswerID = AnswersData.AddNewAnswer(ADTO);
            return (AnswerID != -1);
        }


        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewAnswer())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;
            }
            return false;
        }


        public static Answer FindAnswerbyAnswerID(int AnswerId)
        {

            AnswerDTO Answer = AnswersData.GetAnswerByID(AnswerId);

            if (Answer != null)
                return new Answer(Answer.AnswerID, Answer.QuizID, Answer.ChoiceID, Answer.QuestionID);
            else
                return null;

        }

        public static bool DeleteAnswer(AnswerDTO Answer)
        {
            return AnswersData.DeleteAnswer(Answer);
        }

    }
}
