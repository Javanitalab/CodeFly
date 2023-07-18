using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataAccess.Models;

namespace CodeFly.DTO;

public class UserQuestDTO
{
    public static UserQuestDTO Create(Userquest quest)
    {
        var dto = new UserQuestDTO();
        dto.Id = quest.Id;
        dto.Title = quest.Quest.Title;
        if (quest.Quest.EndDate != null) dto.EndDate = (DateOnly)quest.Quest.EndDate;
        dto.NeededProgress = quest.Quest.NeededProgress;
        if (quest.Quest.Completed != null) dto.Completed = (bool)quest.Quest.Completed;
        dto.RewardType = quest.Quest.RewardType;
        if (quest.Quest.RewardValue != null) dto.RewardValue = (short)quest.Quest.RewardValue;

        var progress = quest.UserquestUserlessons.Select(uqul => uqul.Userlesson.Lesson.Id).ToList();
        dto.Progress = progress;

        return dto;
    }

    public int Id { get; set; }
    public string Title { get; set; }
    public DateOnly EndDate { get; set; }
    public List<int> Progress { get; set; }
    public BitArray NeededProgress { get; set; }
    public BitArray RewardType { get; set; }
    public short RewardValue { get; set; }
    public bool Completed { get; set; }
}