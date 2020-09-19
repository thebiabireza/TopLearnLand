using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
using TopLearnLand_Core.DTOs_ViewModels_;
using TopLearnLand_DataLayer.Entities.User;
using TopLearnLand_DataLayer.Entities.Wallet;

namespace TopLearnLand_Core.Services.InterFaces
{
    public interface IUserService
    {
        int AddUser(User user);
        //void AddUserById(string id);
        bool IsExistUserName(string userName);
        bool IsExistEmail(string Email);
        User LoginUser(LoginViewModels login);
        bool ActiveAccountUser(string activecode);
        User GetUserByUserId(int userId);
        User GetUserDeletedByUserId(int userId);
        List<User> GetUserByUserId(List<int> usersId);
        User GetUserByUserName(string userName);
        User GetUserByEmail(string email);
        User GetUserByActiveCode(string activecode);
        int GetUserIdByUserName(string userName);
        List<User> GetUsers();
        List<string> GetUserRoles(int userId);
        List<int> GetUserRoleId(int userId);
        void UpdateUser(User user);
        void DeleteUser(int userId);
        //void RecoveryUser(int userId);
        void RecoveryUser(User user);
        //void RecoveryUser(List<int> usersId);
        void SaveUserAvatar(string avatarName, IFormFile userAvatar);

        public bool IsUserInCourse(string userName, int courseId);


        #region User Panel Services

        InformationUserViewModel GetUserInformation(string userName);
        InformationUserViewModel GetUserInformation(int userId);
        InformationDeletedUserViewModel GetUserDeletedInformation(int userId);
        SideBarUserPanelViewModel GetSideBarUserPanelData(string userName);
        EditProfileViewModel GetDataForEditProfileUser(string userName);
        void EditUserProfile(string userName, EditProfileViewModel editProfile);
        bool CompareOldPassWord(string userName, string oldPassWord);
        void ChangeUserPassWord(string userName, string newPassWord);

        #endregion

        #region Wallet

        int BalanceUserWallet(string userName);
        List<WalletViewModel> GetWalletUser(string userName);
        int ChargeWallet(string userName, string description, int amount, bool isPay = false);
        int AddWallet(Wallet wallet);
        Wallet GetWalletByWalletId(int walletId);
        void UpdateWallet(Wallet wallet);

        #endregion

        #region Admin Panel

        UsersForAdminPanelViewModels GetUsersForAdminPanel(int pageId, string userName, string email);
        UsersForAdminPanelViewModels GetUsersDeletedForAdminPanel(int pageId, string userName, string email);
        int AddUserFromAdmin(CreateUserViewModel createUser);
        EditUserViewModel GetUserForShowInEditMode(int userId);
        void EditUserFromAdmin(EditUserViewModel editUser);

        #endregion

    }
}
