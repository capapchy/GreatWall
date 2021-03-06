﻿using System;
using System.ComponentModel.DataAnnotations;
using GreatWall.Domain.Enums;
using Util.Ui.Attributes;

namespace GreatWall.Domain.Models {
    /// <summary>
    /// 客户端密钥
    /// </summary>
    [Model]
    public class ClientSecret {
        /// <summary>
        /// 密钥类型
        /// </summary>
        [Display( Name = "密钥类型" )]
        public ClientSecretType SecretType { get; set; }
        /// <summary>
        /// 密钥值
        /// </summary>
        [Display( Name = "密钥值" )]
        public string Value { get; set; }
        /// <summary>
        /// 到期时间
        /// </summary>
        [Display( Name = "到期时间" )]
        public DateTime? Expiration { get; set; }
    }
}
