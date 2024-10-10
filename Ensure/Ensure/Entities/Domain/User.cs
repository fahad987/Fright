using System.ComponentModel.DataAnnotations;
using Ensure.Entities.Enum;

namespace EnsureFreightInc.Entities.Domain;

public class User
{
    public Guid id { get; set; }=Guid.Empty;

    [Required(ErrorMessage = "First name required")]
    [MaxLength(50, ErrorMessage = "Firstname can not be more than {1} characters")]
    public string firstName { get; set; } = string.Empty;
    public TitleEnum titleId { get; set; } = TitleEnum.Mr;

    [MaxLength(50,ErrorMessage = "Lastname can not be more than {1} characters")]
    public string lastName { get; set; } = string.Empty;
   
    [MaxLength(100,ErrorMessage = "Personal email can not be more than {1} characters")]
    public string personalEmailAddress { get; set; } = string.Empty;
    
    [MaxLength(50,ErrorMessage = "CNIC can not be more than {1} characters")]
    public string cnic { get; set; } = string.Empty;
    
    [MaxLength(50,ErrorMessage = "Home phone no can not be more than {1} characters")]
    public string homePhoneNo { get; set; } = string.Empty;
    
    [MaxLength(50,ErrorMessage = "Mobile phone no can not be more than {1} characters")]
    public string mobilePhoneNo { get; set; } = string.Empty;
    [MaxLength(100,ErrorMessage = "Address no can not be more than {1} characters")]
    public string address { get; set; } = string.Empty;
    public Guid cityId { get; set; } = Guid.Empty;
    public string city { get; set; } = string.Empty;
    [MaxLength(100,ErrorMessage = "Province phone no can not be more than {1} characters")]
    public string province { get; set; } = string.Empty;
    public MaritalStatusEnum maritalStatusId { get; set; } = MaritalStatusEnum.Single;
    
    [MaxLength(50,ErrorMessage = "Emergency contact no can not be more than {1} characters")]
    public string emergencyContactNo { get; set; } = string.Empty;
    [Required(ErrorMessage = "Email required")]
    [MaxLength(100,ErrorMessage = "Email can not be more than {1} characters")]
    public string email { get; set; } = string.Empty;
    
    public List<UserRole> roles { get; set; }=new List<UserRole>();
    public Guid branchId { get; set; } = Guid.Empty;
    public string branch { get; set; } = string.Empty;
    public Guid imageId { get; set; }=Guid.Empty;
    public IFormFile? imageFile { get; set; } = null;
    public string image { get; set; } = string.Empty;

}