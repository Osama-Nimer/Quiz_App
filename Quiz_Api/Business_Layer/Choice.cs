using Data_Layer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Data_Layer.ChoicesData;
using static Data_Layer.QuestionsData;

namespace Business_Layer
{
    public class Choice
    {
        public int ChoiceID { get; set; }
        public int QuestionID { get; set; }
        public Question QuestionInfo { get; set; }
        public string ChoiceText { get; set; }
        public bool IsCorrect{ get; set; }
        public enMode Mode = enMode.AddNew;
        public ChoiceDTO CDTO
        {
            get { return (new ChoiceDTO(this.ChoiceID,this.QuestionID, this.ChoiceText,this.IsCorrect)); }
        }
        public enum enMode
        {
            AddNew = 0,
            Update = 1
        }

        public Choice(ChoiceDTO CDTO, enMode cMdde = enMode.AddNew)
        {
            this.ChoiceID = CDTO.ChoiceID;
            this.QuestionID = CDTO.QuestionID;
            this.ChoiceText = CDTO.ChoiceText;
            this.IsCorrect = CDTO.isCorrect;
            Mode = cMdde;
        }

        private Choice(int ChoiceID, int QuestionID, string ChoiceText, bool isCorrect)
        {
            this.ChoiceID = ChoiceID;
            this.QuestionID = QuestionID;
            this.ChoiceText = ChoiceText;
            this.IsCorrect = isCorrect;
            this.QuestionInfo = Question.FindQuestionbyQuestionID(this.QuestionID);
            Mode = enMode.Update;
        }



        public static List<ChoiceDTO> GetAllChoices()
        {
            return ChoicesData.GetAllChoices();
        }


        private bool _AddNewChoice()
        {
            this.ChoiceID = ChoicesData.AddNewChoice(CDTO);
            return (ChoiceID != -1);
        }

        private bool _UpdateChoice()
        {
            return ChoicesData.UpdateChoice(CDTO);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewChoice())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;
                case enMode.Update:
                    return _UpdateChoice();
            }
            return false;
        }


        public static Choice FindChoicebyChoiceID(int ChoiceId)
        {

            ChoiceDTO Choice = ChoicesData.GetChoiceByID(ChoiceId);

            if (Choice != null)
                return new Choice(Choice.ChoiceID, Choice.QuestionID, Choice.ChoiceText, Choice.isCorrect);
            else
                return null;

        }


        public static bool DeleteChoice(ChoiceDTO Choice)
        {
            return ChoicesData.DeleteChoice(Choice);
        }
    }
}

