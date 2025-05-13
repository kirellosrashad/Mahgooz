
namespace STGeorgeReservation.Constants;

public partial class Constants
{
    public const string ArabicLanguage = "ar-SA";
    public const string Arabic = "ar";

    public const string EnglishLanguage = "en-US";
    public const string English = "en";

    public const string EmailRegex = @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
               @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$";
    public const string SuperAdminNameEn = "Super Admin";
    public const string SuperAdminNameAr = "المشرف الرئيسى";
    public const string TenantClaimType = "tenant";
    public const string Issuer = "HRCom-Api";
    public const string Audience = "HRCom-Api";
    public const string SecretKey = "2d98a347-3adb-445f-a26c-a0c3d041df82";
    public const string UserPortalRouteApiPrefix = "user";
    public const string SalaryGradeExcelName = "Salary_Grade_Sample.xlsx";
    public const string SalaryGradeFileExcelName = "SalaryGradeFile";
    public const string ActionPermission = "action_permission";
    public const string DefaultDbConnectionName = "DbConnection";
    public const string AppURL = "App_Base_URL";


}
