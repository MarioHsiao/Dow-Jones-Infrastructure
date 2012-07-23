/*!
* LoginInfo

The following functions should be implemented in main project:
1. function translate
2. function SaveLoginInfo

The following tokens should be added to a page which uses the control:
1. "emptyEmailMessage"
2. "profValidEmail"
3. "profOldPasswd"
4. "profNewPasswd"
5. "profRepeatPasswd"
6. "profPasswdDontMatch"
*/

    DJ.UI.LoginInfo = DJ.UI.Component.extend({

        LoginInfoData:
        {
            IsUpdateEmail: false,
            IsConvertEmail: false,
            IsChangePassword: false,
            IsAutoLogin: false,
            Email: "",
            OldPass: "",
            NewPass: "",
            NewPassVerify: "",
            IsConvertEmailOptionOn: false // NN - specifies if it is convert or update email
        },

        // Default options
        defaults: {
            debug: false,
            cssClass: 'LoginInfo'

        },

        events: {
            // jQuery events are namespaced as <event>.<namespace>
            afterSetData: "afterSetData.dj.LoginInfo",
            saveClick: "saveClick.dj.LoginInfo",
            resetClick: "resetClick.dj.LoginInfo",
            cancelClick: "cancelClick.dj.LoginInfo"
        },


        selectors: {
            save: 'a.dc_btn-save',
            reset: 'a.dc_btn-reset',
            cancel: 'a.dc_btn-cancel'
        },

        _initializeEventHandlers: function () {
            this._super();
            var self = this,
            $el = this.$element;

            $(this.selectors.save).live('click', function () {
                if(self.checkLoginInfo())
                {
                    $el.triggerHandler(self.events.saveClick);
                }
            });

            $(this.selectors.reset).live('click', function () {
                $el.triggerHandler(self.events.resetClick);
            });

            $(this.selectors.cancel).live('click', function () {
                $el.triggerHandler(self.events.cancelClick);
            });

            $(this).live('afterSetData', function () {
                $el.triggerHandler(self.events.afterSetData);
            });
        },

        init: function (element, meta) {
            var $meta = $.extend({ name: "LoginInfo" }, meta);
            this._super(element, $meta);
            this.setData(meta.data);
            this.$element.triggerHandler(this.events.afterSetData);

            $('#djUserEmailAddress').focus(function() {
                $("#djConvertEmail").attr('checked', true);
                $("#djUpdateEmail").attr('checked', true);
             });

            $('#djOldPassword, #djNewPassword, #djVerifyPassword').focus(function() {
                $("#djChangePassword").attr('checked', true);
             });
        },

        setData: function (data) {
            // this.$element.html("");
            if (data) {
                this.$element.append(this.templates.logininfo({ logininfo: data }));
            }
        },

        //Validation
        validateEmail: function (str) {
            if ($.trim(str).length > 80) {
                return false;
            }
            else {
                if (str.indexOf("@") == -1 || (str.indexOf("@") != str.lastIndexOf("@")) || str.indexOf(" ") != -1) {
                    return false;
                } else {
                    if (str.indexOf(".") == -1) {
                        return false
                    } else {
                        return true;
                    }
                }
            }
        },

        showError: function (divId, msgToken) {
            $(divId + " span:first-child").html(msgToken);
            $(divId).removeClass("hide");
            $(divId).parent().addClass("error");
        },

        hideErrors : function()
        {
            $('.dj_form-message').each(function (index) {
                $(this).addClass("hide");
                $(this).parent().removeClass("error");
          });
        },

        checkLoginInfo: function () {
            var loginInfo = this.LoginInfoData;
            var email = "";
            this.hideErrors();
            var isConvertEmail = $('#djConvertEmail').is(':checked');  //InputConvertEmail
            loginInfo.IsConvertEmail = isConvertEmail;
            loginInfo.IsConvertEmailOptionOn = ($('#djConvertEmail').length > 0);

            var isUpdateEmail = $('#djUpdateEmail').is(':checked');  //InputUpdateEmail 
            loginInfo.IsUpdateEmail = isUpdateEmail;

            if (isConvertEmail || isUpdateEmail || loginInfo.IsConvertEmailOptionOn) {
                var elEmail = $("#djUserEmailAddress");    //InputUserEmailAddress
                email = $.trim($("#djUserEmailAddress").val());

                if (email == "") {
                    this.showError("#djUserEmailAddress_error", "<%=Token("emptyEmailMessage")%>");
                    elEmail.focus();
                    return false;
                }
                if (!this.validateEmail(email)) {
                    this.showError("#djUserEmailAddress_error", "<%=Token("profValidEmail")%>"); 
                    elEmail.focus();
                    return false;
                }
            }
            loginInfo.Email = email;

            var strErrorMsg = ""; var oldPass = ""; var newPass = ""; var newPassVerify = "";
            var isFocusSet = false;

            var isChangePassword = $('#djChangePassword').is(':checked');  //InputChangePassword 
            loginInfo.IsChangePassword = isChangePassword;

            var passError = false;
            if (isChangePassword) {
                var elOldPass = $("#djOldPassword");    //InputOldPassword
                oldPass = $.trim($("#djOldPassword").val());
                if (oldPass == "" || oldPass.length < 5 || oldPass.length > 32) {
                    this.showError("#djOldPassword_error", "<%=Token("profOldPasswd")%>");
                    passError = true;
                    elOldPass.focus();
                    isFocusSet = true;
                }

                var elNewPass = $("#djNewPassword");    //InputNewPassword
                newPass = $.trim($("#djNewPassword").val());
                if (newPass == "" || newPass.length < 5 || newPass.length > 32) {
                    this.showError("#djNewPassword_error", "<%=Token("profNewPasswd")%>");
                    passError = true;
                    if (!isFocusSet) {
                        elNewPass.focus();
                        isFocusSet = true;
                    }
                }

                var elNewPassVerify = $("#djVerifyPassword");    //InputVerifyPassword
                newPassVerify = $.trim($("#djVerifyPassword").val());
                if (newPassVerify == "" || newPassVerify.length < 5 || newPassVerify.length > 32) {
                    this.showError("#djVerifyPassword_error", "<%=Token("profRepeatPasswd")%>");
                    passError = true;
                    if (!isFocusSet) {
                        elNewPassVerify.focus();
                        isFocusSet = true;
                    }
                }

                if (newPass != "" && newPassVerify != "" && newPass != newPassVerify) {
                    this.showError("#djVerifyPassword_error", "<%=Token("profPasswdDontMatch")%>");
                    passError = true;
                }

                if (passError) {
                    return false;
                }
            }
            loginInfo.OldPass = oldPass;
            loginInfo.NewPass = newPass;
            loginInfo.NewPassVerify = newPassVerify;

            var isAutoLogin = $('#djAutoLogin').is(':checked');  //InputAutoLogin 
            loginInfo.IsAutoLogin = isAutoLogin;

            //SaveLoginInfo(loginInfo);
            return true;
        }
    });


    $.plugin('dj_LoginInfo', DJ.UI.LoginInfo);
