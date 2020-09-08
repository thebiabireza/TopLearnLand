using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TopLearn.Core.Security;
using TopLearnLand_Core.Convertors;
using TopLearnLand_Core.DTOs_ViewModels_;
using TopLearnLand_Core.Generator;
using TopLearnLand_Core.Services.InterFaces;
using TopLearnLand_DataLayer.Context;
using TopLearnLand_DataLayer.Entities.User;
using TopLearnLand_DataLayer.Entities.Wallet;

namespace TopLearnLand_Core.Services
{
    public class UserService : IUserService
    {
        private TopLearnLandContext _DBcontext;
        public UserService(TopLearnLandContext context)
        {
            _DBcontext = context;
        }

        #region Manage User Account
        public bool ActiveAccountUser(string activecode)
        {
            var user = _DBcontext.Users.SingleOrDefault(a => a.ActiveCode == activecode);
            if (user == null || user.IsActive == false)
                return false;

            user.IsActive = true;
            string newActiveCode = NameGenerator.GenericUniqCode();
            user.ActiveCode = newActiveCode;
            _DBcontext.SaveChanges();

            return true;
        }

        public User GetUserByUserId(int userId)
        {
            return _DBcontext.Users.Find(userId);
        }

        public User GetUserDeletedByUserId(int userId)
        {
            return _DBcontext.Users.IgnoreQueryFilters()
                .Single(user => user.UserId == userId && user.IsDeleted == true);
        }

        public List<User> GetUserByUserId(List<int> usersId)
        {
            List<User> users = new List<User>();
            foreach (var userId in usersId)
            {
                users = _DBcontext.Users.Where(user => user.UserId == userId).ToList();
            }

            return users;
        }

        public User GetUserByUserName(string userName)
        {
            return _DBcontext.Users.SingleOrDefault(user => user.UserName == userName);
        }

        public int AddUser(User user)
        {

            _DBcontext.Users.Add(user);
            _DBcontext.SaveChanges();

            return user.UserId;
        }

        public User GetUserByActiveCode(string activeCode)
        {
            return _DBcontext.Users.SingleOrDefault(u => u.ActiveCode == activeCode);
        }

        public int GetUserIdByUserName(string userName)
        {

            return GetUserByUserName(userName).UserId;
        }

        public List<User> GetUsers()
        {
            return _DBcontext.Users.ToList();
        }

        public List<string> GetUserRoles(int userId)
        {
            return _DBcontext.UserRoles.Include(role => role.Roles)
                .Where(userRole => userRole.UserId == userId)
                .Select(ur => ur.Roles.RoleTitle).ToList();
        }

        public List<int> GetUserRoleId(int userId)
        {
            var userRole = _DBcontext.UserRoles.Include(role => role.Roles)
                .Where(userRole => userRole.UserId == userId)
                .Select(ur => ur.RoleId).ToList();

            return userRole;
        }

        public User GetUserByEmail(string email)
        {
            return _DBcontext.Users.SingleOrDefault(u => u.Email == email);
        }

        public bool IsExistEmail(string email)
        {
            return _DBcontext.Users.Any(u => u.Email == email);
        }

        public bool IsExistUserName(string userName)
        {
            return _DBcontext.Users.Any(u => u.UserName == userName);
        }

        public User LoginUser(LoginViewModels login)
        {
            string FixedEmail = FixedTexts.FixedEmail(login.Email);
            string HashedPassword = PasswordHelper.EncodePasswordMd5(login.Password);
            return _DBcontext.Users.SingleOrDefault(login => login.Email == FixedEmail && login.Password == HashedPassword);
        }

        public void UpdateUser(User user)
        {
            _DBcontext.Update(user);
            _DBcontext.SaveChanges();

        }

        public void DeleteUser(int userId)
        {
            User user = GetUserByUserId(userId);
            user.IsDeleted = true;
            UpdateUser(user);
        }

        public void RecoveryUser(User user)
        {
            //User user = GetUserByUserId(userId);
            user.IsDeleted = false;
            UpdateUser(user);
        }

        //public void RecoveryUser(List<int> usersId)
        //{
        //    List<User> users = GetUserByUserId(usersId);
        //    foreach (var user in users)
        //    {
        //        user.IsDeleted = false;
        //        UpdateUser(user);
        //    }
        //}

        public void SaveUserAvatar(string avatarName, IFormFile userAvatar)
        {
            avatarName = NameGenerator.GenericUniqCode() + Path.GetExtension(userAvatar.FileName);
            string imageNewPath = Path.Combine(Directory.GetCurrentDirectory(),
                "wwwroot/UserAvatar", avatarName);

            using (var stream = new FileStream(imageNewPath, FileMode.Create))
            {
                userAvatar.CopyTo(stream);
            }
        }

        #endregion

        #region User Panel

        public InformationUserViewModel GetUserInformation(string userName)
        {
            User user = GetUserByUserName(userName);

            InformationUserViewModel informationUser = new InformationUserViewModel();
            informationUser.UserName = user.UserName;
            informationUser.Email = user.Email;
            informationUser.RegisterDate = user.RegisterDate;
            informationUser.Wallet = BalanceUserWallet(userName);
            informationUser.userRoles = GetUserRoles(user.UserId);

            return informationUser;
        }

        public InformationUserViewModel GetUserInformation(int userId)
        {
            User user = GetUserByUserId(userId);

            InformationUserViewModel informationUser = new InformationUserViewModel
            {
                UserName = user.UserName,
                Email = user.Email,
                RegisterDate = user.RegisterDate,
                Wallet = BalanceUserWallet(user.UserName)
            };

            return informationUser;
        }

        public InformationDeletedUserViewModel GetUserDeletedInformation(int userId)
        {
            User user = GetUserByUserId(userId);
            InformationDeletedUserViewModel informationDeletedUser = new InformationDeletedUserViewModel();
            informationDeletedUser.UserId = user.UserId;
            informationDeletedUser.UserName = user.UserName;
            informationDeletedUser.Email = user.Email;
            informationDeletedUser.RegisterDate = user.RegisterDate;

            return informationDeletedUser;
        }

        public SideBarUserPanelViewModel GetSideBarUserPanelData(string userName)
        {
            return _DBcontext.Users.Where(user => user.UserName == userName).Select(user => new SideBarUserPanelViewModel()
            {
                UserName = user.UserName,
                ImageName = user.UserAvatar,
                RegisterDate = user.RegisterDate

            }).Single();
        }

        public EditProfileViewModel GetDataForEditProfileUser(string userName)
        {
            return _DBcontext.Users.Where(u => u.UserName == userName)
                .Select(user => new EditProfileViewModel()
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    AvatarName = user.UserAvatar,

                }).Single();
        }

        public void EditUserProfile(string userName, EditProfileViewModel editProfile)
        {
            if (editProfile.UserAvatar != null)
            {
                if (editProfile.AvatarName != "Defult.jpg")
                {
                    string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar",
                        editProfile.AvatarName);
                    if (File.Exists(imagePath))
                    {
                        File.Delete(imagePath);
                    }
                }
                SaveUserAvatar(editProfile.AvatarName, editProfile.UserAvatar);
            }
            var user = GetUserByUserName(userName);
            user.UserName = editProfile.UserName;
            user.Email = editProfile.Email;
            user.UserAvatar = editProfile.AvatarName;

            UpdateUser(user);
        }

        public bool CompareOldPassWord(string userName, string oldPassWord)
        {
            string hashOldPassWord = PasswordHelper.EncodePasswordMd5(oldPassWord);
            return _DBcontext.Users.Any(u => u.UserName == userName && u.Password == hashOldPassWord);
        }

        public void ChangeUserPassWord(string userName, string newPassWord)
        {
            var user = GetUserByUserName(userName);
            user.Password = PasswordHelper.EncodePasswordMd5(newPassWord);
            UpdateUser(user);
        }

        #endregion

        #region Wallet

        public int BalanceUserWallet(string userName)
        {
            User user = GetUserByUserName(userName);

            var deposit = _DBcontext.Wallets
                .Where(w => w.UserId == user.UserId && w.IsPay && w.WalletTypeId == 1)
                .Select(w => w.Amount).ToList();

            var harvest = _DBcontext.Wallets
                .Where(w => w.UserId == user.UserId && w.WalletTypeId == 2)
                .Select(w => w.Amount).ToList();

            return (deposit.Sum() - harvest.Sum());
        }

        public List<WalletViewModel> GetWalletUser(string userName)
        {
            var userId = GetUserIdByUserName(userName);

            var wallet = _DBcontext.Wallets
                .Where(w => w.UserId == userId && w.IsPay)
                .Select(wallet => new WalletViewModel()
                {
                    Amount = wallet.Amount,
                    DateCharge = wallet.CreateDate,
                    WalletTypeId = wallet.WalletTypeId,
                    Description = wallet.Description
                }).ToList();

            return wallet;
        }

        public int ChargeWallet(string userName, string description, int amount, bool isPay = false)
        {
            Wallet wallet = new Wallet()
            {
                Amount = amount,
                CreateDate = DateTime.Now,
                IsPay = isPay,
                WalletTypeId = 1,
                UserId = GetUserIdByUserName(userName),
                Description = description,
            };

            return AddWallet(wallet);
        }

        public int AddWallet(Wallet wallet)
        {
            _DBcontext.Wallets.Add(wallet);
            _DBcontext.SaveChanges();

            return wallet.WalletId;
        }

        public Wallet GetWalletByWalletId(int walletId)
        {
            return _DBcontext.Wallets.Find(walletId);
        }

        public void UpdateWallet(Wallet wallet)
        {
            _DBcontext.Wallets.Update(wallet);
            _DBcontext.SaveChanges();
        }

        public UsersForAdminPanelViewModels GetUsersForAdminPanel(int pageId = 1, string userName = "", string email = "")
        {
            IQueryable<User> users = _DBcontext.Users;

            if (!string.IsNullOrEmpty(userName))
            {
                users = users.Where(u => u.UserName.Contains(userName));
            }

            if (!string.IsNullOrEmpty(email))
            {
                users = users.Where(u => u.Email.Contains(email));
            }

            #region Show Item In Page

            int take = 20;
            int skip = (pageId - 1) * take;

            UsersForAdminPanelViewModels list = new UsersForAdminPanelViewModels();
            list.Users = users.OrderBy(u => u.RegisterDate).Skip(skip).Take(take).ToList();
            list.UsersCount = GetUsers().Count;
            list.PageCount = (users.Count() / take);
            list.CurrentPage = pageId;

            #endregion

            return list;
        }

        public UsersForAdminPanelViewModels GetUsersDeletedForAdminPanel(int pageId, string userName, string email)
        {
            IQueryable<User> users = _DBcontext.Users.IgnoreQueryFilters().Where(u => u.IsDeleted);

            if (!string.IsNullOrEmpty(userName))
            {
                users = users.Where(u => u.UserName.Contains(userName));
            }

            if (!string.IsNullOrEmpty(email))
            {
                users = users.Where(u => u.Email.Contains(email));
            }

            #region Show Item In Page

            int take = 20;
            int skip = (pageId - 1) * take;

            UsersForAdminPanelViewModels list = new UsersForAdminPanelViewModels();
            list.Users = users.OrderBy(u => u.RegisterDate).Skip(skip).Take(take).ToList();
            list.UsersCount = GetUsers().Count;
            list.PageCount = (users.Count() / take);
            list.CurrentPage = pageId;

            #endregion

            return list;
        }

        public int AddUserFromAdmin(CreateUserViewModel createUser)
        {
            var user = new User
            {
                Password = PasswordHelper.EncodePasswordMd5(createUser.Password),
                UserName = createUser.UserName,
                Email = createUser.Email,
                ActiveCode = NameGenerator.GenericUniqCode(),
                RegisterDate = DateTime.Now,
                IsActive = createUser.IsActive
            };
            //TODO badan khodet az admin bepors(yani tu modelet filde IsActive bezar, age admin tik zad ke hich, age nazad bia metodi bezar k acountesho ba email active kone user)

            #region Add UserAvatar

            if (createUser.UserAvatar != null)
            {
                SaveUserAvatar(user.UserAvatar, createUser.UserAvatar);
            }

            #endregion

            return AddUser(user);
        }

        public EditUserViewModel GetUserForShowInEditMode(int userId)
        {
            return _DBcontext.Users.Where(u => u.UserId == userId)
                .Select(user => new EditUserViewModel()
                {
                    UserId = user.UserId,
                    UserName = user.UserName,
                    Password = user.Password,
                    Email = user.Email,
                    AvatarName = user.UserAvatar,
                    IsActive = user.IsActive,
                    UserRoles = user.UserRoles.Select(role => role.RoleId).ToList()
                }).Single();
        }

        public void EditUserFromAdmin(EditUserViewModel editUser)
        {
            User user = GetUserByUserId(editUser.UserId);
            if (editUser.Password != user.Password)
            {
                user.Password = PasswordHelper.EncodePasswordMd5(editUser.Password);
            }
            user.Email = editUser.Email;
            user.IsActive = editUser.IsActive;

            #region Edit User Avatar

            if (editUser.UserAvatar != null)
            {
                #region Delete Old ImageAvatar

                if (editUser.AvatarName != "Defult.jpg")
                {
                    string imageDeletePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar",
                        editUser.AvatarName);
                    if (File.Exists(imageDeletePath))
                    {
                        File.Delete(imageDeletePath);
                    }
                }

                #endregion

                #region Save New ImageAvatar

                SaveUserAvatar(editUser.AvatarName, editUser.UserAvatar);

                #endregion
            }

            #endregion

            UpdateUser(user);
        }

        #endregion
    }
}
