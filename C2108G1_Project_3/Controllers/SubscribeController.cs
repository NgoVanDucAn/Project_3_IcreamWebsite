using C2108G1_Project_3.Data;
using C2108G1_Project_3.Models;
using C2108G1_Project_3.Enum;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using Newtonsoft.Json;
using PayPal.Api;
using Microsoft.AspNetCore.Authorization;

namespace C2108G1_Project_3.Controllers
{
    [Authorize]
    public class SubscribeController : Controller
    {

        private readonly IConfiguration _configuration;
        private readonly SignInManager<Users> _signInManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<Users> _userManager;
        private readonly IHttpContextAccessor _contextAccessor;
        public SubscribeController(IHttpContextAccessor contextAccessor, UserManager<Users> userManager, IConfiguration configuration, SignInManager<Users> signInManager, ApplicationDbContext dbContext)
        {
            _configuration = configuration;
            _signInManager = signInManager;
            _dbContext = dbContext;
            _userManager = userManager;
            _contextAccessor = contextAccessor;
        }
        [HttpPost]
        public IActionResult ProcessPayment(string amount, MembershipType membershipType)
        {
            int day = 0;
            //kiểm tra người dùng đăng nhập hay chưa
            if (_signInManager.IsSignedIn(User))//đã đăng nhập
            {
                //nhận tên người dùng
                var currentUser = _userManager.GetUserAsync(_contextAccessor.HttpContext.User).Result;
                var id = currentUser.Id;
                //phân loại thanh toán
                //phân loại thanh toán
                if (amount == "15")
                {
                    day = 30;
                    membershipType = MembershipType.Monthly;
                }
                else if (amount == "150")
                {
                    day = 365;
                    membershipType = MembershipType.Yearly;
                }
                //kiểm tra ngày hết hạn sử dụng người dùng đăng kí chưa
                var expirationDate = _dbContext.RegisterUsers
                .Where(p => p.IdentityUserId == id)
                .Select(p => p.dueDate)
                .Max();
                //kiểm tra xem người dùng còn đã đăng kí bh chưa
                if (expirationDate != null)
                { //đã từng đăng kí

                    if (DateTime.Now > expirationDate)//khi người dùng hết hạn
                    {
                        var register = new RegisterUsers
                        {
                            IdentityUserId = id,
                            registrationDate = DateTime.Now,
                            dueDate = DateTime.Now.AddDays(day),
                            membershipType = membershipType
                        };
                        TempData["RegisterData"] = JsonConvert.SerializeObject(register);
                        
                    }

                    else // khi người dùng còn hạn
                    {
                        var register = new RegisterUsers
                        {
                            IdentityUserId = id,
                            registrationDate = expirationDate,
                            dueDate = expirationDate + TimeSpan.FromDays(day),
                            membershipType = membershipType
                        };
                        TempData["RegisterData"] = JsonConvert.SerializeObject(register);
                    }
                }
                else //chưa từng đăng kí
                {
                    var register = new RegisterUsers
                    {
                        IdentityUserId = id,
                        registrationDate = DateTime.Now,
                        dueDate = DateTime.Now.AddDays(day),
                        membershipType = membershipType
                    };
                    TempData["RegisterData"] = JsonConvert.SerializeObject(register);

                }
                //hoạt động thanh toán qua paypal 
                var apiContext = new APIContext(new OAuthTokenCredential(_configuration["AppSettings:PayPalClientId"], _configuration["AppSettings:PayPalSecretKey"]).GetAccessToken());

                var payment = Payment.Create(apiContext, new Payment
                {
                    intent = "sale",
                    payer = new Payer { payment_method = "paypal" },
                    transactions = new List<Transaction>
                {
                    new Transaction
                    {
                        amount = new Amount { total = amount, currency = "USD" },
                        description = "Thanh toán PayPal"
                    }
                },
                    redirect_urls = new RedirectUrls
                    {
                        return_url = Url.Action("PaymentSuccess", "Subscribe", null, Request.Scheme),
                        cancel_url = Url.Action("PaymentCancelled", "Subscribe", null, Request.Scheme)
                    }
                });

                //chuyển hướng sau khi bắt đầu thanh toán
                var redirectUrl = payment.links.Find(x => x.rel == "approval_url").href;
                return Redirect(redirectUrl);
            }
            else
            {//chưa đăng nhập
                return Redirect("/Identity/Account/Login");
            }
        }
        public async Task<IActionResult> PaymentSuccessAsync(string paymentId, string token, string PayerID)
        {
            var apiContext = new APIContext(new OAuthTokenCredential(_configuration["AppSettings:PayPalClientId"], _configuration["AppSettings:PayPalSecretKey"]).GetAccessToken());

            var paymentExecution = new PaymentExecution { payer_id = PayerID };
            var payment = new Payment { id = paymentId }.Execute(apiContext, paymentExecution);

            if (payment.state.ToLower() == "approved")
            {
                if (TempData.ContainsKey("RegisterData"))
                {
                    var registerData = TempData["RegisterData"] as string;
                    var register = JsonConvert.DeserializeObject<RegisterUsers>(registerData);
                    var currentUser = _userManager.GetUserAsync(_contextAccessor.HttpContext.User).Result;
                    var subUserPermission = "SUBUSERS";
                    await _userManager.AddToRoleAsync(currentUser, subUserPermission);
                    await _userManager.UpdateAsync(currentUser);
                    var userId = currentUser.UserName;
                    _dbContext.RegisterUsers.Add(register);
                    _dbContext.SaveChanges();
                }
                TempData["WelcomeMessage"] = "PaymentSuccess";
                return RedirectToAction("Subscribe", "Home");
            }
            else
            {
                // Thanh toán thất bại
                TempData["WelcomeMessage"] = "PaymentFail";
                return RedirectToAction("Subscribe", "Home");
            }

        }
        public IActionResult PaymentCancelled()
        {
            // Xử lý khi người dùng hủy thanh toán

            TempData["WelcomeMessage"] = "PaymentCancelled";
            return RedirectToAction("Subscribe", "Home");
            

        }
    }
}

