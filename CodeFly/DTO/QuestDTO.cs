using System;
using System.Collections;
using DataAccess.Enums;
using DataAccess.Models;

namespace CodeFly.DTO;

public class QuestDTO
{
    public static QuestDTO Create(Quest quest)
    {
        var dto = new QuestDTO();
        dto.Id = quest.Id;
        dto.Title = quest.Title;
        dto.NeededProgress = quest.NeededProgress;
        dto.Completed = quest.Completed;
        dto.RewardType = (RewardType)quest.RewardType;
        dto.RewardValue = (short)quest.RewardValue;
        dto.QuestType = (QuestType)quest.QuestType;
        dto.EndDate = quest.EndDate;

        return dto;
    }

    public int Id { get; set; }

    public string Title { get; set; }

    public string EndDate { get; set; }

    public QuestType QuestType { get; set; }

    public int NeededProgress { get; set; }

    public bool Completed { get; set; }

    public RewardType RewardType { get; set; }

    public short RewardValue { get; set; }
}