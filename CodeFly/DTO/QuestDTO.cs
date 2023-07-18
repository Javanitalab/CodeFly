using System;
using System.Collections;
using DataAccess.Models;

namespace CodeFly.DTO;

public class QuestDTO
{
    public static QuestDTO Create(Quest quest)
    {
        var dto = new QuestDTO();
        dto.Id = quest.Id;
        dto.Title = quest.Title;
        if (quest.EndDate != null) dto.EndDate = (DateOnly)quest.EndDate;
        dto.NeededProgress = quest.NeededProgress;
        if (quest.Completed != null) dto.Completed = (bool)quest.Completed;
        dto.RewardType = quest.RewardType;
        if (quest.RewardValue != null) dto.RewardValue = (short)quest.RewardValue;

        return dto;
    }

    public int Id { get; set; }

    public string Title { get; set; }

    public DateOnly EndDate { get; set; }

    public BitArray NeededProgress { get; set; }

    public bool Completed { get; set; }

    public BitArray RewardType { get; set; }

    public short RewardValue { get; set; }
}