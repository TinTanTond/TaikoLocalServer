using System.Text.Json;
using TaikoWebUI.Pages.Dialogs;

namespace TaikoWebUI.Components;

public partial class UserCard
{
    [Parameter] public User? User { get; set; }
    [Parameter] public UserSetting? UserSetting { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
    }

    private Task ShowQrCode(User user)
    {
        var parameters = new DialogParameters
        {
            ["user"] = user
        };

        var options = new DialogOptions { DisableBackdropClick = true };
        DialogService.Show<UserQrCodeDialog>(Localizer["QR Code"], parameters, options);

        return Task.CompletedTask;
    }

    private async Task ResetPassword(User user)
    {
        var options = new DialogOptions { DisableBackdropClick = true };
        if (AuthService.LoginRequired && !AuthService.IsAdmin)
        {
            await DialogService.ShowMessageBox(
                Localizer["Error"],
                "Only admin can reset password.",
                Localizer["Dialog OK"], null, null, options);
            return;
        }

        var parameters = new DialogParameters
        {
            ["user"] = user
        };

        var dialog =
            await DialogService.ShowAsync<ResetPasswordConfirmDialog>(Localizer["Reset Password"], parameters, options);
        await dialog.Result;
    }

    private async Task DeleteUser(User user)
    {
        var options = new DialogOptions { DisableBackdropClick = true };
        if (!AuthService.AllowUserDelete)
        {
            await DialogService.ShowMessageBox(
                Localizer["Error"],
                "User deletion is disabled by admin.",
                Localizer["Dialog OK"], null, null, options);
            return;
        }

        var parameters = new DialogParameters
        {
            ["user"] = user
        };

        var dialog =
            await DialogService.ShowAsync<UserDeleteConfirmDialog>((string)Localizer["Delete User"], parameters,
                options);
        var result = await dialog.Result;

        if (result.Canceled) return;

        if (user.Baid == AuthService.GetLoggedInBaid())
        {
            await AuthService.Logout();
        }

        NavigationManager.NavigateTo("/Users", true);
    }

    private async Task GenerateInviteCode(uint baid)
    {
        var request = new GenerateOtpRequest
        {
            Baid = baid
        };

        var responseMessage = await Client.PostAsJsonAsync("api/Auth/GenerateOtp", request);

        if (!responseMessage.IsSuccessStatusCode)
        {
            await DialogService.ShowMessageBox(
                Localizer["Error"],
                Localizer["Unknown Error"],
                Localizer["Dialog OK"], null, null, new DialogOptions { DisableBackdropClick = true });
            return;
        }

        var responseContent = await responseMessage.Content.ReadAsStringAsync();
        var responseJson = JsonSerializer.Deserialize<Dictionary<string, string>>(responseContent);
        if (responseJson == null)
        {
            await DialogService.ShowMessageBox(
                Localizer["Error"],
                Localizer["Unknown Error"],
                Localizer["Dialog OK"], null, null, new DialogOptions { DisableBackdropClick = true });
            return;
        }

        var otp = responseJson["otp"];

        var parameters = new DialogParameters
        {
            ["otp"] = otp
        };

        var options = new DialogOptions { DisableBackdropClick = true };
        await DialogService.ShowAsync<OTPDialog>(Localizer["Invite Code"], parameters, options);
    }
}