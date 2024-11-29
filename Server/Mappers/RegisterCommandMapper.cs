using Application.Handlers.Api.Auth;
using Shared.Models.Requests;

namespace Server.Mappers;

[Mapper]
public static partial class RegisterCommandMapper
{
    public static partial RegisterCommand ToCommand(RegisterRequest request);
}