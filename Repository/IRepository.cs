using Dtos;

namespace Repository;
public interface IUserRepository
{
    Task<OnboardingFileDto> GetOnboardingFile(string id);
    Task<List<ProfileDto>> GetProfiles();
    Task UploadProfile(string name, string phoneNumber, byte[] file, string fileType);
}