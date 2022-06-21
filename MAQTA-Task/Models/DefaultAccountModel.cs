using System;

namespace MAQTA.Models
{
    public class DefaultAccountModel
    {
        public const string DefaultAccount = "DefaultAccount";

        public string UserName { get; set; } = String.Empty;
        public string Password { get; set; } = String.Empty;
    }
}
