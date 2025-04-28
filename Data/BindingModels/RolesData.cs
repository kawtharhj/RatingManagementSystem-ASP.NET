namespace RatingManagementSystem.Data.BindingModels
{
    public class RolesData
    {
        public class RolesDataBindingModel
        {
            public string RoleID { get; set; }
            public string RoleName { get; set; }
            public bool isSelected { get; set; }
        }



        public class UserRolesBindingModel
        {
            public string Id { get; set; }
            public string Email { get; set; }
            public List<RolesDataBindingModel> Roles { get; set; }
        }

    }
}
