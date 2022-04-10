using Common;
using System.Security.Claims;

namespace Drawfael.Services
{
    public class UserService
    {
        private User me = new User();
        public static User him = new User() { Color = CellColor.Blue, Username = "him"};

        //replace with db
        private Dictionary<string, User> _users = new Dictionary<string, User>();
        public event EventHandler<User>? UserTimeChanged;

        public UserService()
        {
            _users.Add(him.Username, him);
        }
        public User GetUser(ClaimsPrincipal cp)
        {
            var name = cp?.Identity?.Name;
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }
            if (!_users.ContainsKey(cp.Identity?.Name))
            {
                var newUser = new User() { Color = CellColor.Red, LastColor = DateTime.Now.AddHours(-1), Username = name };
                _users.Add(name, newUser);
            }

            return _users[name];
        }

        public void UserPlaced(User user)
        {
            var savedUser = _users[user.Username];
            savedUser.LastColor = DateTime.Now;
            UserTimeChanged?.Invoke(null, savedUser);
        }
    }
}
