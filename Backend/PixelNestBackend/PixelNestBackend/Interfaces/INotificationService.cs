﻿using PixelNestBackend.Dto;
using PixelNestBackend.Dto.Projections;

namespace PixelNestBackend.Interfaces
{
    public interface INotificationService
    {
        ICollection<ResponseNotificationsDto> GetNotifications(string userGuid);
        int CountNotifications(string userGuid);
        bool MarkAsOpened(MarkAsOpenedDto markAsrReadDto, string userGuid);
    }
}
