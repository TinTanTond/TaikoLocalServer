namespace Application.Models.Game;

public class CommonGetSongIntroductionResponse
{
    public uint Result { get; set; }

    public List<SongIntroductionData> ArySongIntroductionDatas { get; set; } = [];
}