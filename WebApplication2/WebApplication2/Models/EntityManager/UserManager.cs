﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using WebApplication2.Models.DB;
using WebApplication2.Models.ViewModel;

namespace WebApplication2.Models.EntityManager
{
    public class UserManager
    {
        public void AddUserAccount(UserSignUpView newUser)  // dodanie użytkownika 
        {
            using (ProjektEntities db = new ProjektEntities())
            {
                MD5 md5Hash = MD5.Create();
                USER user = new USER();
                user.User_ID = newUser.Login;
                user.Password = GetMd5Hash(md5Hash, newUser.Password);
                user.e_mail = newUser.Email;
                user.Total_score = 0;
                user.Is_Admin = false;
                user.Is_Exists = true;
                user.Is_Log = true;
                user.Date_join = DateTime.Now;
                USER admin = db.USER.Find("Admin");
                user.Image = admin.Image;
                 db.USER.Add(user);
                db.SaveChanges();
                
            }
        }
        public bool IsLoginExist(string login)    // sprawdzenie czy login istnieje w bazie danych
        {
            using (ProjektEntities db = new ProjektEntities())
            {
                return db.USER.Where(u => u.User_ID.Equals(login)).Any();
            }
        }

        public bool IsUserExists(string login)
        {
            using (ProjektEntities db = new ProjektEntities())
            {
                return db.USER.Where(u => u.User_ID.Equals(login)).Where(u => u.Is_Exists == true).Any();               
            }
        }

        public string GetUserPassword(string login)     //uzyskanie hasła danego użytkownika z bazy danych
        {
            using (ProjektEntities db = new ProjektEntities())
            {
                var user = db.USER.Where(u => u.User_ID.ToLower().Equals(login));
                if (user.Any())
                {
                    return user.FirstOrDefault().Password;
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public string GetLogin(string login)
        {
            using (ProjektEntities db = new ProjektEntities())
            {
                var user = db.USER.Where(o => o.User_ID.Equals(login));
                if (user.Any())
                {
                    return user.FirstOrDefault().User_ID;
                }
            }
            return string.Empty;
        }

        public string GetEmail(string login)
        {
            using (ProjektEntities db = new ProjektEntities())
            {
                var user = db.USER.Where(o => o.User_ID.Equals(login));
                if (user.Any())
                {
                    return user.FirstOrDefault().e_mail;
                }
            }
            return string.Empty;
        }

        public string GetDescription(string login)
        {
            using (ProjektEntities db = new ProjektEntities())
            {
                var user = db.USER.Where(o => o.User_ID.Equals(login));
                if (user.Any())
                {
                    return user.FirstOrDefault().Description;
                }
            }
            return string.Empty;
        }
        public DateTime GetDateJoin(string login)
        {
            using (ProjektEntities db = new ProjektEntities())
            {
                var user = db.USER.Where(o => o.User_ID.Equals(login));
                if (user.Any())
                {
                    return user.FirstOrDefault().Date_join;
                }
            }
            return DateTime.Now;
        }
        public byte[] GetImage(string login)
        {
            using (ProjektEntities db = new ProjektEntities())
            {
                var user = db.USER.Where(o => o.User_ID.Equals(login));
                if (user.Any())
                {
                    return user.FirstOrDefault().Image;
                }
            }
            return null;
        }

        public void ChangeImage(UserSettingView user, string login)
        {
            using (ProjektEntities db = new ProjektEntities())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        USER us = db.USER.Find(login);
                        us.Image = user.imageView.imageData;

                        db.SaveChanges();
                        dbContextTransaction.Commit();
                    }
                    catch
                    {
                        dbContextTransaction.Rollback();
                    }
                }
            }
        }
        public void ChangeEmail(UserSettingView user, string login)
        {
            using (ProjektEntities db = new ProjektEntities())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        USER us = db.USER.Find(login);
                        us.e_mail = user.emailView.newEmail;

                        db.SaveChanges();
                        dbContextTransaction.Commit();
                    }
                    catch
                    {
                        dbContextTransaction.Rollback();
                    }
                }
            }
        }

        public void ChangePassword(UserSettingView user, string login)
        {
            using (ProjektEntities db = new ProjektEntities())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        MD5 md5Hash = MD5.Create();
                        USER us = db.USER.Find(login);
                        us.Password = GetMd5Hash(md5Hash, user.passwordView.Password);

                        db.SaveChanges();
                        dbContextTransaction.Commit();
                    }
                    catch
                    {
                        dbContextTransaction.Rollback();
                    }
                }
            }
        }

        public void ChangeDescription(UserSettingView user, string login)
        {
            using (ProjektEntities db = new ProjektEntities())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        USER us = db.USER.Find(login);
                        us.Description = user.userDescriptionView.description;

                        db.SaveChanges();
                        dbContextTransaction.Commit();
                    }
                    catch
                    {
                        dbContextTransaction.Rollback();
                    }
                }
            }
        }

        public void ChangeDescription(ProfileView user, string login)
        {
            using (ProjektEntities db = new ProjektEntities())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        USER us = db.USER.Find(login);
                        us.Description = user.description;

                        db.SaveChanges();
                        dbContextTransaction.Commit();
                    }
                    catch
                    {
                        dbContextTransaction.Rollback();
                    }
                }
            }
        }


        public void DeleteUser(UserSettingView user, string login)
        {
            using (ProjektEntities db = new ProjektEntities())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        USER us = db.USER.Find(login);
                        us.Is_Exists = false;
                        us.Is_Admin = false;
                        us.Is_Log = false;
                        db.SaveChanges();
                        dbContextTransaction.Commit();
                    }
                    catch
                    {
                        dbContextTransaction.Rollback();
                    }
                }
            }
        }

       public void SetLogIn1(string login)
        {
            using (ProjektEntities db = new ProjektEntities())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        USER us = db.USER.Find(login);
                        us.Is_Log = true;
                        db.SaveChanges();
                        dbContextTransaction.Commit();
                    }
                    catch
                    {
                        dbContextTransaction.Rollback();
                    }
                }
            }
        }

        public void SetLogIn0(string login)
        {
            using (ProjektEntities db = new ProjektEntities())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        USER us = db.USER.Find(login);
                        us.Is_Log = false;
                        db.SaveChanges();
                        dbContextTransaction.Commit();
                    }
                    catch
                    {
                        dbContextTransaction.Rollback();
                    }
                }
            }
        }

       
        public static string GetMd5Hash(MD5 md5Hash, string input)
        {
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }
    }

}