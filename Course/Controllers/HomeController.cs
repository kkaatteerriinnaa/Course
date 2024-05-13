using Course.Data;
using Course.Data.DAL;
using Course.Models;
using Course.Models.Home.FrontendForm;
using Course.Models.Home.Model;
using Course.Models.Home.Model.Ioc;
using Course.Models.Home.Model.Singnup;
using Course.Models.Home.Signup;
using Course.Services.Email;
using Course.Services.Hash;
using Course.Services.Kdf;
using Course.Data.DAL;
using Course.Data;
using Course.Models.Home.FrontendForm;
using Course.Models.Home.Signup;
using Course.Models;
using Course.Services.Hash;
using Course.Services.Kdf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.Net.Mail;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

namespace Course.Controllers
{
    public class HomeController : Controller
    {
        /* �������� ������ (�����������) - ����� � ����������
        �� �������� �������� �� ������� ��'����. �������
        �������������� ����� �������� - ����� �����������. ��
        ��������, ��-�����, ��������� ���� �� ������ (readonly) ��,
        ��-�����, ������������ ��������� ��'���� ��� ��������
        �����������. � ���������� ����� �������� �������������
        �� ����� ��������� (_logger)
        */

        // �������� ��������� ����� - ���� � �� ������, �� ���� ������
        private readonly DataContext _dataContext;


        private readonly ILogger<HomeController> _logger;

        // ��������� ���� ��� ��������� �� �����
        private readonly IHashService _hashService;

        private readonly DataAccessor _dataAccessor;

        private readonly IKdfService _kdfService;
        private readonly IEmailService _emailService;

        //������ �� ������������ ��������-��������� � �������� �� � ��
        public HomeController(IHashService hashService, ILogger<HomeController> logger, DataContext dataContext, DataAccessor dataAccessor, IKdfService kdfService, IEmailService emailService)
        {
            _logger = logger;               // ���������� ��������� �����������, �� ��
            _hashService = hashService;     // ������ ��������� ��� �������� ����������
            _dataContext = dataContext;
            _dataAccessor = dataAccessor;
            _kdfService = kdfService;
            _emailService = emailService;
        }

        public IActionResult ConfirEmail(String id)
        {
            String email, code;
            try
            {
                String data = System.Text.Encoding.UTF8.GetString(
                Convert.FromBase64String(id));
                String[] parts = data.Split(':', 2);
                email = parts[0];
                code = parts[1];
                ViewData["result"] =
                    _dataAccessor.UserDao.ConfirmEmail(email, code)
                    ? "����� ������ ������������"
                    : "������� ������������ �����";
            }
            catch
            {
                ViewData["result"] = " ��� �� ��������";
            }

            return View();
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Intro()
        {
            return View();
        }
        public IActionResult URL()
        {
            return View();
        }
        public IActionResult AboutRazor()
        {
            Models.Home.Model.AboutRazor.AboutRazorPageModel aboutRazorModel = new()
            {
                TabHeader = "Razor",
                PageTitle = "��������� Razor",
                RazorIs = "��������� ��������� �� ������ HTML �������\r\n\t������ ���� ������������� C#.",
                RazorInstrc = "���������� - ���������� ������ ���� �������������, �� ������ ����\r\n\t�� �������� " +
                "(���������� ���� �� ������� ��������),\r\n\t��� � ���������������� ���������.\r\n\tRazor-i��������� ������� � ������ ����� " +
                "&commat;{...}\r\n\t� ������� ����� ������ ���� ������ ���������� ����� �#.\r\n\t������������� ������� ���� � ����������, ��" +
                " ���������� �������\r\n\t�� ������������� ��� � �� " +
                "�������������� ������ �� ���� ������\r\n\t������.",
            };
            return View(aboutRazorModel);
        }

        public ViewResult Admin()
        {
            return View();
        }

        // ������ ����� ����������� ���������� ������, ����������-�����������
        public IActionResult Model(Models.Home.Model.FormModel? formModel)
        {
            // ������ ������������� ����������� � ������������ ������i���
            Models.Home.Model.PageModel pageModel = new()
            {
                TabHeader = "�����i",
                PageTitle = "�����i � ASP",
                FormModel = formModel
            };
            // ������ ������������� ���������� ���������� View()
            return View(pageModel);
        }

        [HttpPost]      //������� �������� ����� ���� POST-�������
        public JsonResult FrontendFrom([FromBody] FrontendFormInput input)
        {
            FrontendFormOutput output = new()
            {
                Code = 200,
                Message = $"{input.UserName} -- {input.UserEmail} -- {input.UserGen} -- {input.UserDate.ToString().Substring(0, 10)}"
            };
            _logger.LogInformation(output.Message);
            return Json(output);
        }

        public IActionResult Data()
        {
            Models.Home.Data.DataPageModel dataPageModel = new()
            {
                TabHeader = "������ � ��",
                PageTitle = "������ � ������. �i��������� ��",
                NuGetPackages = new List<String>
                {
                    "Microsoft.EntityFrameworkCore - ���� ����������, ������ ������",
                    "Microsoft. EntityFrameworkCore.Tools - ����������� ��������� ���������",
                    "������� ��: � ��������� �� ���� -\r\n\t\t��� MSSQL: " +
                    "Microsoft.EntityFrameworkCore.SqlServer\r\n\t\tMySQL: Pomelo.EntityFrameworkCore.MySql"
                },
                DataStruct = new List<String>
                {
                    "��������� � ����� ������ ����� Data, � �� - ���� DataContext",
                    "�������� ����� ���������� �� ��. MSSQL ���� ���������� ��,\r\n\t\t�������� ����� �������� ����� �� ���� �� �������� ��.\r\n\t\tMySQL - ����� �������� ��, ��� �������� ���������. ����� ����������\r\n\t\t������� �� appsettings.json � ���������� ������ \"ConnectionStrings\"",
                    "��������� ����� Data/Entities, � �� ���� - User",
                    "���� DataContext ������������� �� ����� DbContext. � ���� DataContext ������� ������������� ������ \r\n\t\tOnConfiguring() � OnModelCreating(), � ���� ����������� ��������� �������� DbContextOptions ����� base(options).",
                    "� ���� Program.cs ��������� ������ �������� ����� �� ��������� builder.Services.AddDbContext<DataContext>\r\n\t\t\t\t\t(options =>\r\n\t\t\t\t\toptions.UseSqlServer(\r\n\t\t\t\t\tbuilder.Configuration.GetConnectionString(\"LocalMSSQL\")));",
                    "���� ����� ��������������� ������� add-migration '�������� ��������' � ������� ��� ��������� �������.",
                    "ϳ��� ��������� ������� �� ����� ����������� �� ���� ����� �� ��������� ������� update-database."
                }
            };
            ViewData["users-count"] = _dataContext.Users.Count();
            return View(dataPageModel);
        }
        public IActionResult Ioc(String? format)  // Inversion of Control
        {
            // ����������� ������������ �������
            // ViewData["hash"] = 
            // ViewData - ����������� ��'��� ��� �������� �����
            // �� �������������. ���� ����� �� ������ ["hash"]
            // ����� ���������� � ��������� �������
            IocPageModel pageModel = new()
            {
                TabHeader = "IoC",
                PageTitle = "������i� ���������. ������. ",
                SingleHash = _hashService.Digest("123"),
                IoCIs = "IoC (Inversion of Control, ������� ���������) - ������������� ������," +
                "\r\n\t����� � ���� ������ ��������� ������� ������ ��'���� ��������������" +
                "\r\n\t�� ����������� ������ (��������, ��������� �����������, ���������)." +
                "\r\n\t������� ���� ��'����: CRUD. ��������� �� ������, �� ������ ���������" +
                "\r\n\tnew / delete ������ ������� ��������� �� ����������.",
                IoCOptions = new List<String>
                {
                    "��������� ������ - �����, �� ���� ��������� ���������������. ",
                    "��������� ��� ������ � ��������� (��������)",
                    "�������� ������ � ���� ��'����, ���� ���� ������"
                },
                HashExm = new List<String>
                {
                    "(����������) ��������� ����� Services � ����� ������. ",
                    "������� ����� - �� ���������� ��� ����� (���� �� ���������),\r\n\t\t��� ������� ������ ����� ����������� ����� (Hash)",
                    "��������� ��������� IHashService �� ���� Md5HashService ",
                    "P������� ����� (���. Program.cs, ����� 8 � ���)",
                    "��������� ����� (���. HomeController)",
                    $"���������� ���� ������: {_hashService.Digest("123")}",
                    "I����� ������: ��������� ������� �� ����� ���-�������� SHA",
                    "OCP (3 SOLID) \"��������, ��� �� �����\" -- ��������� �����\r\n\t\t���� ShaHashService � ����� Services/Hash"
                },
                Title = "������� ���������� ���������:",
            };
            for (int i = 0; i < 5; i++)
            {
                String str = (i + 100500).ToString();
                pageModel.Hashes[str] = _hashService.Digest(str);
            }
            if (format == "json")
                return Json(pageModel);
            return View(pageModel);
        }
        public IActionResult URLStruct()
        {
            Models.Home.URLStruct.URLStructPageModel uRLStructPage = new()
            {
                TabHeader = "URL",
                PageTitle = "��������� URL",
                PageText = new List<String>
                {
                    "��������: This is the designation of the protocol that is used to access the resource. \r\n        For example, http://, https://, ftp://, mailto:, etc..",
                    "�����: This is the part of the URL that indicates the address of the server on which the resource is hosted. \r\n        For example, www.example.com.",
                    "����: This points to a specific path to a resource on the server. This is the part of the URL after the domain. \r\n        For example, /path/to/resource.",
                    "������ �������: These are the parameters passed in the URL for a request to a resource. \r\n        They begin with a question symbol ? and can contain key-value pairs separated by the ampersand & character. For example, ?key1=value1&key2=value2.",
                    "��������: This is a specific part of the resource that you need to go to or scroll to after the page has loaded. \r\n        The fragment begins with a hash symbol #. For example, #section1."
                },
                PageImageSrc = "/img/url.jpg"
            };
            return View(uRLStructPage);
        }

        public IActionResult Privacy()
        {
            Models.Home.Privacy.PrivacyPageModel privacyPage = new()
            {
                TabHeader = "Privacy",
                PageTitle = "Privacy Policy",
                PageText = "Use this page to detail your site's privacy policy."
            };
            return View(privacyPage);
        }
        public IActionResult Signup(SingupFormModel? formModel)
        {
            SingupPageModel pageModel = new()
            {
                FormModel = formModel
            };
            if (formModel?.HasData ?? false)
            {
                pageModel.ValidationErrors = _ValidateSingupModel(formModel);
                if (pageModel.ValidationErrors.Count == 0)
                {
                    String code = Guid.NewGuid().ToString()[..6];
                    String slug = Convert.ToBase64String(
                        System.Text.Encoding.UTF8.GetBytes(
                        $"{formModel.UserEmail}:{code}"));
                    MailMessage mailMessage = new()
                    {
                        Subject = "ϳ����������� �����",
                        IsBodyHtml = true,
                        Body = "<p>��� ������������ ����� ������ �� ���� ���</p>" +
                        $"<h2 style='color: orange'>{code}</h2>" +
                        $"<p>��� �������� �� <a href='{Request.Scheme}://{Request.Host}/Home/ConfirEmail/{slug}'>��� ����������</a></p>"
                    };
                    mailMessage.To.Add(formModel.UserEmail);
                    try
                    {
                        _emailService.Send(mailMessage);
                        String salt = Guid.NewGuid().ToString();
                        _dataAccessor.UserDao.Signup(new()
                        {
                            Name = formModel.UserName,
                            Email = formModel.UserEmail,
                            EmailComfirmCode = code,
                            Birthdate = formModel.UserBirthdate,
                            AvatarUrl = formModel.SavedAvatarFilename,
                            Salt = salt,
                            DrivedKey = _kdfService.DerivedKey(salt, formModel.Password)
                        });
                    }
                    catch (Exception ex)
                    {
                        pageModel.ValidationErrors["email"] = "�� ������� �������� E-mail";
                        _logger.LogInformation(ex.Message);
                    }


                }
            }
            //_logger.LogInformation(Directory.GetCurrentDirectory());
            return View(pageModel);
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        private Dictionary<string, string> _ValidateSingupModel(SingupFormModel? model)
        {
            Dictionary<string, string> result = new();
            if (model == null)
            {
                result["model"] = "Model is Null";
            }
            else
            {
                if (String.IsNullOrEmpty(model.UserName))
                {
                    result[nameof(model.UserName)] = "User Name should not be empty";
                }
                if (String.IsNullOrEmpty(model.UserEmail))
                {
                    result[nameof(model.UserEmail)] = "User Email should not be empty";
                }
                if (model.UserBirthdate == default(DateTime))
                {
                    result[nameof(model.UserBirthdate)] = "User Birthdate should not be empty";
                }
                if (String.IsNullOrEmpty(model.Password) || !Regex.IsMatch(model.Password, @"^(?=.*[a-zA-Z])(?=.*\d).+$"))
                {
                    result[nameof(model.Password)] = "Password should not be empty and must contain at least one letter and one number";
                }
                if (model.Password != model.UserRepeat)
                {
                    result[nameof(model.UserRepeat)] = "Password repeat must match the password";
                }
                if (!model.Agreement)
                {
                    result[nameof(model.Agreement)] = "You must agree to the terms";
                }
                if (model.UserAvatar != null)
                {
                    int dotPosition = model.UserAvatar.FileName.LastIndexOf('.');
                    String ext = ""; // ��������� ���������� �����
                    if (dotPosition == -1)
                    {
                        result[nameof(model.UserAvatar)] = "File without extension not allowed";
                    }
                    else
                    {
                        ext = model.UserAvatar.FileName[dotPosition..].ToLower();
                        if (!new[] { ".png", ".jpg", ".jpeg", ".svg", ".bmp", ".gif", ".webp" }.Contains(ext))
                        {
                            result[nameof(model.UserAvatar)] = "File type not allowed. Allowed types are .png, .jpg, .jpeg, .svg, .bmp, .gif, .webp";
                        }
                    }
                    String path = Directory.GetCurrentDirectory() + "/wwwroot/img/avatars/";
                    _logger.LogInformation(path);
                    String fileName;
                    String pathName;
                    do
                    {
                        fileName = Guid.NewGuid().ToString() + ext;
                        pathName = path + fileName;
                    }
                    while (System.IO.File.Exists(pathName));

                    using var stream = System.IO.File.OpenWrite(pathName);
                    model.UserAvatar.CopyTo(stream);

                    model.SavedAvatarFilename = fileName;
                }
            }
            return result;
        }


    }
}
