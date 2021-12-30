using Esim7.Action;
using Esim7.Models.PowerManageModel;
using Esim7.parameter.PowerManage;
using System.Collections.Generic;
using System.Web.Http;

/// <summary>
/// 权限管理模块
/// </summary>
namespace Esim7.Controllers
{
    public class PowerManageController : ApiController
    {
        PowerManageAction r = new PowerManageAction();
        ///<summary>
        ///用户添加子用户
        /// </summary>
        /// <param user></param>
        /// <returns></returns>
        //[HttpPost]
        //[Route("AddUser")]
        //public cf_userInfo AddUser(AddUserPara para)
        //{
        //    cf_userInfo c = new cf_userInfo();
        //    c = r.AddUser(para);
        //    return c;
        //}

        ///<summary>
        ///查看子用户列表
        /// </summary>
        [HttpGet]
        [Route("ListUserInfo")]
        public cf_userInfo ListUserInfo(string Company_ID,string CompanyName)
        {
            cf_userInfo c = new cf_userInfo();
            c = r.ListUserInfo(Company_ID,CompanyName);
            return c;
        }

        ///<summary>
        ///用户修改密码
        /// </summary>
        [Route("UpdateUserPwd")]
        public cf_userInfo UpdateUserPwd(string OldPassword, string NewPassWord, string Company_ID)
        {
            cf_userInfo c = new cf_userInfo();
            c = r.UpdateUserPwd(OldPassword, NewPassWord, Company_ID);
            return c;
        }
        ///<summary>
        ///设置子用户的启用禁用状态
        /// </summary>
        [Route("UpdateStatus")]
        public cf_userInfo UpdateStatus(int status, int id)
        {
            cf_userInfo c = new cf_userInfo();
            c = r.UpdateStatus(status, id);
            return c;
        }

        ///<summary>
        ///删除子用户
        /// </summary>
        [HttpGet]
        [Route("DeleUser")]
        public Message DeleUser(int id)
        {
            Message m = new Message();
            m = r.DeleUser(id);
            return m;
        }

        #region  菜单管理
        ///<summary>
        ///添加菜单信息
        /// </summary>
        [Route("AddMenu")]
        public fk_menuInfo AddMenu(AddMenuPara para)
        {
            fk_menuInfo c = new fk_menuInfo();
            c = r.AddMenu(para);
            return c;
        }

        ///<summary>
        ///编辑菜单信息
        /// </summary>
        [Route("UpdateMenu")]
        public fk_menuInfo UpdateMenu(EditMenuPara para)
        {
            fk_menuInfo c = new fk_menuInfo();
            c = r.UpdateMenu(para);
            return c;
        }

        ///<summary>
        ///设置菜单的启用禁用状态
        /// </summary>
        [Route("EditMenuStatus")]
        public Message EditMenuStatus(int id, int status)
        {
            Message mess = new Message();
            mess = r.EditMenuStatus(id,status);
            return mess;
        }

        ///<summary>
        ///显示顶级菜单id和名称
        /// </summary>
        [Route("ListOneMenuInfo")]
        public OneMenuInfo ListOneMenuInfo()
        {
            OneMenuInfo oneMenu = new OneMenuInfo();
            oneMenu = r.ListOneMenuInfo();
            return oneMenu;
        }

        ///<summary>
        ///显示菜单信息 树形结构
        /// </summary>
        [HttpGet]
        [Route("ListMenuInfo")]
        public string ListMenuInfo(string Company_ID,string userCompany_ID)
        {
            string info = string.Empty;
            //TreelikeMenu info = new TreelikeMenu();
            info = r.ListMenuInfo(Company_ID, userCompany_ID);
            return info;
        }
        #endregion

        #region  分配管理
        /// <summary>
        /// 用户给子级用户分配卡
        /// </summary>
        /// <param name="para">公司编号 分配卡的公司编号 卡信息集合</param>
        /// <returns></returns>
        [Route("CardAssign")]
        public Message CardAssign(AssignCardPara para)
        {
            Message c = new Message();
            c = r.CardAssign(para);
            return c;
        }

        ///<summary>
        ///撤销卡的分配
        /// </summary>
        [Route("CancelCard")]
        public Message CancelCard(AssignCardPara para)
        {
            Message c = new Message();
            c = r.CancelCard(para);
            return c;
        }

        ///<summary>
        ///查看二级用户卡信息（选择分配的卡 已经分配的卡不显示）
        /// </summary>
        /// <param name="CompanyId">登录的用户公司编号</param>
        /// <returns></returns>
        [Route("QueryCardList")]
        public QueryCardInfo QueryCardList(string CompanyId, int PagNumber, int Num)
        {
            QueryCardInfo c = new QueryCardInfo();
            c = r.QueryCardList(CompanyId,PagNumber,Num);
            return c;
        }

        /// <summary>
        /// 给用户分配权限
        /// </summary>
        /// <param name="Company_ID">用户编号</param>
        /// <param name="User_Menu">选择的权限拼接串 如:1,2,4,5</param>
        /// <returns></returns>
        [HttpPost]
        [Route("AssignAuthority")]
        public Message AssignAuthority(AddAndEditMenuCompanyPara para)
        {
            Message c = new Message();
            c = r.AssignAuthority(para);
            return c;
        }

        /// <summary>
        /// 获取用户的权限菜单
        /// </summary>
        /// <param name="Company_ID">用户编号</param>
        /// <returns></returns>
        [Route("GetUserMenu")]
        public List<GetUserMenuDto> GetUserMenu(string Company_ID)
        {
            List<GetUserMenuDto> c = new List<GetUserMenuDto>();
            c = r.GetUserMenu(Company_ID);
            return c;
        }
        #endregion
    }
}
