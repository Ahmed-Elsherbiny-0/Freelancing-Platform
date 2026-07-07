using Domain.Comman;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Comman
{
    public static class PhotoErrors
    {
        public static readonly Error PhotoNotFounded = new("Photo.notFounded", "Photo Not Founded ..photo may be deleted!", StatusCodes.Status404NotFound);
        public static readonly Error PhotoNotSave = new Error("Photo.error", "Photo unsave ...try again later ", StatusCodes.Status400BadRequest);
    
    }
}
