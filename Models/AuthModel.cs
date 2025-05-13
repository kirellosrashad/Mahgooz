using System.Text.Json.Serialization;

namespace STGeorgeReservation.Models
{
    public class AuthModel
    {
        public string Message { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public string UserID { get; set; }
        public string Roles { get; set; }
        public bool IsAuthenticated { get; set; }
        public DateTime ExpiresOn { get; set; }
        public bool? IsSuperAdmin { get; set; }

        // Updated to hold only the flat list of permissions
        public IEnumerable<UserProfileRolePermissionsResponseDTO> PermissionsList { get; set; }
    }

    public class UserProfileRolePermissionsResponseDTO
    {
        [JsonPropertyName("name_en")]
        public string NameEn { get; set; }

        [JsonPropertyName("claim_value")]
        public string ClaimValue { get; set; }
    }
}
