using Common;

namespace Drawfael.Services
{
    public class UserService
    {
        private User me = new User();
        public static User him = new User() { Color = CellColor.Blue};
        public event EventHandler<DateTime>? userTimeChanged;

        public UserService()
        {

        }
        public User GetUser()
        {
            return me;
        }

        public void UserPlaced(User user)
        {
            user.LastColor = DateTime.Now;
            userTimeChanged?.Invoke(null,me.LastColor);
        }
    }
}
