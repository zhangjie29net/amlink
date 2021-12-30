using Esim7.Action;
using Esim7.Models;
using Esim7.parameter.User;
using Esim7.ReturnMessage;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using static Esim7.Action.Login;

namespace Esim7.Controllers
{
    public class LoginController : ApiController
    {
        /// <summary>
        /// 客户注册
        /// </summary>
        /// <param name="Username"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        [Route("Inner")]
        [HttpPost]
        public IHttpActionResult inner(string Username, string Password)
        {
            return Json<User>(Login.Inner(Username, Password));
        }

        ///<summary>
        ///新用户注册
        /// </summary>
        [HttpPost]
        [Route("UserRegister")]
        public UserRegisterDto UserRegister(UserRegisterPara para)
        {
            Login l = new Login();
            UserRegisterDto info = new UserRegisterDto();
            info = l.UserRegister(para);
            return info;
        }

        ///<summary>
        ///下发短信
        /// </summary>
        [HttpGet]
        [Route("shormessage")]
        public PhoneShortMessage shormessage(string phonenum)
        {
            Login l = new Login();
            PhoneShortMessage info = new PhoneShortMessage();
            info = l.shormessage(phonenum);
            return info;
        }

        ///<summary>
        ///下发短信测试内容
        /// </summary>
        [HttpGet]
        [Route("shormessagecontent")]
        public PhoneShortMessage shormessagecontent(string phonenum)
        {
            Login l = new Login();
            PhoneShortMessage info = new PhoneShortMessage();
            info = l.shormessagecontent(phonenum);
            return info;
        }

        

        /// <summary>
        /// 客户登陆
        /// </summary>
        /// <param name="Username"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        [Route("Login")]
        [HttpGet]
        public IHttpActionResult Loginin(string loginname, string Password)
        {
            //CLogin com = JsonConvert.DeserializeObject<CLogin>(Json);
            //string Username = com.Username;
            //string Password = com.Password;
            return Json<User>(Login.Judge(loginname, Password));
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        [Route("UPdate")]
        [HttpPost]
        public IHttpActionResult UPdate(dynamic str)
        {
            User u = new User();
            u.loginname = str["UserName"];
            u.loginpwd = str["Password"];
            return Json<User>(Login.update(u));
        }

        //[Route("UPdate")]
        //[HttpPost]
        //public IHttpActionResult UPdate(dynamic str)
        //{
        //    User u = new User();
        //    u.loginname = str["UserName"];
        //    u.loginpwd = str["Password"];
        //    return Json<User>(Login.update(u));
        //}


        [Route("Select")]
        [HttpGet]
        public IHttpActionResult SelectUser()
        {
            Result_User result_User = new Result_User();
            if (Login.Getusers().Count!=0)
            {
                result_User.users = Login.Getusers();
                result_User.flg = "1";
                result_User.Msg = "Success";               
            }
            else
            {
                result_User.flg = "0";
                result_User.Msg = "未找到用户";
            }
            return Json(result_User);
        }

        ///<summary>
        ///修改个人信息
        /// </summary>
        [HttpPost]
        [Route("UpdateInfo")]
        public Information UpdateInfo(UserUpdateInfo para)
        {
            Login l = new Login();
            Information info = new Information();
            info = l.UpdateInfo(para);
            return info;
        }


        ///<summary>
        ///重置密码
        /// </summary>
        [HttpPost]
        [Route("ResetPassword")]
        public Information ResetPassword(User para)
        {
            Login l = new Login();
            Information info = new Information();
            info = l.ResetPassword(para);
            return info;
        }


        #region simlink平台企业认证
        ///<summary>
        ///企业认证   上传认证信息文件
        /// </summary>
        public Information UploadAuthenticationInfo(UploadAuthenticationPara para)
        {
            Login l = new Login();
            Information info = new Information();
            info = l.UploadAuthenticationInfo(para);
            return info;
        }


        ///<summary>
        ///查看企业认证信息
        /// </summary>
        [HttpGet]
        [Route("GetAuthenticationInfo")]
        public AuthenticationDtos GetAuthenticationInfo(string Company_ID)
        {
            Login l = new Login();
            AuthenticationDtos dtos = new AuthenticationDtos();
            dtos = l.GetAuthenticationInfo(Company_ID);
            return dtos;
        }
        #endregion


        #region   用户上传logo
        /// <summary>
        /// 用户上传logo
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("UserLogoImgFile")]
        public Task<HttpResponseMessage> PostFormData()
        {
            // Check if the request contains multipart/form-data.  
            // 检查该请求是否含有multipart/form-data  
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }
            string root = this.PubFileRootPath;
            var provider = new MultipartFormDataStreamProvider(root);
            // 最大文件大小
            const int maxSize = 10000000;
            DateTime dat = DateTime.Now;
            string Tpath = string.Format("/{0}/{1}/{2}", dat.Year, dat.Month, dat.Day);

            string FilePath = root + "\\";//+ Tpath + "\\";
            DirectoryInfo di = new DirectoryInfo(FilePath);
            if (!di.Exists) { di.Create(); }
            // 读取表单数据，并返回一个async任务  
            var task = Request.Content.ReadAsMultipartAsync(provider).
                ContinueWith<HttpResponseMessage>(t =>
                {
                    if (t.IsFaulted || t.IsCanceled)
                    {
                        Request.CreateErrorResponse(HttpStatusCode.InternalServerError, t.Exception);
                    }
                    // 以下描述了如何获取文件名  
                    string fileName = string.Empty;
                    List<LogoFileName> files = new List<LogoFileName>();
                    foreach (MultipartFileData fileitem in provider.FileData)
                    {
                        LogoFileName res = new LogoFileName();
                        var fileGuid = Guid.NewGuid();
                        string FileName = fileGuid.ToString("N");
                        res.fileName = FileName;
                        fileName = FileName;
                        string fileFullName = string.Empty;
                        fileFullName = string.Format("{0}{1}", FilePath, FileName);
                    //保存文件到路径 
                    //var file = fileitem;// provider.FileData[0];
                    var file = provider.FileData[0];
                        var fileinfo = new System.IO.FileInfo(file.LocalFileName);
                        fileinfo.MoveTo(fileFullName);
                        files.Add(res);
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, new { success = true, fileName });
                });

            return task;
        }

        ///<summary>
        /// 用户上传logo公共文件目录
        /// </summary>
        public string PubFileRootPath
        {
            get
            {
                var mappedPath = System.Web.Hosting.HostingEnvironment.MapPath("~/");
                return mappedPath + "UploadUserLogoFiles";
            }
        }
        /// <summary>
        /// 用户上传logo返回文件名称
        /// </summary>
        public class LogoFileName
        {
            public string fileName { get; set; }
        }
        #endregion

        #region   企业认证上传文件
        /// <summary>
        /// 用户上传营业执照
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("UploadAuthentication")]
        public AuthenticationDto UploadAuthentication()
        {
            AuthenticationDto result = new AuthenticationDto();
            try
            {
                string uploadPath = HttpContext.Current.Server.MapPath("~/UploadAuthenticationFiles/LicenseFiles/");
                HttpRequest request = System.Web.HttpContext.Current.Request;
                HttpFileCollection fileCollection = request.Files;
                // 判断是否有文件
                if (fileCollection.Count > 0)
                {
                    // 获取文件
                    HttpPostedFile httpPostedFile = fileCollection[0];
                    string fileExtension = Path.GetExtension(httpPostedFile.FileName);// 文件扩展名
                    string fileName = Guid.NewGuid().ToString() + fileExtension;// 名称
                    //string filePath = uploadPath + httpPostedFile.FileName;// 上传路径
                    string filePath = uploadPath + fileName;
                    int filemax= httpPostedFile.ContentLength;// 文件大小 取字节 
                    if (filemax<20971520) //判断文件大小 不能超过20MB
                    {
                        if (fileExtension == ".jpg" || fileExtension == ".jpeg" || fileExtension == ".bmp" || fileExtension == ".gif" || fileExtension == ".png") //判断文件格式
                        {
                            // 如果目录不存在则要先创建
                            if (!Directory.Exists(uploadPath))
                            {
                                Directory.CreateDirectory(uploadPath);
                            }
                            // 保存新的文件
                            //while (File.Exists(filePath))
                            //{
                            //    fileName = Guid.NewGuid().ToString("N") + fileExtension;
                            //    filePath = uploadPath + fileName;
                            //}

                            httpPostedFile.SaveAs(filePath);
                            result.success = true;
                            result.LicenseName = fileName;
                        }
                    }
                    else
                    {
                        result.success = false;
                        result.Msg = "文件格式错误或文件大小超出范围";
                    }

                }
            }
            catch (Exception ex)
            {
                result.success =false;
                result.Msg = "错误" + ex;
            }
            return result;
        }

        /// <summary>
        /// 用户上传身份证正面照片
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("UploadIDpositive")]
        public AuthenticationDto UploadIDpositive()
        {
            AuthenticationDto result = new AuthenticationDto();
            try
            {
                string uploadPath = HttpContext.Current.Server.MapPath("~/UploadAuthenticationFiles/IDpositiveFiles/");
                HttpRequest request = System.Web.HttpContext.Current.Request;
                HttpFileCollection fileCollection = request.Files;
                // 判断是否有文件
                if (fileCollection.Count > 0)
                {
                    // 获取文件
                    HttpPostedFile httpPostedFile = fileCollection[0];
                    string fileExtension = Path.GetExtension(httpPostedFile.FileName);// 文件扩展名
                    string fileName = Guid.NewGuid().ToString() + fileExtension;// 名称
                                                                                //string filePath = uploadPath + httpPostedFile.FileName;// 上传路径
                    string filePath = uploadPath + fileName;
                    int filemax = httpPostedFile.ContentLength;// 文件大小 取字节 
                    if (filemax < 20971520) //判断文件大小 不能超过20MB
                    {
                        if (fileExtension == ".jpg" || fileExtension == ".jpeg" || fileExtension == ".bmp" || fileExtension == ".gif" || fileExtension == ".png") //判断文件格式
                        {
                            // 如果目录不存在则要先创建
                            if (!Directory.Exists(uploadPath))
                            {
                                Directory.CreateDirectory(uploadPath);
                            }
                            // 保存新的文件
                            //while (File.Exists(filePath))
                            //{
                            //    filePath = uploadPath + fileName;
                            //}
                            httpPostedFile.SaveAs(filePath);
                            result.success = true;
                            result.IDpositiveName = fileName;
                        }
                    }
                    else
                    {
                        result.success = false;
                        result.Msg = "文件格式错误或文件大小超出范围";
                    }

                }
            }
            catch (Exception ex)
            {
                result.success = false;
                result.Msg = "错误" + ex;
            }
            return result;
        }


        /// <summary>
        /// 用户上传身份证反面照片
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("UploadIDback")]
        public AuthenticationDto UploadIDback()
        {
            AuthenticationDto result = new AuthenticationDto();
            try
            {
                string uploadPath = HttpContext.Current.Server.MapPath("~/UploadAuthenticationFiles/IDbackFiles/");
                HttpRequest request = System.Web.HttpContext.Current.Request;
                HttpFileCollection fileCollection = request.Files;
                // 判断是否有文件
                if (fileCollection.Count > 0)
                {
                    // 获取文件
                    HttpPostedFile httpPostedFile = fileCollection[0];
                    string fileExtension = Path.GetExtension(httpPostedFile.FileName);// 文件扩展名
                    string fileName = Guid.NewGuid().ToString() + fileExtension;// 名称
                    //string filePath = uploadPath + httpPostedFile.FileName;// 上传路径
                    string filePath = uploadPath + fileName;
                    int filemax = httpPostedFile.ContentLength;// 文件大小 取字节 
                    if (filemax < 20971520) //判断文件大小 不能超过20MB
                    {
                        if (fileExtension == ".jpg" || fileExtension == ".jpeg" || fileExtension == ".bmp" || fileExtension == ".gif" || fileExtension == ".png") //判断文件格式
                        {
                            // 如果目录不存在则要先创建
                            if (!Directory.Exists(uploadPath))
                            {
                                Directory.CreateDirectory(uploadPath);
                            }
                            // 保存新的文件
                            //while (File.Exists(filePath))
                            //{
                            //    filePath = uploadPath + fileName;
                            //}
                            httpPostedFile.SaveAs(filePath);
                            result.success = true;
                            result.IDbackName = fileName;
                        }
                    }
                    else
                    {
                        result.success = false;
                        result.Msg = "文件格式错误或文件大小超出范围";
                    }
                }
            }
            catch (Exception ex)
            {
                result.success = false;
                result.Msg = "错误"+ex;
            }
            return result;
        }

        /// <summary>
        /// 用户上传logo返回文件名称
        /// </summary>
        public class AuthenticationDto
        {
            ///<summary>
            ///状态
            /// </summary>
            public bool success { get; set; }
            ///<summary>
            ///信息
            /// </summary>
            public string Msg { get; set; }
            /// <summary>
            /// 营业执照图片
            /// </summary>
            public string LicenseName { get; set; }
            /// <summary>
            /// 身份证正面照
            /// </summary>
            public string IDpositiveName { get; set; }
            /// <summary>
            /// 身份证反面照
            /// </summary>
            public string IDbackName { get; set; }
        }
        #endregion

    }
}
