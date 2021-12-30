using Esim7.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esim7.parameter.User
{
    /// <summary>
    /// 用户注册成功后的返回值
    /// </summary>
    public class UserRegisterDto: Information
    {
        public string LoginName { get; set; }
    }
}