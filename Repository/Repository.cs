
using Dtos;
using Microsoft.Extensions.Options;
using Npgsql;
using Npgsql.Internal;

namespace Repository;
public class UserRepository(IOptions<EnvConfig.EnvConfig> options) : IUserRepository
{
    private EnvConfig.EnvConfig EnvConfig => options.Value;

    public async Task<OnboardingFileDto> GetOnboardingFile(string id)
    {
        await using var conn = new NpgsqlConnection(EnvConfig.Db);
        await conn.OpenAsync();

        await using var cmd = new NpgsqlCommand("SELECT file, name, file_type FROM myschema.\"users\" WHERE id = @p1", conn);
        cmd.Parameters.AddWithValue("p1", NpgsqlTypes.NpgsqlDbType.Integer, int.Parse(id));
        await using var reader = await cmd.ExecuteReaderAsync();
        string name = "";
        string fileType = "";
        byte[] byteArr = [];
        while (await reader.ReadAsync())
        {
            byteArr = (byte[])reader[0];
            name = reader.GetString(1);
            fileType = reader.GetString(2);
        }
        return new OnboardingFileDto{
            Name = name,
            FileType = fileType,
            File = byteArr
        };
    }

    public async Task<List<ProfileDto>> GetProfiles()
    {
        await using var conn = new NpgsqlConnection(EnvConfig.Db);
        await conn.OpenAsync();

        await using var cmd = new NpgsqlCommand("SELECT id, name, phone_number FROM myschema.\"users\"", conn);
        await using var reader = await cmd.ExecuteReaderAsync();
        List<ProfileDto> res = [];
        while (await reader.ReadAsync())
        {
            res.Add(new ProfileDto{
                Id = reader.GetString(0),
                Name = reader.GetString(1),
                PhoneNumber = reader.GetString(2)
            });
        }
        return res;
    }

    public async Task UploadProfile(string name, string phoneNumber, byte[] file, string fileType)
    {
        await using var conn = new NpgsqlConnection(EnvConfig.Db);
        await conn.OpenAsync();

        // Insert some data
        await using var cmd = new NpgsqlCommand("INSERT INTO myschema.\"users\" (name, phone_number, file, file_type) VALUES (@p1, @p2, @p3, @p4)", conn);
        cmd.Parameters.AddWithValue("p1", name);
        cmd.Parameters.AddWithValue("p2", phoneNumber);
        cmd.Parameters.AddWithValue("p3", NpgsqlTypes.NpgsqlDbType.Bytea, file);
        cmd.Parameters.AddWithValue("p4", fileType);
        await cmd.ExecuteNonQueryAsync();

    }
}