using Dapper;
using Esim7.Models.PowerManageModel;
using Esim7.parameter.PowerManage;
using Esim7.UNity;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;


/// <summary>
/// 权限管理模块方法
/// </summary>
namespace Esim7.Action
{
    public class PowerManageAction
    {
        ///<summary>
        ///用户添加子用户
        /// </summary>
        /// <returns></returns>
        //public cf_userInfo AddUser(AddUserPara para)
        //{
        //    //cf_userInfo users = new cf_userInfo();
        //    string CompanyID = Unit.GetTimeStamp(DateTime.Now);
        //    cf_userInfo us = new cf_userInfo();
        //    string sql = "select * from cf_user where status=1 and Company_ID=" + para.Company_ID;
        //    int UserId = 0;
        //    string sqllist = "select LoginName from cf_user where status=1";
        //    string s = string.Empty;          
        //    s = Unit.MD5_64("123456");
        //    string Num = Unit.GetTimeStamp(DateTime.Now);
        //    try
        //    {
        //        using (IDbConnection conn = DapperService.MySqlConnection())
        //        {
        //            var unamelist = conn.Query<cf_user>(sqllist).ToList();
        //            var unlist = unamelist.Select(t => t.LoginName).ToList();
        //            var ulist = conn.Query<cf_user>(sql).ToList();
        //            if (unlist.Contains(para.LoginName))//是否存在相同的名称
        //            {
        //                us.MSg = "用户名称已存在，请重新输入用户名称！";
        //                us.flg = "0";
        //            }
        //            else
        //            {
        //                foreach (var item in ulist)
        //                {
        //                    UserId = item.id;                            
        //                    string addusersql = "insert into cf_user(User_Pid,LoginName,Loginpassword,Company_ID,UserID,status)values(" + UserId + ",'" + para.LoginName + "','" + s + "','" + CompanyID + "','" + Num + "',"+1+")";
        //                    string addcompanysql = "insert into company(CompanyID,CompanyName,Companyremarks,CompanyOpeningDate,status,CompanyPhone,CompanyAdress)" +
        //                        "values(" + CompanyID + ",'" + para.CompanyName + "','" + para.CompanyRemark + "','" + DateTime.Now + "'," + 1 + ",'" + para.CompanyPhone + "','"+para.CompanyAddress+"')";
        //                    var Result = conn.Execute(addusersql);
        //                    var Result1 = conn.Execute(addcompanysql);
        //                    us.MSg = "添加用户成功！";                            
        //                    us.flg = "1";
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        us.MSg = "错误" + ex;
        //        us.flg = "-1";
        //    } 
        //    return us;
        //}

        ///<summary>
        ///查看子用户列表
        /// </summary>
        public cf_userInfo ListUserInfo(string Company_ID,string CompanyName)
        {
            cf_userInfo userInfo = new cf_userInfo();
            List<cf_user> user = new List<cf_user>();
            string s = string.Empty;
            //s = " limit " + (PagNumber - 1) * Num + "," + Num;
            string sqllist = "select id from cf_user where Company_ID='" + Company_ID + "'";
            try
            {
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    int id = conn.Query<cf_user>(sqllist).Select(t => t.id).FirstOrDefault();
                    string sqluserlist = "select t1.id,t1.LoginName,t1.status, t1.Company_ID,t2.CompanyName,t2.CompanyPhone,t2.CompanyAdress,t2.Companyremarks " +
                       "from cf_user t1 left join company t2 on t1.Company_ID = t2.CompanyID  where User_Pid=" + id + "";
                    user = conn.Query<cf_user>(sqluserlist).ToList();
                    if (!string.IsNullOrWhiteSpace(CompanyName))
                    {
                        user = user.Where(t => t.CompanyName.Contains(CompanyName)).ToList();
                    }
                    string sqlcount = "select * from cf_user where User_Pid=" + id + "";
                    userInfo.cf_Users = user;
                    userInfo.RowNum = user.Count;//conn.Query<cf_user>(sqlcount).Count();
                    userInfo.flg = "1";
                    userInfo.MSg = "查询成功！";
                }
            }
            catch (Exception ex)
            {
                userInfo.flg = "-1";
                userInfo.MSg = "查询失败"+ex;
            }           
                return userInfo;
        }

        ///<summary>
        ///用户修改密码
        /// </summary>
        public cf_userInfo UpdateUserPwd(string OldPassword,string NewPassWord, string Company_ID)
        {
            cf_userInfo userInfo = new cf_userInfo();
            List<cf_user> user = new List<cf_user>();
            string OldPwd= Unit.MD5_64(OldPassword);
            string sqllist = "select Loginpassword from cf_user where Company_ID='" + Company_ID + "'";
            try
            {
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    string OldPasswords = conn.Query<cf_user>(sqllist).Select(t => t.Loginpassword).FirstOrDefault();
                    if (OldPwd != OldPasswords)
                    {
                        userInfo.flg = "0";
                        userInfo.MSg = "旧密码输入错误，请核对后在输入！";
                    }
                    else
                    {                       
                        NewPassWord = Unit.MD5_64(NewPassWord);
                        string updatepwd = "update cf_user set Loginpassword='" + NewPassWord + "' where Company_ID='" + Company_ID + "'";
                        var Result = conn.Execute(updatepwd);
                        userInfo.flg = "1";
                        userInfo.MSg = "修改密码成功！";
                    }                   
                }
            }
            catch (Exception ex)
            {
                userInfo.flg = "-1";
                userInfo.MSg = "修改失败" + ex;
            }
            return userInfo;
        }

        ///<summary>
        ///设置子用户的启用禁用状态
        /// </summary>
        public cf_userInfo UpdateStatus(int status,int id)
        {
            cf_userInfo userInfo = new cf_userInfo();
            List<cf_user> user = new List<cf_user>();
            try
            {
                using (IDbConnection conn = DapperService.MySqlConnection())
                {                    
                    string updatepwd = "update cf_user set status='" + status + "' where id=" +id + " or User_Pid="+id+"";
                    var Result = conn.Execute(updatepwd);
                    userInfo.flg = "1";
                    userInfo.MSg = "设置状态成功！"; 
                }
            }
            catch (Exception ex)
            {
                userInfo.flg = "-1";
                userInfo.MSg = "设置状态失败" + ex;
            }
            return userInfo;
        }

        ///<summary>
        ///删除子用户
        /// </summary>
        public Message DeleUser(int id)
        {
            Message m = new Message();
            try
            {
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    string sqluserid = "select Company_ID from cf_user where id="+id+"";
                    string Company_ID = conn.Query<cf_user>(sqluserid).Select(t => t.Company_ID).FirstOrDefault();
                    string sqldeluser = "update  cf_user set status=-1 where id=" + id+"";//修改用户表状态
                    string sqldelcompany = "update  company set status=0 where CompanyID='" + Company_ID+"'";//修改公司信息状态
                    var result = conn.Execute(sqldeluser);
                    var result1 = conn.Execute(sqldelcompany);
                    m.flg = "1";
                    m.Msg = "删除成功!";
                }
            }
            catch (Exception ex)
            {
                m.flg = "-1";
                m.Msg = "删除失败"+ex;
            }
            return m;
        }
        #region  菜单管理模块
        ///<summary>
        ///添加菜单信息
        /// </summary>
        public fk_menuInfo AddMenu(AddMenuPara para)
        {
            fk_menuInfo fk = new fk_menuInfo();
            try
            {
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    string sqllist = "select id from fk_menu";
                    int id = conn.Query<fk_menu>(sqllist).Select(t=>t.id).LastOrDefault();
                    int ids = id + 1;
                    int Type = 0;
                    string idss = ids.ToString();
                    string MenuId = "000";
                    MenuId = MenuId.Insert(2, idss);
                    if (para.Menu_FatherID == 0)
                    {
                        Type = 1;
                    }
                    else
                    {
                        Type = 2;
                    }
                    string sqladdmenu = "insert into fk_menu(MenuID,MenuName,MenuURL,Menu_FatherID,Type) values('" + MenuId + "','" + para.MenuName + "','" + para.MenuURL + "'," + para.Menu_FatherID + ","+ Type+")";
                    var Resule = conn.Execute(sqladdmenu);
                    fk.flg = "1";
                    fk.Msg = "添加菜单成功！";
                }                   
            }
            catch (Exception ex)
            {
                fk.flg = "-1";
                fk.Msg = "添加菜单失败!"+ex;
            }
            return fk;
        }

        ///<summary>
        ///编辑菜单信息
        /// </summary>
        public fk_menuInfo UpdateMenu(EditMenuPara para)
        {
            string sqlupdatemenu = string.Empty;
            fk_menuInfo fk = new fk_menuInfo();
            int Type = 0;
            if (para.Menu_FatherID == 0)
            {
                Type = 1;
            }
            else
            {
                Type = 2;
            }
            try
            {
                using (IDbConnection conn = DapperService.MySqlConnection())
                {                  
                    sqlupdatemenu = "update fk_menu set MenuName='" + para.MenuName + "',MenuURL='" + para.MenuURL + "',Type='"+Type+ "',Menu_FatherID='"+para.Menu_FatherID+"' where id=" + para.id + "";                
                    var Resule = conn.Execute(sqlupdatemenu);
                    fk.flg = "1";
                    fk.Msg = "编辑菜单成功！";
                }
            }
            catch (Exception ex)
            {
                fk.flg = "-1";
                fk.Msg = "编辑菜单失败!" + ex;
            }
            return fk; 
        }

        ///<summary>
        ///设置菜单的启用禁用状态
        /// </summary>
        public Message EditMenuStatus(int id,int status)
        {
            Message mes = new Message();
            string sqleditstatus = string.Empty;
            try
            {
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    string listtype = "select Type from fk_menu where id=" +id + "";
                    int Type = conn.Query<fk_menu>(listtype).Select(t => t.Type).FirstOrDefault();
                    if (Type == 1)
                    {
                        sqleditstatus = "update fk_menu set Status=" + status + " where id=" + id + " or Menu_FatherID="+id+"";
                        var Result = conn.Execute(sqleditstatus);
                    }
                    else
                    {
                        sqleditstatus = "update fk_menu set Status=" + status + " where id=" + id + "";
                        var Result = conn.Execute(sqleditstatus);
                    }                    
                    mes.flg = "1";
                    mes.Msg = "菜单设置成功";
                }
            }
            catch (Exception ex)
            {
                mes.flg = "-1";
                mes.Msg = "菜单设置失败"+ex;
            }
            return mes;
        }

        ///<summary>
        ///显示顶级菜单id和名称
        /// </summary>
        public OneMenuInfo ListOneMenuInfo()
        {
            List<OneMenu> oneMenus = new List<OneMenu>();
            OneMenuInfo menus = new OneMenuInfo();           
            try
            {
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    OneMenu one = new OneMenu();
                    one.Id = 0;
                    one.MenuName = "顶级菜单";                    
                    string sqlmenu = "select id,MenuName from fk_menu where Menu_FatherID=" + 0 + " and Status="+1+"";                    
                    oneMenus = conn.Query<OneMenu>(sqlmenu).ToList();
                    oneMenus.Insert(0,one);
                    menus.oneMenus = oneMenus;
                    menus.flg = "1";
                    menus.Msg = "查询成功!";
                }
            }
            catch (Exception ex)
            {
                menus.flg = "-1";
                menus.Msg = "查询失败" + ex;
            }
            return menus;
        }

        ///<summary>
        ///显示菜单信息  树形结构
        /// </summary>
        public string ListMenuInfo(string Company_ID,string userCompany_ID)
        {
            int[] a=new int[] { };//全选中的
            int[] b = new int[] { };//半选中的
            string strJson = string.Empty;
            TreelikeMenu info = new TreelikeMenu();            
            List<TreeMenuInfo> TreeMenuInfos = new List<TreeMenuInfo>();       
            try
            {
                if (Company_ID == "1556265186243" && string.IsNullOrWhiteSpace(userCompany_ID))
                {
                    using (IDbConnection conn = DapperService.MySqlConnection())
                    {
                        string sqllist = "select id,MenuID,MenuName as lable,Menu_FatherID,Type,MenuURL from fk_menu where Status=" + 1 + " and Type=" + 1 + "";
                        var onemenu = conn.Query<fk_menu>(sqllist).ToList();
                        string alllist = "select id,MenuID,MenuName as lable,Menu_FatherID,Type,MenuURL from fk_menu where Status=" + 1 + "";
                        var All = conn.Query<fk_menu>(alllist).ToList();
                        foreach (var item in onemenu)
                        {
                            List<nemuinfo> fk_Menus = new List<nemuinfo>();
                            TreeMenuInfo treeMenu = new TreeMenuInfo();

                            int id = item.id;
                            //treeMenu.Type = item.Type;
                            treeMenu.lable = item.lable;
                            treeMenu.id = item.id;
                            var TwoMenu = All.Where(t => t.Menu_FatherID == item.id).ToList();
                            foreach (var items in TwoMenu)
                            {
                                nemuinfo fk = new nemuinfo();
                                fk.lable = items.lable;
                                //fk.Type = items.Type;
                                fk.id = items.id;
                                fk_Menus.Add(fk);
                            }
                            treeMenu.children = fk_Menus;
                            TreeMenuInfos.Add(treeMenu);
                        }
                        //strJson = JsonConvert.SerializeObject(TreeMenuInfos);
                        info.key = a;
                        info.key1 = b;
                        info.treeMenuInfos = TreeMenuInfos;                       
                        info.flg = "1";
                        info.Msg = "查询成功!";
                        strJson = JsonConvert.SerializeObject(info);
                    }
                }
                if (Company_ID == "1556265186243" && !string.IsNullOrWhiteSpace(userCompany_ID))
                {
                    using (IDbConnection conn = DapperService.MySqlConnection())
                    {
                        string userMenuSql = "select User_Menu from cf_user where Company_ID='"+userCompany_ID+"'";
                        string userMenu = conn.Query<cf_user>(userMenuSql).Select(t => t.User_Menu).FirstOrDefault();
                        if (!string.IsNullOrWhiteSpace(userMenu))
                        {
                            string[] UserMenus = userMenu.Split(',');
                            string pidstr = string.Empty;
                            for (int num = 0; num < UserMenus.Length; num++)
                            {
                                string sqlpidmenuid = "select Menu_FatherID from fk_menu where id=" + UserMenus[num] + "";
                                int Pid = conn.Query<fk_menu>(sqlpidmenuid).Select(t => t.Menu_FatherID).FirstOrDefault();                                
                                if (Pid != 0)
                                {
                                    int id = Array.IndexOf(UserMenus, Pid.ToString()); // 这里的pid就是你要查找的值
                                    if (id == -1)//不存在
                                    {
                                        pidstr += Pid.ToString()+",";                                        
                                    }
                                }
                            }
                            a = new int[UserMenus.Length];
                            for (int i = 0; i < UserMenus.Length; i++)
                            {
                                int j = Convert.ToInt32(UserMenus[i]);                               
                                a[i] = j;
                            }

                            if (!string.IsNullOrWhiteSpace(pidstr))
                            {
                                pidstr = pidstr.Substring(0,pidstr.Length-1);
                                string[] noselect = pidstr.Split(',');
                                b = new int[noselect.Length];
                                for (int s = 0; s < noselect.Length; s++)
                                {
                                    int r = Convert.ToInt32(noselect[s]);
                                    b[s] = r;
                                }
                            }
                        }
                        string sqllist = "select id,MenuID,MenuName as lable,Menu_FatherID,Type,MenuURL from fk_menu where Status=" + 1 + " and Type=" + 1 + "";
                        var onemenu = conn.Query<fk_menu>(sqllist).ToList();
                        string alllist = "select id,MenuID,MenuName as lable,Menu_FatherID,Type,MenuURL from fk_menu where Status=" + 1 + "";
                        var All = conn.Query<fk_menu>(alllist).ToList();
                        foreach (var item in onemenu)
                        {
                            List<nemuinfo> fk_Menus = new List<nemuinfo>();
                            TreeMenuInfo treeMenu = new TreeMenuInfo();

                            int id = item.id;
                            if (id == 5)
                            {
                                treeMenu.disabled = true;
                            }
                            //treeMenu.Type = item.Type;
                            treeMenu.lable = item.lable;
                            treeMenu.id = item.id;
                            var TwoMenu = All.Where(t => t.Menu_FatherID == item.id).ToList();
                            foreach (var items in TwoMenu)
                            {
                                nemuinfo fk = new nemuinfo();
                                fk.lable = items.lable;
                                if (items.id==12 || items.id == 13 || items.id == 14 || items.id == 15 || items.id == 16)
                                {
                                    fk.disabled = true;
                                }
                                //fk.Type = items.Type;
                                fk.id = items.id;
                                fk_Menus.Add(fk);
                            }
                            treeMenu.children = fk_Menus;
                            TreeMenuInfos.Add(treeMenu);
                        }
                       // strJson = JsonConvert.SerializeObject(TreeMenuInfos);
                        info.key = a;
                        info.key1 = b;
                        info.treeMenuInfos = TreeMenuInfos;                        
                        info.flg = "1";
                        info.Msg = "查询成功!";
                        strJson = JsonConvert.SerializeObject(info);
                    }
                       
                }
                if (Company_ID!= "1556265186243" && !string.IsNullOrWhiteSpace(userCompany_ID))
                {
                    using (IDbConnection conn = DapperService.MySqlConnection())
                    {
                        string userMenuSql = "select User_Menu from cf_user where Company_ID='" + userCompany_ID + "'";
                        string userMenu = conn.Query<cf_user>(userMenuSql).Select(t => t.User_Menu).FirstOrDefault();
                        string sqluserinfo = "select User_Menu from cf_user where Company_ID='" + Company_ID + "'";
                        string User_Menu = conn.Query<cf_user>(sqluserinfo).Select(t => t.User_Menu).FirstOrDefault();
                        if (!string.IsNullOrWhiteSpace(userMenu))
                        {
                            string[] UserMenus = userMenu.Split(',');
                            string pidstr = string.Empty;
                            for (int num = 0; num < UserMenus.Length; num++)
                            {
                                string sqlpidmenuid = "select Menu_FatherID from fk_menu where id=" + UserMenus[num] + "";
                                int Pid = conn.Query<fk_menu>(sqlpidmenuid).Select(t => t.Menu_FatherID).FirstOrDefault();
                                if (Pid != 0)
                                {
                                    int id = Array.IndexOf(UserMenus, Pid.ToString()); // 这里的pid就是你要查找的值
                                    if (id == -1)//不存在
                                    {
                                        pidstr += Pid.ToString() + ",";
                                    }
                                }
                            }
                            string[] User_Menus = User_Menu.Split(',');
                            for (int s = 0; s < User_Menus.Length; s++)
                            {
                                string sqlpidmenuid = "select Menu_FatherID from fk_menu where id=" + User_Menus[s] + "";
                                int Pids = conn.Query<fk_menu>(sqlpidmenuid).Select(t => t.Menu_FatherID).FirstOrDefault();
                                if (Pids != 0)
                                {
                                    int id = Array.IndexOf(User_Menus, Pids.ToString()); // 这里的pid就是你要查找的值
                                    if (id == -1)//不存在
                                    {
                                        User_Menu = User_Menu + ",";
                                        User_Menu += Pids.ToString();
                                        
                                    }
                                }
                            }
                            //User_Menu = User_Menu.Substring(0, User_Menu.Length - 1);
                            a = new int[UserMenus.Length];
                            for (int i = 0; i < UserMenus.Length; i++)
                            {
                                int j = Convert.ToInt32(UserMenus[i]);
                                a[i] = j;
                            }

                            if (!string.IsNullOrWhiteSpace(pidstr))
                            {
                                pidstr = pidstr.Substring(0, pidstr.Length - 1);
                                string[] noselect = pidstr.Split(',');
                                b = new int[noselect.Length];
                                for (int s = 0; s < noselect.Length; s++)
                                {
                                    int r = Convert.ToInt32(noselect[s]);
                                    b[s] = r;
                                }
                            }
                        }
                        string sqlMenuList = "select id,MenuID,MenuName as lable,Menu_FatherID,Type,MenuURL from fk_menu where id IN(" + User_Menu + ")";
                        var MenuList = conn.Query<fk_menu>(sqlMenuList).ToList();
                        var MenuListOne = MenuList.Where(t => t.Type == 1);
                        foreach (var item in MenuListOne)
                        {
                            List<nemuinfo> fk_Menus = new List<nemuinfo>();
                            TreeMenuInfo treeMenu = new TreeMenuInfo();
                            int Fid = item.Menu_FatherID;
                            int id = item.id;
                            treeMenu.id = item.id;
                            treeMenu.lable = item.lable;
                            //treeMenu.Type = item.Type;
                            var TwoMenu = MenuList.Where(t => t.Menu_FatherID == item.id).ToList();
                            foreach (var items in TwoMenu)
                            {
                                nemuinfo fk = new nemuinfo();
                                fk.lable = items.lable;
                                fk.id = items.id;
                                fk_Menus.Add(fk);
                            }

                            treeMenu.children = fk_Menus;
                            TreeMenuInfos.Add(treeMenu);
                        }
                        info.treeMenuInfos = TreeMenuInfos;
                        info.key = a;
                        info.key1 = b;
                        info.flg = "1";
                        info.Msg = "查询成功!";
                        strJson = JsonConvert.SerializeObject(info);
                    }
                }
                if (Company_ID != "1556265186243" && string.IsNullOrWhiteSpace(userCompany_ID))
                {
                    using (IDbConnection conn = DapperService.MySqlConnection())
                    {
                        string sqluserinfo = "select User_Menu from cf_user where Company_ID='" + Company_ID + "'";
                        string User_Menu = conn.Query<cf_user>(sqluserinfo).Select(t => t.User_Menu).FirstOrDefault();
                        string sqlMenuList = "select id,MenuID,MenuName as lable,Menu_FatherID,Type,MenuURL from fk_menu where id IN(" + User_Menu + ")";
                        var MenuList = conn.Query<fk_menu>(sqlMenuList).ToList();
                        var MenuListOne = MenuList.Where(t => t.Type == 1);
                        foreach (var item in MenuListOne)
                        {
                            List<nemuinfo> fk_Menus = new List<nemuinfo>();
                            TreeMenuInfo treeMenu = new TreeMenuInfo();
                            int Fid = item.Menu_FatherID;
                            int id = item.id;
                            treeMenu.id = item.id;
                            treeMenu.lable = item.lable;
                            //treeMenu.Type = item.Type;
                            var TwoMenu = MenuList.Where(t => t.Menu_FatherID == item.id).ToList();
                            foreach (var items in TwoMenu)
                            {
                                nemuinfo fk = new nemuinfo();
                                fk.lable = items.lable;
                                fk.id = items.id;
                                fk_Menus.Add(fk);
                            }

                            treeMenu.children = fk_Menus;
                            TreeMenuInfos.Add(treeMenu);
                        }                        
                        info.treeMenuInfos = TreeMenuInfos;
                        info.key = a;
                        info.key1 = b;
                        info.flg = "1";
                        info.Msg = "查询成功!";
                        strJson = JsonConvert.SerializeObject(info);
                    }
                }
                                               
            }
            catch (Exception ex)
            {
                info.flg = "-1";
                info.Msg = "查询失败"+ex;
            }
            return strJson;
        }
        #endregion

        #region  分配管理
        /// <summary>
        /// 用户给子级用户分配卡
        /// </summary>
        /// <param name="para">公司编号 分配卡的公司编号 卡信息集合</param>
        /// <returns></returns>
        public Message CardAssign(AssignCardPara para)
        {
            Message meg = new Message();
            string UserCompanyId = para.UserCompanyId;
            string CompanyId = para.CompanyId;
            try
            {
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    foreach (var item in para.cardInfos)
                    {
                        string AddCopy1 = "insert into card_copy1 (Card_ICCID,Card_CompanyID) values('"+item.Card_ICCID+"','"+UserCompanyId+"')";
                        string editCopy1 = "Update card_copy1 set AssignType=" + 2+" where  Card_ICCID='"+item.Card_ICCID+ "'and Card_CompanyID='"+CompanyId+"' ";
                        var res = conn.Execute(AddCopy1);
                        var res1 = conn.Execute(editCopy1);
                        meg.flg = "1";
                        meg.Msg = "卡分配成功!";
                    }
                }
            }
            catch (Exception ex)
            {
                meg.flg = "-1";
                meg.Msg = "卡分配失败"+ex;
            }
            return meg;
        }

        ///<summary>
        ///撤销卡的分配
        /// </summary>
        public Message CancelCard(AssignCardPara para)
        {
            Message meg = new Message();
            try
            {
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    foreach (var item in para.cardInfos)
                    {
                        string UpdateCopy1 = "update card_copy1 set AssignType=1 where Card_CompanyID='"+para.CompanyId+ "'and Card_ICCID='"+item.Card_ICCID+"'";
                        string DelCopy1 = " DELETE FROM card_copy1 where Card_CompanyID='"+para.UserCompanyId+"' and Card_ICCID='" + item.Card_ICCID + "'";
                        var res = conn.Execute(UpdateCopy1);
                        var res1 = conn.Execute(DelCopy1);
                        meg.flg = "1";
                        meg.Msg = "卡撤销分配成功!";
                    }
                }
            }
            catch (Exception ex)
            {
                meg.flg = "-1";
                meg.Msg = "卡撤销分配失败" + ex;
            }
            return meg;
        }

        ///<summary>
        ///查看二级用户卡信息（选择分配的卡 已经分配的卡不显示）
        /// </summary>
        /// <param name="CompanyId">登录的用户公司编号</param>
        /// <returns></returns>
        public QueryCardInfo QueryCardList(string CompanyId,int PagNumber,int Num)
        {
            QueryCardInfo cardInfo = new QueryCardInfo();
            int Total = 0;
            string s = string.Empty;
            s = " limit " + (PagNumber - 1) * Num + "," + Num;
            try
            {
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    string sqllist = "select id,Card_ID,Card_ICCID,AssignType from card_copy1 where AssignType="+1+" and Card_CompanyID='"+CompanyId+"'"+s;
                    cardInfo.cardInfos = conn.Query<CardInfo>(sqllist).ToList();
                    Total = cardInfo.cardInfos.Count;
                    cardInfo.Total = Total;
                    cardInfo.flg = "1";
                    cardInfo.Msg = "查询成功!";                   
                }
            }
            catch (Exception ex)
            {
                cardInfo.flg = "-1";
                cardInfo.Msg = "查询失败"+ex;
            }
            return cardInfo;
        }

        /// <summary>
        /// 给用户分配权限
        /// </summary>
        /// <param name="Company_ID">用户编号</param>
        /// <param name="User_Menu">选择的权限拼接串 如:1,2,4,5</param>
        /// <returns></returns>
        public Message AssignAuthority(AddAndEditMenuCompanyPara para)
        {
            Message meg = new Message();
            try
            {
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    string UpdateMenu = "update cf_user set User_Menu='"+para.CompanyMenu+ "',LoginName='"+para.LoginName+"' where Company_ID='" + para.Company_ID+"'";
                    var res = conn.Execute(UpdateMenu);//修改用户信息和用户菜单
                    string UpdateCompany = "update company set CompanyName='"+para.CompanyName+ "',Companyremarks='"+para.Companyremarks+ "',CompanyPhone='"+para.CompanyPhone+"'," +
                        "CompanyAdress='"+para.CompanyAdress+ "' where CompanyID='"+para.Company_ID+"'";
                    var rescompany = conn.Execute(UpdateCompany);
                    meg.flg = "1";
                    meg.Msg = "操作成功!";
                }
            }
            catch (Exception ex)
            {
                meg.flg = "-1";
                meg.Msg = "操作失败!"+ex;
            }
            return meg;
        }

        /// <summary>
        /// 获取用户的权限菜单
        /// </summary>
        /// <param name="Company_ID">用户编号</param>
        /// <returns></returns>
        public List<GetUserMenuDto> GetUserMenu(string Company_ID)
        {
            List<GetUserMenuDto> info = new List<GetUserMenuDto>();
                    
            try
            {
                if (Company_ID == "1556265186243")//奇迹物联
                {
                    using (IDbConnection conn = DapperService.MySqlConnection())
                    {
                        string sqlmeun = "select * from fk_menu where Status=1";
                        var onemenulist = conn.Query<fk_menu>(sqlmeun).Where(t=>t.Type==1).OrderBy(t=>Convert.ToInt32(t.Index)).ToList();                       
                        foreach (var item in onemenulist)
                        {
                            GetUserMenuDto dtos = new GetUserMenuDto();
                            List<childrens> childrens = new List<childrens>();
                            dtos.Index = item.Index;
                            dtos.MenuName = item.MenuName;
                            dtos.icon = item.icon;
                            var twomenulist = conn.Query<fk_menu>(sqlmeun).Where(t => t.Menu_FatherID == item.id).ToList();
                            foreach (var items in twomenulist)
                            {                               
                                childrens s = new childrens();
                                s.Menu = items.MenuURL;
                                s.MenuName = items.MenuName;
                                childrens.Add(s);                               
                            }
                             dtos.children = childrens;
                             info.Add(dtos);
                        }                       
                    }
                }
                else
                {
                    using (IDbConnection conn = DapperService.MySqlConnection())
                    {
                        string sqluserinfo = "select User_Menu from cf_user where Company_ID='" + Company_ID + "'";
                        string User_Menu = conn.Query<cf_user>(sqluserinfo).Select(t => t.User_Menu).FirstOrDefault();
                        string[] UserMenus = User_Menu.Split(',');
                        for (int num = 0; num < UserMenus.Length; num++)
                        {
                            string sqlpidmenuid = "select Menu_FatherID from fk_menu where id=" + UserMenus[num] + "";
                            int Pid = conn.Query<fk_menu>(sqlpidmenuid).Select(t => t.Menu_FatherID).FirstOrDefault();
                            if (Pid != 0)
                            {
                                int id = Array.IndexOf(UserMenus, Pid.ToString()); // 这里的pid就是你要查找的值
                                if (id == -1)//不存在
                                {
                                    User_Menu = User_Menu + ",";
                                    User_Menu +=Pid.ToString() ;
                                }
                            }
                        }
                        string sqlMenuList = "select * from fk_menu where id IN(" + User_Menu + ")";
                        var MenuList = conn.Query<fk_menu>(sqlMenuList).ToList();
                        var MenuListOne = MenuList.Where(t => t.Type == 1);
                        foreach (var item in MenuListOne)
                        {
                            List<childrens> childrens = new List<childrens>();
                            GetUserMenuDto dtos = new GetUserMenuDto();
                            dtos.Index = item.Index;
                            dtos.MenuName = item.MenuName;
                            dtos.icon = item.icon;
                            var TwoMenu = MenuList.Where(t => t.Menu_FatherID == item.id).ToList();
                            foreach (var items in TwoMenu)
                            {
                                childrens s = new childrens();
                                s.Menu = items.MenuURL;
                                s.MenuName = items.MenuName;
                                childrens.Add(s);
                            }
                            dtos.children = childrens;
                            info.Add(dtos);
                        }
                    }
                }              
            }
            catch (Exception ex)
            {
               
            }
            return info;
        }
        #endregion
    }
}