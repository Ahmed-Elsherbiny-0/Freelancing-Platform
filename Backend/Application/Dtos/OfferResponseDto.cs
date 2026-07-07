using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public record   OfferResponseDto(string Description, DateTime Duration, int Price, bool IsApproved, 
        bool IsCompleted, int JobId,decimal Rating, int CompletedTasks,string PictureUrl,string FristName,string LastName,string WorkerEamil,
        string? JobTitle,string? WorkerId 
        );
}
