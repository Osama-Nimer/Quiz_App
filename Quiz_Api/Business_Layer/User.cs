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
        public bool Role { get; set; } = false;
        public enMode Mode = enMode.AddNew;
        public UserDTO UDTO
        {
            get { return (new UserDTO(this.UserID, this.UserName, this.Email,this.Password,this.Role)); }
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
            this.Role = UDTO.Role;
            Mode = cMdde;
        }

        private User(int UserID, string UserName,string Email, string Password , bool role = false)
        {
            this.UserID = UserID;
            this.UserName = UserName;
            this.Email = Email;
            this.Password = Password;
            this.Role = role;
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
        
        private bool _UpdateUser()
        {
            return UsersData.UpdateUser(UDTO);
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
                case enMode.Update:
                    return _UpdateUser();
            }
            return false;
        }

        public static User FindUserbyUserID(int userID)
        {

            UserDTO User = UsersData.GetUserByUserID(userID);

            if (User != null)
                return new User(User.UserID, User.UserName,User.Email , User.Password, User.Role);
            else
                return null;

        }
        public static User FindUserbyUserNameAndPassword(string UserName, string Password)
        {
            UserDTO user = UsersData.GetUserByUserNameAndPassword(UserName, Password);

            if (user != null)
            {
                return new User(user.UserID, user.UserName,user.Email, user.Password ,user.Role);
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

        public static bool DeleteUser(UserDTO user)
        {
            return UsersData.DeleteUser(user);
        }
    }
}
