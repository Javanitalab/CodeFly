using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataAccess.Models;

namespace CodeFly.DTO;

public class UserQuestDTO
{
    public static UserQuestDTO Create(Userquest userquest)
    {
        var dto = new UserQuestDTO();
        dto.Id = userquest.Id;
        dto.Title = userquest.Quest.Title;
        dto.EndDate = (userquest.Quest.EndDate);
        dto.NeededProgress = userquest.Quest.NeededProgress;
        dto.Completed = userquest.Quest.Completed;
        dto.RewardType = userquest.Quest.RewardType;
        dto.RewardValue = (short)userquest.Quest.RewardValue;

        dto.Progress = userquest.Progress;

        return dto;
    }

    public int Id { get; set; }
    public string Title { get; set; }
    public string EndDate { get; set; }
    public int Progress { get; set; }
    public int NeededProgress { get; set; }
    public int RewardType { get; set; }
    public short RewardValue { get; set; }
    public bool Completed { get; set; }
}