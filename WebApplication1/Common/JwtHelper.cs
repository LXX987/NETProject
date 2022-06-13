using Microsoft.IdentityModel.Tokens;
using WebApplication1.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebApplication1.Common
{
    public class JwtHelper
    {
        /// <summary>
        /// 颁发JWT字符串
        /// </summary>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        public static string IssueJwt(TokenModel tokenModel)
        {
            //获取Appsetting配置
            //string iss = AppSetting.app(new string[] { "AppSettings", "JwtSetting", "Issuer" });//颁发者
            //string aud = AppSetting.app(new string[] { "AppSettings", "JwtSetting", "Audience" });//可以给哪些客户端使用
            //string secret = AppSetting.app(new string[] { "AppSettings", "JwtSetting", "SecretKey" });//加密的Key
            string iss = "jwtIssuer";
            string aud = "jwtAudience";
            string secret = "jwtsecretkeysixteen";

            //定义需要使用的Claim
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, $"{tokenModel.Uid}"),
                new Claim(JwtRegisteredClaimNames.Iat, $"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}"),
                new Claim(JwtRegisteredClaimNames.Nbf,$"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}"),
                //这个就是过期时间，目前是过期1天，可自定义，注意JWT有自己的缓冲过期时间
                new Claim (JwtRegisteredClaimNames.Exp,$"{new DateTimeOffset(DateTime.Now.AddSeconds(60*60*24)).ToUnixTimeSeconds()}"),
                new Claim(ClaimTypes.Expiration, DateTime.Now.AddSeconds(60*60*24).ToString()),
                new Claim(JwtRegisteredClaimNames.Iss,iss),
                new Claim(JwtRegisteredClaimNames.Aud,aud),
            };

            //秘钥 (SymmetricSecurityKey 对安全性的要求，密钥的长度太短会报出异常)
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(
                issuer: iss,
                claims: claims,
                signingCredentials: creds);

            var jwtHandler = new JwtSecurityTokenHandler();
            var encodedJwt = jwtHandler.WriteToken(jwt);

            return encodedJwt;
        }

        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="jwtStr"></param>
        /// <returns></returns>
        public static TokenModel SerializeJwt(string jwtStr)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtToken = jwtHandler.ReadJwtToken(jwtStr);
            
            var tm = new TokenModel
            {
                Uid = jwtToken.Id.ToString(),
            };
            return tm;
        }
    }
}
