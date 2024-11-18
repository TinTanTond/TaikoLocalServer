namespace Application.Models.Game;

public class CommonGetFolderResponse
{
    public uint Result { get; set; }

    public List<EventFolderData> AryEventfolderDatas { get; set; } = [];
}