using System;
using System.Collections.Generic;
using System.Text;

namespace library.elegalhubs.com.lms.Admin
{
    public static class Permissions
    {
        public static List<string> GeneratePermissionsForModule(string module)
        {
            return new List<string>()
        {
            $"Permissions.{module}.Create",
            $"Permissions.{module}.View",
            $"Permissions.{module}.Edit",
            $"Permissions.{module}.Delete",
        };
        }
        public static class ContactTypes
        {
            public const string View = "Permissions.ContactTypes.View";
            public const string Create = "Permissions.ContactTypes.Create";
            public const string Edit = "Permissions.ContactTypes.Edit";
            public const string Delete = "Permissions.ContactTypes.Delete";
        }
        public static class Projects
        {
            public const string View = "Permissions.Projects.View";
            public const string Create = "Permissions.Projects.Create";
            public const string Edit = "Permissions.Projects.Edit";
            public const string Delete = "Permissions.Projects.Delete";
        }
        public static class Chat
        {
            public const string View = "Permissions.Chat.View";
            public const string Create = "Permissions.Chat.Create";
            public const string Edit = "Permissions.Chat.Edit";
            public const string Delete = "Permissions.Chat.Delete";
        }
    }
}
