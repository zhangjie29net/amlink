using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace Esim7.Controllers
{
    /// <summary>
    /// 获取文件
    /// </summary>
    public class GetFileController : ApiController
    {
        ///<summary>
        ///查看LOGO
        /// </summary>
        /// <summary>
        [Route("GetUserLogoFile")]
        [HttpGet]
        public IHttpActionResult GetSupFile(string fileName, string Company_ID)
        {
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK);
            //var fileObj = r.GetFileByAccKey(fileKey, OutCode, this.PubFileRootPath);
            //if (fileObj.IsExist)
            //{

            //    FileInfo f = new FileInfo(fileObj.FilePath);
            //    var filePath = fileObj.FilePath;
            //    if (!f.Exists)
            //    {
            //        filePath = this.FileDefaultPicPath;
            //    }
            //    FileStream fs = File.OpenRead(filePath);
            //    httpResponseMessage.Content = new StreamContent(fs);
            //    httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
            //    return ResponseMessage(httpResponseMessage);
            //}
            //else
            //{

            string filepath = string.Format(@"{0}\{1}", this.PubFileRootPath, fileName);
            string extension = Path.GetExtension(filepath);
            // string filepaths = string.Format(@"{0}\{1}", this.PubFileRootPath, "default.png"); ;
            FileStream fs = File.OpenRead(filepath);
            httpResponseMessage.Content = new StreamContent(fs);
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
            return ResponseMessage(httpResponseMessage);
            //}
        }

        /// <summary>
        /// 系统默认图片路径
        /// </summary>
        public string FileDefaultPicPath
        {
            get
            {
                return string.Format(@"{0}\{1}", this.PubFileRootPath, "default.png");
            }
        }

        /// <summary>
        /// 公共文件目录
        /// </summary>
        public string PubFileRootPath
        {
            get
            {
                var mappedPath = System.Web.Hosting.HostingEnvironment.MapPath("~/");
                return mappedPath + "UploadUserLogoFiles";
            }
        }

        ///<summary>
        ///查看用户支付信息
        /// </summary>
        /// <summary>
        [Route("GetUserPayFile")]
        [HttpGet]
        public IHttpActionResult GetUserPayFile(string fileName)
        {
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK);
            string filepath = string.Format(@"{0}\{1}", this.PayFileRootPath, fileName);
            string extension = Path.GetExtension(filepath);
            // string filepaths = string.Format(@"{0}\{1}", this.PubFileRootPath, "default.png"); ;
            FileStream fs = File.OpenRead(filepath);
            httpResponseMessage.Content = new StreamContent(fs);
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
            return ResponseMessage(httpResponseMessage);
        }

        /// <summary>
        /// 系统默认图片路径
        /// </summary>
        public string FilePayPath
        {
            get
            {
                return string.Format(@"{0}\{1}", this.PayFileRootPath, "default.png");
            }
        }

        /// <summary>
        /// 公共文件目录
        /// </summary>
        public string PayFileRootPath
        {
            get
            {
                var mappedPath = System.Web.Hosting.HostingEnvironment.MapPath("~/");
                return mappedPath + "UploadUserPayFiles";
            }
        }

        ///<summary>
        ///获取用户认证
        /// </summary>
    }
}
