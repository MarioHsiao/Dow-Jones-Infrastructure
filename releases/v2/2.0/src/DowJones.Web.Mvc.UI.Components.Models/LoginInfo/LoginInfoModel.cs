using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace DowJones.Web.Mvc.UI.Components.LoginInfo
{    /*
    public class LoginInfoModel
    {
        public static string DefaultTrackFolder { get; set; }
        public List<SelectListItem> DefaultAlert { get; set; }
    }
    
    public enum EasyAuthState
    {
        canNotConvert,
        canConvert,
        canUpdate,
    }

    public enum EmailDisplayType
    {
        Editable,
        NotEditable,
        Hidden
    }

    public static class LoginInfoModel1
    {
        private static bool _canEditPassword = true;
        private static bool _hideEmail=false;
        private static EasyAuthState _authState = EasyAuthState.canConvert;
        private static EmailDisplayType _emailDisplay = EmailDisplayType.Editable;
        private static bool _canAutoLogin = true;
        
        
        public static bool CanEditPassword
        {
            set { _canEditPassword = value; }
            get { return _canEditPassword; }
        }

        
        public static bool HideEmail
        {
            set { _hideEmail = value; }
            get { return _hideEmail; }
        }

        public static bool CanAutoLogin
        {
            set { _canAutoLogin = value; }
            get { return _canAutoLogin; }
        }

        
        public  static EasyAuthState AuthState
        {
            set { _authState = value; }
            get { return _authState; }
        }

        
        public static EmailDisplayType EmailDisplay
        {
            set { _emailDisplay = value; }
            get { return _emailDisplay; }
        }

        private static string _emailAddress = String.Empty;
        public static string EmailAddress
        {
            set { _emailAddress = value; }
            get { return _emailAddress; }
        }

        private static string _userId = String.Empty;
        public static string UserId
        {
            set { _userId = value; }
            get
            {
                //return string.IsNullOrEmpty(_userId) ? UIRegistry.SessionData.NavigationPreferences.userId : _userId;
                return string.IsNullOrEmpty(_userId) ? _userId : _userId;
            }
        }
    }

    */
    public class LoginInfoModel : ViewComponentModel
    {

         [ClientData("userId")]
        public string UserId { set; get; }

         [ClientData("nameSpace")]
        public string NameSpace { set; get; }

        [ClientData("emailAddress")]
        public string EmailAddress { set; get; }

        [ClientData("planId")]
        public string PlanId { set; get; }

        [ClientData("canChangePassword")]
        public bool CanChangePassword { set; get; }

        [ClientData("password")]
        public string Password { set; get; }

        [ClientData("newPassword")]
        public string NewPassword { set; get; }

        [ClientData("verifyPassword")]
        public string VerifyPassword { set; get; }

        [ClientData("authLwrFlag")]
        public string AuthLwrFlag { set; get; }

        [ClientData("authEmailLoginState")]
        public string AuthEmailLoginState { set; get; }

        [ClientData("authUserType")]
        public string AuthUserType { set; get; }

        [ClientData("prefAdminEditRegCanEditReg")]
        public bool PrefAdminEditRegCanEditReg { set; get; }

        [ClientData("autoLoginCookieKey")]
        public string AutoLoginCookieKey { set; get; }

        [ClientData("userDataEmailLogin")]
        public string UserDataEmailLogin { set; get; }

        [ClientData("canAutoLogin")]
        public bool CanAutoLogin { set; get; }

        [ClientData("hasAutoLogin")]
        public bool HasAutoLogin { set; get; }

        [ClientData("hideButtons")]
        public bool HideButtons  { set; get; }       
            

        //public enum EasyAuthState
        //{
        //    canNotConvert,
        //    canConvert,
        //    canUpdate,
        //}

        //public enum EmailDisplayType
        //{
        //    Editable,
        //    NotEditable,
        //    Hidden
        //}

        [ClientData("authState")]
        public string AuthState
        {
            set { }
            get
            {
                if (UserDataEmailLogin.ToLower() == "disabled")
                {
                    UserDataEmailLogin = AuthEmailLoginState;
                }

                if (AuthUserType.ToLower() == "creditcard" ||
                    AuthLwrFlag.ToLower() == "c" || AuthLwrFlag.ToLower() == "l")
                {
                    switch (UserDataEmailLogin)
                    {
                        case "enabled":
                        case "pending":
                            return "canUpdate";
                        default:
                            return "canConvert";
                    }
                }
                return "canNotConvert";
            }
        }
        [ClientData("emailDisplay")]
        public string EmailDisplay
        {
            set { }
            get
            {
                string emailDisplay;
                switch (AuthState)
                {
                    case "canNotConvert":
                    case "canConvert":
                        emailDisplay = PrefAdminEditRegCanEditReg ? "Hidden" : "NotEditable";
                        break;
                    default:
                        emailDisplay = "Editable";
                        break;
                }
                return emailDisplay;
            }
        }

        //[JsonProperty("enableEmailLogin")]
        //public bool EnableEmailLogin { set; get; }

        //[JsonProperty("canConvertEmail")]
        //public bool CanConvertEmail { set; get; }

        //[JsonProperty("canUpdatetEmail")]
        //public bool CanUpdateEmail { set; get; }

        //[JsonProperty("isChangePassword")]
        //public bool IsChangePassword {set; get;}

        //[JsonProperty("isAutoLogin")]
        //public bool IsAutoLogin { set; get; }  

        //[JsonProperty("saveUserIdPassword")]
        //public bool SaveUserIdPassword { set; get; }

        //[JsonProperty("loginPageURL")]
        //public string LoginPageURL { set; get; }

        //[JsonProperty("ipAddress")]
        //public string IpAddress { set; get; }

        //[JsonProperty("secondaryURL")]
        //public string SecondaryURL { set; get; }
       
    }

}
