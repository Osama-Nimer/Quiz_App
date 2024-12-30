using Data_Layer;
using static Data_Layer.UsersData;

namespace Business_Layer
{
    public class User
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public enMode Mode = enMode.AddNew;
        public UserDTO UDTO
        {
            get { return (new UserDTO(this.UserID, this.UserName, this.Email,this.Password)); }
        }
        public enum enMode
        {
            AddNew = 0,
            Update = 1
        }

        public User(UserDTO UDTO, enMode cMdde = enMode.AddNew)
        {
            this.UserID = UDTO.UserID;
            this.UserName = UDTO.UserName;
            this.Email = UDTO.Email;
            this.Password = UDTO.Password;
            Mode = cMdde;
        }

        private User(int UserID, string UserName,string Email, string Password)
        {
            this.UserID = UserID;
            this.UserName = UserName;
            this.Email = Email;
            this.Password = Password;
            Mode = enMode.Update;
        }



        public static List<UserDTO> GetAllUsers()
        {
            return UsersData.GetAllUsers();
        }

        private bool _AddNewUser()
        {
            this.UserID = UsersData.AddNewUser(UDTO);
            return (UserID != -1);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewUser())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;
            }
            return false;
        }

        public static User FindUserbyUserID(UserDTO user)
        {

            UserDTO User = UsersData.GetUserByUserID(user);

            if (User != null)
                return new User(User.UserID, User.UserName,User.Email , User.Password);
            else
                return null;

        }
        public static User FindUserbyUserNameAndPassword(string UserName, string Password)
        {
            UserDTO user = UsersData.GetUserByUserNameAndPassword(UserName, Password);

            if (user != null)
            {
                return new User(user.UserID, user.UserName,user.Email, user.Password);
            }
            else
            {
                return null;
            }
        }

        public static bool IsUserExist(UserDTO user)
        {
            return UsersData.IsUserExist(user);
        }

    }
}
