using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using System.Web.Http;
using SS.GovInteract.Core;
using SS.GovInteract.Core.Utils;

namespace SS.GovInteract.Controllers
{
    [RoutePrefix("captcha")]
    public partial class CaptchaController : ApiController
    {
        private const string ApiRoute = "";
        private const string ApiRouteActionsCheck = "actions/check";

        [HttpGet, Route(ApiRoute)]
        public void Get()
        {
            var response = HttpContext.Current.Response;

            var code = CreateValidateCode();
            if (CacheUtils.Exists($"{CookieName}.{code}"))
            {
                code = CreateValidateCode();
            }

            CookieUtils.SetCookie(CookieName, code, DateTime.Now.AddMinutes(10));

            response.BufferOutput = true;  //特别注意
            response.Cache.SetExpires(DateTime.Now.AddMilliseconds(-1));//特别注意
            response.Cache.SetCacheability(HttpCacheability.NoCache);//特别注意
            response.AppendHeader("Pragma", "No-Cache"); //特别注意
            response.ContentType = "image/png";

            byte[] buffer;

            using (var image = new Bitmap(130, 53, PixelFormat.Format32bppRgb))
            {
                var r = new Random();
                var colors = Colors[r.Next(0, 5)];

                using (var g = Graphics.FromImage(image))
                {
                    g.FillRectangle(new SolidBrush(Color.FromArgb(240, 243, 248)), 0, 0, 200, 200); //矩形框
                    g.DrawString(code, new Font(FontFamily.GenericSerif, 28, FontStyle.Bold | FontStyle.Italic), new SolidBrush(colors), new PointF(14, 3));//字体/颜色

                    var random = new Random();

                    for (var i = 0; i < 25; i++)
                    {
                        var x1 = random.Next(image.Width);
                        var x2 = random.Next(image.Width);
                        var y1 = random.Next(image.Height);
                        var y2 = random.Next(image.Height);

                        g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
                    }

                    for (var i = 0; i < 100; i++)
                    {
                        var x = random.Next(image.Width);
                        var y = random.Next(image.Height);

                        image.SetPixel(x, y, Color.FromArgb(random.Next()));
                    }

                    g.Save();
                }

                using (var ms = new MemoryStream())
                {
                    image.Save(ms, ImageFormat.Png);
                    buffer = ms.ToArray();
                }
            }

            response.ClearContent();
            response.BinaryWrite(buffer);
            response.End();
        }

        [HttpPost, Route(ApiRouteActionsCheck)]
        public IHttpActionResult Check([FromBody] CheckRequest request)
        {
            try
            {
                var code = CookieUtils.GetCookie(CookieName);

                if (string.IsNullOrEmpty(code) || CacheUtils.Exists($"{CookieName}.{code}"))
                {
                    return BadRequest("验证码已超时，请点击刷新验证码！");
                }

                CookieUtils.Erase(CookieName);
                CacheUtils.InsertMinutes($"{CookieName}.{code}", true, 10);

                if (!StringUtils.EqualsIgnoreCase(code, request.Captcha))
                {
                    return BadRequest("验证码不正确，请重新输入！");
                }

                return Ok(new
                {
                    Value = true
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
